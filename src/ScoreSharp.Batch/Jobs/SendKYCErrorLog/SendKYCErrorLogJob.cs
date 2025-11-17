using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.EntityFrameworkCore;
using ScoreSharp.Batch.Infrastructures.Adapter.Models;
using ScoreSharp.RazorTemplate.Mail.KYCErrorLog;

namespace ScoreSharp.Batch.Jobs.SendKYCErrorLog;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("寄信KYC錯誤")]
[WorkdayCheck]
public class SendKYCErrorLogJob(
    ILogger<SendKYCErrorLogJob> logger,
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
            if (systemBatchSet!.SendKYCErrorLog_IsEnabled == "N")
            {
                logger.LogInformation("系統參數設定不執行【寄信KYC錯誤】排程，執行結束");
                return;
            }

            logger.LogInformation("查詢KYC錯誤");
            var peddingLogs = await GetPendingLogs();

            if (peddingLogs.Count == 0)
            {
                logger.LogInformation("沒有需要寄信的KYC錯誤");
                return;
            }

            logger.LogInformation("查詢寄送KYC錯誤_寄信設定");
            var mailSet = await GetMailSet();

            if (mailSet == null)
                throw new Exception("寄送KYC錯誤_寄信設定不存在");

            logger.LogInformation("開始執行寄信");

            int BATCH_SIZE = systemBatchSet.SendKYCErrorLog_BatchSize;
            logger.LogInformation("批次大小：{0}", BATCH_SIZE);

            var batcheLogs = peddingLogs.Chunk(BATCH_SIZE).ToList();
            logger.LogInformation("批次數量：{0}", batcheLogs.Count);
            logger.LogInformation("信件：{0}", peddingLogs.Count);

            var to = mailSet.KYCErrorLog_To.Split(',').ToList();
            var subject = mailSet.KYCErrorLog_Title;
            var template = mailSet.KYCErrorLog_Template;

            logger.LogInformation("寄信對象：{0}", to);
            logger.LogInformation("主旨：{0}", subject);
            logger.LogInformation("模板：{0}", template);

            for (int idx = 0; idx < batcheLogs.Count; idx++)
            {
                var batch = batcheLogs[idx];
                try
                {
                    var kycErrlogs = batch.Select(x => MapToKYCErrorLogDto(x)).ToList();

                    var renderedView = await razorTemplateEngine.RenderAsync(
                        template,
                        new KYCErrorLogViewModel() { KYCErrorLogViewModels = kycErrlogs }
                    );

                    if (env.IsDevelopment())
                    {
                        await SendEmailAdapterAsync(subject, renderedView, to);
                    }
                    else
                    {
                        await SendEmailAsync(subject, renderedView, to);
                    }

                    await UpdateSendStatus(batch.ToList(), DateTime.Now, KYCLastSendStatus.成功);
                    logger.LogInformation($"寄信成功({idx})");
                }
                catch (Exception ex)
                {
                    await UpdateSendStatus(batch.ToList(), DateTime.Now, KYCLastSendStatus.失敗);
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

    private async Task<List<Reviewer3rd_KYCQueryLog>> GetPendingLogs() =>
        await context.Reviewer3rd_KYCQueryLog.AsNoTracking().Where(x => x.KYCLastSendStatus == KYCLastSendStatus.等待).ToListAsync();

    private async Task<SysParamManage_MailSet> GetMailSet() => await context.SysParamManage_MailSet.FirstOrDefaultAsync();

    private KYCErrorLogDto MapToKYCErrorLogDto(Reviewer3rd_KYCQueryLog log) =>
        new()
        {
            SeqNo = log.SeqNo.ToString() ?? string.Empty,
            ApplyNo = log.ApplyNo.ToString() ?? string.Empty,
            CardStatus = log.CardStatus.ToString() ?? string.Empty,
            CurrentHandler = log.CurrentHandler.ToString() ?? string.Empty,
            ID = log.ID.ToString() ?? string.Empty,
            KYCCode = log.KYCCode?.ToString() ?? string.Empty,
            KYCRank = log.KYCRank?.ToString() ?? string.Empty,
            KYCMsg = log.KYCMsg?.ToString() ?? string.Empty,
            Request = log.Request?.ToString() ?? string.Empty,
            Response = log.Response?.ToString() ?? string.Empty,
            AddTime = log.AddTime.ToString(),
            APIName = log.APIName?.ToString() ?? string.Empty,
            Source = log.Source?.ToString() ?? string.Empty,
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

    private async Task UpdateSendStatus(List<Reviewer3rd_KYCQueryLog> logs, DateTime sendTime, KYCLastSendStatus status)
    {
        logs.ForEach(x =>
        {
            x.KYCLastSendStatus = status;
            x.KYCLastSendTime = sendTime;
        });
        context.Reviewer3rd_KYCQueryLog.UpdateRange(logs);
        await context.SaveChangesAsync();
        context.ChangeTracker.Clear();
    }
}
