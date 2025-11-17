using FluentEmail.Core;
using FluentEmail.Core.Models;
using ScoreSharp.Batch.Infrastructures.Adapter.Models;
using ScoreSharp.RazorTemplate.Mail.SystemErrorLog;

namespace ScoreSharp.Batch.Jobs.SendSystemErrorLog;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("寄信系統錯誤")]
[WorkdayCheck]
public class SendSystemErrorLogJob(
    ILogger<SendSystemErrorLogJob> logger,
    ScoreSharpContext context,
    IHostEnvironment env,
    IEmailAdapter emailAdapter,
    IRazorTemplateEngine razorTemplateEngine,
    IFluentEmail fluentEmail
)
{
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("寄信系統錯誤 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var systemBatchSet = await context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();
            if (systemBatchSet!.SendSystemErrorLog_IsEnabled == "N")
            {
                logger.LogInformation("系統參數設定不執行【寄信系統錯誤】排程，執行結束");
                return;
            }

            logger.LogInformation("查詢系統錯誤");
            var peddingLogs = await GetPendingLogs();

            if (peddingLogs.Count == 0)
            {
                logger.LogInformation("沒有需要寄信的系統錯誤");
                return;
            }

            logger.LogInformation("查詢寄送系統錯誤_寄信設定");
            var mailSet = await GetMailSet();

            if (mailSet == null)
                throw new Exception("寄送系統錯誤_寄信設定不存在");

            logger.LogInformation("開始執行寄信");

            int BATCH_SIZE = systemBatchSet.SendSystemErrorLog_BatchSize;
            logger.LogInformation("批次大小：{0}", BATCH_SIZE);

            var batcheLogs = peddingLogs.Chunk(BATCH_SIZE).ToList();
            logger.LogInformation("批次數量：{0}", batcheLogs.Count);
            logger.LogInformation("信件：{0}", peddingLogs.Count);

            var to = mailSet.SystemErrorLog_To.Split(',').ToList();
            var subject = mailSet.SystemErrorLog_Title;
            var template = mailSet.SystemErrorLog_Template;

            logger.LogInformation("寄信對象：{0}", to);
            logger.LogInformation("主旨：{0}", subject);
            logger.LogInformation("模板：{0}", template);

            for (int idx = 0; idx < batcheLogs.Count; idx++)
            {
                var batch = batcheLogs[idx];
                try
                {
                    var sysErrlogs = batch.Select(x => MapToSystemErrorLogDto(x)).ToList();

                    var renderedView = await razorTemplateEngine.RenderAsync(
                        template,
                        new SystemErrorLogViewModel() { SystemErrorLogViewModels = sysErrlogs }
                    );

                    if (env.IsDevelopment())
                    {
                        await SendEmailAdapterAsync(subject, renderedView, to);
                    }
                    else
                    {
                        await SendEmailAsync(subject, renderedView, to);
                    }

                    await UpdateSendStatus(batch.ToList(), DateTime.Now, SendStatus.成功);
                    logger.LogInformation($"寄信成功({idx})");
                }
                catch (Exception ex)
                {
                    await UpdateSendStatus(batch.ToList(), DateTime.Now, SendStatus.失敗, ex.ToString());
                    logger.LogError($"寄信失敗({idx})，批次時發生錯誤訊息 / {ex.Message}");
                    logger.LogError($"寄信失敗({idx})，批次時發生錯誤詳細訊息 / {ex.ToString()}");
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError("寄信失敗:{ex}", ex);
        }
        finally
        {
            semaphore.Release();
        }
    }

    private async Task<List<System_ErrorLog>> GetPendingLogs()
    {
        return await context.System_ErrorLog.AsNoTracking().Where(x => x.SendStatus == SendStatus.等待).ToListAsync();
    }

    private async Task<SysParamManage_MailSet> GetMailSet()
    {
        return await context.SysParamManage_MailSet.FirstOrDefaultAsync();
    }

    private SystemErrorLogDto MapToSystemErrorLogDto(System_ErrorLog log) =>
        new()
        {
            SeqNo = log.SeqNo.ToString(),
            ApplyNo = log.ApplyNo?.ToString(),
            Project = log.Project.ToString(),
            Source = log.Source.ToString(),
            Type = log.Type?.ToString(),
            ErrorMessage = log.ErrorMessage.ToString(),
            ErrorDetail = log.ErrorDetail?.ToString(),
            Request = log.Request?.ToString(),
            Response = log.Response?.ToString(),
            AddTime = log.AddTime.ToString(),
            Note = log.Note?.ToString(),
        };

    private async Task SendEmailAdapterAsync(string subject, string body, List<string> to)
    {
        var request = new SendEmailRequest()
        {
            To = to.Select(x => new EmailAddressDto() { Name = "", Address = x }).ToList(),
            Subject = subject,
            Body = body,
            IsHtml = true,
        };
        var result = await emailAdapter.SendEmailAsync(request);

        if (!result.IsSuccess)
        {
            throw new Exception($"status: {result.IsSuccess} / error: {string.Join(",", result.ErrorMessage)}");
        }
    }

    private async Task SendEmailAsync(string subject, string body, List<string> to)
    {
        // 🔔 因套件 Data 在同個上下文共用，因此需清理Data，也可以使用 IFluentEmailFactory 來建立新的 Email 實例
        fluentEmail.Data.ToAddresses.Clear();
        var result = await fluentEmail.To(to.Select(x => new Address(x, "")).ToArray()).Subject(subject).Body(body, true).SendAsync();
        if (!result.Successful)
        {
            throw new Exception($"status: {result.Successful} / error: {string.Join(",", result.ErrorMessages)}");
        }
    }

    private async Task UpdateSendStatus(List<System_ErrorLog> logs, DateTime sendTime, SendStatus status, string? errorMessage = null)
    {
        logs.ForEach(x =>
        {
            x.SendStatus = status;
            x.SendEmailTime = sendTime;
            x.FailLog = errorMessage;
        });
        context.System_ErrorLog.UpdateRange(logs);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}
