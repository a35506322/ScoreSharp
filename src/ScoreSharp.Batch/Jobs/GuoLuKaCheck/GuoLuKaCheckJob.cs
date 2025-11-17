using DocumentFormat.OpenXml.Wordprocessing;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Newtonsoft.Json;
using ScoreSharp.Batch.Infrastructures.Adapter.Models;
using ScoreSharp.RazorTemplate.Mail.ReviewerLogGuoLuKaCheckFail;

namespace ScoreSharp.Batch.Jobs.GuoLuKaCheck;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("國旅卡人士資料檢核排程")]
[WorkdayCheck]
public class GuoLuKaCheckJob(
    ScoreSharpContext _context,
    IScoreSharpDapperContext _dapperContext,
    ILogger<GuoLuKaCheckJob> _logger,
    IMW3ProcAdapter _mW3ProcAdapter,
    IHostEnvironment _env,
    IEmailAdapter _emailAdapter,
    IRazorTemplateEngine _razorTemplateEngine,
    IFluentEmail _fluentEmail
)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("國旅卡人士資料檢核排程 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await _semaphore.WaitAsync(0))
        {
            _logger.LogInformation("上一個批次任務還在執行中，本次執行已取消。");
            return;
        }

        try
        {
            var systemBatchSet = await _context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();

            if (systemBatchSet!.GuoLuKaCheck_IsEnabled == "N")
            {
                _logger.LogInformation("系統參數設定不執行【國旅卡人士資料檢核】排程，執行結束");
                return;
            }

            var 系統撤件天數 = systemBatchSet!.GuoLuKaCaseWithdrawDays;
            var 檢核案件數 = systemBatchSet!.GuoLuKaCheck_BatchSize;

            _logger.LogInformation("系統撤件天數：{Days}", 系統撤件天數);
            _logger.LogInformation("排程檢核案件數：{Count}", 檢核案件數);

            var 國旅卡待檢核案件 = await 取得國旅卡申請案件(檢核案件數);

            if (國旅卡待檢核案件.Count == 0)
            {
                _logger.LogInformation("查無「國旅人士名冊確認」狀態的案件，執行結束");
                return;
            }

            _logger.LogInformation("取得國旅卡案件數量：{Count}", 國旅卡待檢核案件.Count);

            foreach (var item in 國旅卡待檢核案件)
            {
                _logger.LogInformation("檢核國旅卡客戶資訊 申請書編號：{ApplyNo}", item.ApplyNo);

                var (guoLuKaCheckResult, mw3Response) = await 檢核_查詢國旅卡客戶資訊(item.ID);

                using var transaction = await _context.Database.BeginTransactionAsync();
                try
                {
                    if (guoLuKaCheckResult)
                    {
                        _logger.LogInformation("國旅卡客戶資訊正確，更新案件狀態為「網路件初始_非卡友_待檢核」");

                        await _context
                            .Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == item.ApplyNo)
                            .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CardStatus, CardStatus.網路件_非卡友_待檢核));

                        await _context
                            .Reviewer_ApplyCreditCardInfoMain.Where(x => x.ApplyNo == item.ApplyNo)
                            .ExecuteUpdateAsync(setters =>
                                setters.SetProperty(x => x.LastUpdateUserId, UserIdConst.SYSTEM).SetProperty(x => x.LastUpdateTime, DateTime.Now)
                            );

                        await _context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(
                            new()
                            {
                                ApplyNo = item.ApplyNo,
                                Process = CardStatus.網路件_非卡友_待檢核.ToString(),
                                StartTime = DateTime.Now,
                                EndTime = DateTime.Now,
                                Notes = "國旅卡客戶資訊檢核成功",
                                ProcessUserId = UserIdConst.SYSTEM,
                            }
                        );
                    }
                    else
                    {
                        _logger.LogInformation("國旅卡客戶資訊有誤，新增國旅卡檢核失敗Log");

                        // 紀錄國旅卡客戶資訊檢核失敗Log
                        await _context.ReviewerLog_GuoLuKaCheckFail.AddAsync(
                            new()
                            {
                                ApplyNo = item.ApplyNo,
                                ID = item.ID,
                                Response = mw3Response,
                                CreateTime = DateTime.Now,
                                SendStatus = SendStatus.等待,
                            }
                        );

                        // 若超過撤件天數，則更新案件狀態為「國旅人士名冊確認_系統撤件」
                        if (item.ApplyDate.AddDays(系統撤件天數) < DateTime.Now)
                        {
                            _logger.LogInformation("案件已超過撤件天數，更新案件狀態為「國旅人士名冊確認_系統撤件」");

                            await _context
                                .Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == item.ApplyNo)
                                .ExecuteUpdateAsync(setters => setters.SetProperty(x => x.CardStatus, CardStatus.國旅人士名冊確認_系統撤件));

                            await _context
                                .Reviewer_ApplyCreditCardInfoMain.Where(x => x.ApplyNo == item.ApplyNo)
                                .ExecuteUpdateAsync(setters =>
                                    setters.SetProperty(x => x.LastUpdateUserId, UserIdConst.SYSTEM).SetProperty(x => x.LastUpdateTime, DateTime.Now)
                                );

                            await _context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(
                                new Reviewer_ApplyCreditCardInfoProcess()
                                {
                                    ApplyNo = item.ApplyNo,
                                    Process = CardStatus.國旅人士名冊確認_系統撤件.ToString(),
                                    StartTime = DateTime.Now,
                                    EndTime = DateTime.Now,
                                    Notes = ProcessNoteConst.國旅人士名冊確認超過撤件天數,
                                    ProcessUserId = UserIdConst.SYSTEM,
                                }
                            );
                        }
                    }

                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                    _context.ChangeTracker.Clear();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"國旅卡客戶檢核排程有誤，批次時發生錯誤訊息 / {ex.Message}");
                    _logger.LogError($"國旅卡客戶檢核排程有誤，批次時發生錯誤詳細訊息 / {ex.ToString()}");
                    await 新增系統異常Log(SystemErrorLogTypeConst.儲存資料庫有誤, ex.Message, ex.ToString(), JsonConvert.SerializeObject(item));
                }
            }

            _logger.LogInformation("查詢國旅卡檢核排程失敗Log");
            var logs = await 取得國旅卡檢核排程失敗Log();

            if (logs.Count == 0)
            {
                _logger.LogInformation("沒有需要寄信的國旅卡檢核失敗Log");
                return;
            }

            _logger.LogInformation("查詢國旅卡客戶檢核失敗_寄信設定");
            var mailSet = await 取得寄信設定();

            if (mailSet is null)
                throw new Exception("國旅卡客戶檢核失敗_寄信設定不存在");

            _logger.LogInformation("開始執行寄信");
            _logger.LogInformation("Log 數量：{Count}", logs.Count);

            var to = mailSet.GuoLuKaCheckFailLog_To.Split(',').ToList();
            var subject = mailSet.GuoLuKaCheckFailLog_Title;
            var template = mailSet.GuoLuKaCheckFailLog_Template;

            _logger.LogInformation("寄信對象：{To}", to);
            _logger.LogInformation("主旨：{Subject}", subject);
            _logger.LogInformation("模板：{Template}", template);

            try
            {
                var dto = logs.Select(log => MapToReviewerLogGuoLuKaCheckFailDto(log)).ToList();

                var renderedView = await _razorTemplateEngine.RenderAsync(
                    template,
                    new ReviewerLogGuoLuKaCheckFailViewModel() { ReviewerLogGuoLuKaCheckFailDtos = dto }
                );

                if (_env.IsDevelopment())
                {
                    await SendEmailAdapterAsync(subject, renderedView, to);
                }
                else
                {
                    await SendEmailAsync(subject, renderedView, to);
                }

                await UpdateSendStatus(logs, DateTime.Now, SendStatus.成功, "");
                _logger.LogInformation($"寄信成功");
            }
            catch (Exception ex)
            {
                _logger.LogError($"寄信失敗，批次時發生錯誤訊息 / {ex.Message}");
                _logger.LogError($"寄信失敗，批次時發生錯誤詳細訊息 / {ex.ToString()}");
                _context.ChangeTracker.Clear();
                await UpdateSendStatus(logs, DateTime.Now, SendStatus.失敗, ex.ToString());
                throw;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"國旅卡客戶檢核排程有誤，批次時發生錯誤訊息 / {ex.Message}");
            _logger.LogError($"國旅卡客戶檢核排程有誤，批次時發生錯誤詳細訊息 / {ex.ToString()}");
            await 新增系統異常Log(SystemErrorLogTypeConst.內部程式錯誤, ex.Message, ex.ToString());
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task UpdateSendStatus(List<ReviewerLog_GuoLuKaCheckFail> logs, DateTime sendTime, SendStatus updateStatus, string errorMessage)
    {
        logs.ForEach(x =>
        {
            x.SendStatus = updateStatus;
            x.SendTime = sendTime;
            x.SendFailLog = errorMessage;
        });

        _context.ReviewerLog_GuoLuKaCheckFail.UpdateRange(logs);
        await _context.SaveChangesAsync();
    }

    private async Task SendEmailAsync(string subject, string body, List<string> to)
    {
        var result = await _fluentEmail.To(to.Select(x => new Address(x, "")).ToArray()).Subject(subject).Body(body, true).SendAsync();

        if (!result.Successful)
        {
            throw new Exception($"status: {result.Successful} / error: {string.Join(",", result.ErrorMessages)}");
        }
    }

    private async Task SendEmailAdapterAsync(string subject, string body, List<string> to)
    {
        var request = new SendEmailRequest()
        {
            To = to.Select(x => new EmailAddressDto() { Name = "", Address = x }).ToList(),
            Subject = subject,
            Body = body,
            IsHtml = true,
        };
        var result = await _emailAdapter.SendEmailAsync(request);

        if (!result.IsSuccess)
        {
            throw new Exception($"status: {result.IsSuccess} / error: {string.Join(",", result.ErrorMessage)}");
        }
    }

    private ReviewerLogGuoLuKaCheckFailDto MapToReviewerLogGuoLuKaCheckFailDto(ReviewerLog_GuoLuKaCheckFail log) =>
        new()
        {
            SeqNo = log.SeqNo.ToString(),
            ApplyNo = log.ApplyNo,
            ID = log.ID,
            Response = log.Response,
            CreateTime = log.CreateTime.ToString(),
        };

    private async Task<SysParamManage_MailSet> 取得寄信設定() => await _context.SysParamManage_MailSet.FirstOrDefaultAsync();

    private async Task<List<ReviewerLog_GuoLuKaCheckFail>> 取得國旅卡檢核排程失敗Log() =>
        await _context.ReviewerLog_GuoLuKaCheckFail.Where(x => x.SendStatus == SendStatus.等待).ToListAsync();

    private async Task<(bool mw3CheckResult, string? mw3Response)> 檢核_查詢國旅卡客戶資訊(string Id)
    {
        bool 檢核結果 = false;

        _logger.LogInformation("MW3 查詢國旅卡客戶資訊 檢核開始");

        var response = await _mW3ProcAdapter.QueryTravelCardCustomer(Id);

        if (response.IsSuccess)
        {
            var guoLuKaCheckResponse = response.Data!;
            var rtnCode = guoLuKaCheckResponse.RtnCode.Trim();

            _logger.LogInformation("MW3 查詢國旅卡客戶資訊成功 ID：{ID} 檢核結果：{Result}", Id, rtnCode);

            if (rtnCode == MW3RtnCodeConst.查詢國旅卡客戶資訊_正確資料)
                檢核結果 = true;
        }
        else
        {
            _logger.LogError("MW3 查詢國旅卡客戶資訊失敗 ID：{ID} 錯誤訊息：{ErrorMessage}", Id, response.ErrorMessage);
        }

        _logger.LogInformation("MW3 檢核結束");

        return (檢核結果, JsonConvert.SerializeObject(response.Data));
    }

    private async Task<List<GuoLuKaCaseContext>> 取得國旅卡申請案件(int checkGuoLuKaLimit)
    {
        try
        {
            string sql = """

                SELECT TOP (@Limit)
                    Handle.ApplyNo,
                    Handle.ID,
                    Handle.CardStatus,
                    Main.ApplyDate
                FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] Handle
                JOIN [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoMain] Main ON Handle.ApplyNo = Main.ApplyNo
                And Main.ID = Handle.ID And Main.UserType = Handle.UserType
                WHERE Handle.CardStatus = @CardStatus
                Order By Main.ApplyDate

                """;

            using var connection = _dapperContext.CreateScoreSharpConnection();
            var result = await connection.QueryAsync<GuoLuKaCaseContext>(
                sql,
                new { Limit = checkGuoLuKaLimit, CardStatus = CardStatus.國旅人事名冊確認 }
            );

            return result.ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError("查詢國旅人士名冊確認案件失敗 請查看System_ErrorLog");
            throw;
            return [];
        }
    }

    private async Task 新增系統異常Log(string type, string errorMessage, string errorDetail, string? request = null)
    {
        _context.ChangeTracker.Clear();

        await _context.System_ErrorLog.AddAsync(
            new()
            {
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "GuoLuKaCheckJob",
                Type = type,
                ErrorMessage = errorMessage,
                ErrorDetail = errorDetail,
                AddTime = DateTime.Now,
                SendStatus = SendStatus.等待,
                Request = request,
            }
        );

        await _context.SaveChangesAsync();
    }
}
