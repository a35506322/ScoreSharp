using ScoreSharp.Batch.Infrastructures.Hangfire.Filters;
using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("紙本件檢核新案件排程")]
public class PaperCheckNewCaseJob
{
    private readonly ILogger<PaperCheckNewCaseJob> _logger;
    private readonly IPaperCheckNewCaseRepository _repository;
    private readonly IPaperCheckNewCaseService _service;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);
    private List<string> _errorApplyNoList = new();

    public PaperCheckNewCaseJob(ILogger<PaperCheckNewCaseJob> logger, IPaperCheckNewCaseRepository repository, IPaperCheckNewCaseService service)
    {
        _logger = logger;
        _repository = repository;
        _service = service;
    }

    [WorkdayCheck]
    [DisplayName("紙本件檢核新案件排程 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await _semaphore.WaitAsync(1))
        {
            _logger.LogWarning("紙本件檢核新案件排程已在執行中，略過本次執行");
            return;
        }

        var startTime = DateTime.Now;
        _logger.LogInformation("紙本件檢核新案件排程開始執行 - 執行人員: {CreateBy}", createBy);

        try
        {
            var systemBatchSet = await _repository.查詢_排程設定();
            if (systemBatchSet!.PaperCheckNewCase_IsEnabled == "N")
            {
                _logger.LogInformation("系統參數設定不執行【紙本件檢核新案件】排程，執行結束");
                return;
            }
            // 工作日檢查已由 WorkdayCheckFilter 處理，此處直接執行業務邏輯

            // 1. 查詢待處理案件
            var pendingCases = await _repository.查詢_待檢核案件(systemBatchSet.PaperCheckNewCase_BatchSize);

            if (!pendingCases.Any())
            {
                _logger.LogInformation("無待處理的紙本件案件");
                return;
            }

            _logger.LogInformation("找到 {Count} 筆待處理的紙本件案件", pendingCases.Count);

            // 3. 查詢案件詳細資料
            var applyNos = pendingCases.Select(x => x.ApplyNo).ToList();
            var caseDetails = await _repository.查詢_待檢核案件詳細資料(applyNos);

            // 4. 查詢通用資料
            var addressInfos = await _repository.GetAddressInfoAsync();

            // 5. 逐筆處理案件
            foreach (var pendingCase in pendingCases)
            {
                try
                {
                    var caseDetail = caseDetails.First(x => x.ApplyNo == pendingCase.ApplyNo);
                    /*
                        PaperCheckJobContext 檢核上下文邏輯，以 IsQueryOriginalCardholderData 為例

                        IsQueryOriginalCardholderData = Y
                        - 正卡人_是否檢核原持卡人 = 否
                        - 附卡人_是否檢核原持卡人 = 否
                        - 正卡人_是否檢核原持卡人成功 = 不須檢核
                        - 附卡人_是否檢核原持卡人成功 = 不須檢核
                        - 正卡人_命中檢核原持卡人 = 不須檢核
                        - 附卡人_命中檢核原持卡人 = 不須檢核

                        IsQueryOriginalCardholderData = N
                        - 正卡人_是否檢核原持卡人 = 是
                        - 附卡人_是否檢核原持卡人 = 是
                        - 正卡人_是否檢核原持卡人成功 = 等待
                        - 附卡人_是否檢核原持卡人成功 = 等待
                        - 正卡人_命中檢核原持卡人 = 等待
                        - 附卡人_命中檢核原持卡人 = 等待

                        當檢核完
                        IsQueryOriginalCardholderData = N
                        - 正卡人_是否檢核原持卡人成功 = 成功 / 失敗
                        - 附卡人_是否檢核原持卡人成功 = 成功 / 失敗
                        - 正卡人_命中檢核原持卡人 = 命中 / 未命中
                        - 附卡人_命中檢核原持卡人 = 命中 / 未命中


                        附卡人不檢核項目
                        - 是否檢核分行資訊
                        - 是否檢核頻繁ID
                        - 行內Email
                        - 行內Mobile
                        因此上述項目附卡人狀態應皆為
                        - 附卡人_是否檢核原持卡人成功 = 不須檢核
                        - 附卡人_命中檢核原持卡人 = 不須檢核

                        更新資料判斷都可以拿正卡人或附卡人的是否檢核XXX成功來判斷
                        只需判斷成功，才繼續即可
                    */
                    var paperCheckJobContext = MapHelper.MapToPaperCheckJobContext(pendingCase, caseDetail);
                    LogStartCheckContext(paperCheckJobContext);
                    await _service.檢核單筆案件(paperCheckJobContext);
                    LogEndCheckContext(paperCheckJobContext);
                    if (paperCheckJobContext.HasAnyCheckFailed())
                        await _service.新增系統錯誤紀錄(paperCheckJobContext.GetFailedSystemErrorLogs());

                    var (isSuccess, errorCount) = await _service.更新案件資料(paperCheckJobContext, addressInfos);
                    if (errorCount == 2)
                        新增錯誤達兩次案件編號(pendingCase.ApplyNo);

                    if (isSuccess)
                    {
                        _logger.LogInformation("案件檢核成功 - 申請書編號: {ApplyNo}", pendingCase.ApplyNo);
                    }
                    else
                    {
                        _logger.LogInformation("案件檢核失敗 - 申請書編號: {ApplyNo}", pendingCase.ApplyNo);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(
                        "紙本件檢核新案件排程執行發生例外 - 申請書編號: {ApplyNo} 錯誤訊息: {Error}",
                        pendingCase.ApplyNo,
                        ex.ToString()
                    );

                    var caseErrorCount = await _service.案件處理異常(
                        pendingCase.ApplyNo,
                        SystemErrorLogTypeConst.紙本件檢核異常,
                        "紙本件檢核新案件達到錯誤次數上限",
                        ex
                    );

                    if (caseErrorCount == 2)
                        新增錯誤達兩次案件編號(pendingCase.ApplyNo);
                }
            }

            // 6. 寄信給達錯誤2次案件
            if (_errorApplyNoList.Any())
            {
                await _service.寄信給達錯誤2次案件(_errorApplyNoList);
            }

            _logger.LogInformation("紙本件檢核新案件排程執行完成，總共處理 {Count} 筆案件", pendingCases.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "紙本件檢核新案件排程執行發生例外");
            throw;
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private void 新增錯誤達兩次案件編號(string applyNo)
    {
        _logger.LogInformation("新增錯誤達兩次案件編號: {ApplyNo}", applyNo);
        _errorApplyNoList.Add(applyNo);
    }

    private void LogStartCheckContext(PaperCheckJobContext context)
    {
        _logger.LogInformation("申請號碼: {ApplyNo}", context.ApplyNo);
        _logger.LogInformation("案件是否檢核原持卡人: {IsCheckOriginalCardholder}", context.案件是否檢核原持卡人);
        _logger.LogInformation("案件是否檢核姓名檢核: {IsCheckNameCheck}", context.案件是否檢核姓名檢核);
        _logger.LogInformation("案件是否檢核行內Email: {IsCheckInternalEmail}", context.案件是否檢核行內Email);
        _logger.LogInformation("案件是否檢核行內Mobile: {IsCheckInternalMobile}", context.案件是否檢核行內Mobile);
        _logger.LogInformation("案件是否檢核929: {IsCheck929}", context.案件是否檢核929);
        _logger.LogInformation("案件是否檢核分行資訊: {IsCheckBranchInfo}", context.案件是否檢核分行資訊);
        _logger.LogInformation("案件是否檢核關注名單: {IsCheckFocusList}", context.案件是否檢核關注名單);
        _logger.LogInformation("案件是否檢核頻繁ID: {IsCheckShortTimeID}", context.案件是否檢核頻繁ID);
        _logger.LogInformation("案件是否重覆進件檢核: {IsRepeatCheck}", context.案件是否檢查重覆進件);

        foreach (var userCheckResult in context.UserCheckResults)
        {
            _logger.LogInformation("持卡人ID: {ID}", userCheckResult.ID);
            _logger.LogInformation("持卡人姓名: {Name}", userCheckResult.Name);
            _logger.LogInformation("持卡人Email: {Email}", userCheckResult.Email);
            _logger.LogInformation("持卡人Mobile: {Mobile}", userCheckResult.Mobile);
            _logger.LogInformation("持卡人類型: {UserType}", userCheckResult.UserType);
            _logger.LogInformation("是否檢核原持卡人: {IsCheckOriginalCardholder}", userCheckResult.是否檢核原持卡人);
            _logger.LogInformation("是否檢核姓名檢核: {IsCheckNameCheck}", userCheckResult.是否檢核姓名檢核);
            _logger.LogInformation("是否檢核行內Email: {IsCheckInternalEmail}", userCheckResult.是否檢核行內Email);
            _logger.LogInformation("是否檢核行內Mobile: {IsCheckInternalMobile}", userCheckResult.是否檢核行內Mobile);
            _logger.LogInformation("是否檢核929: {IsCheck929}", userCheckResult.是否檢核929);
            _logger.LogInformation("是否檢核分行資訊: {IsCheckBranchInfo}", userCheckResult.是否檢核分行資訊);
            _logger.LogInformation("是否檢核關注名單: {IsCheckFocusList}", userCheckResult.是否檢核關注名單);
            _logger.LogInformation("是否檢核頻繁ID: {IsCheckShortTimeID}", userCheckResult.是否檢核頻繁ID);
            _logger.LogInformation("是否檢核重覆進件檢核: {IsRepeatCheck}", userCheckResult.是否檢查重覆進件);
        }
    }

    private void LogEndCheckContext(PaperCheckJobContext context)
    {
        _logger.LogInformation("申請號碼: {ApplyNo}", context.ApplyNo);
        foreach (var userCheckResult in context.UserCheckResults)
        {
            _logger.LogInformation("持卡人ID: {ID}", userCheckResult.ID);
            _logger.LogInformation("持卡人姓名: {Name}", userCheckResult.Name);
            _logger.LogInformation("持卡人Email: {Email}", userCheckResult.Email);
            _logger.LogInformation("持卡人Mobile: {Mobile}", userCheckResult.Mobile);
            _logger.LogInformation("持卡人類型: {UserType}", userCheckResult.UserType);
            _logger.LogInformation("是否檢核原持卡人成功: {IsCheckOriginalCardholderSuccess}", userCheckResult.是否檢核原持卡人成功);
            _logger.LogInformation("是否檢核姓名檢核成功: {IsCheckNameCheckSuccess}", userCheckResult.是否檢核姓名檢核成功);
            _logger.LogInformation("是否檢核行內Email成功: {IsCheckInternalEmailSuccess}", userCheckResult.是否檢核行內Email成功);
            _logger.LogInformation("是否檢核行內Mobile成功: {IsCheckInternalMobileSuccess}", userCheckResult.是否檢核行內Mobile成功);
            _logger.LogInformation("是否檢核929成功: {IsCheck929Success}", userCheckResult.是否檢核929成功);
            _logger.LogInformation("是否檢核分行資訊成功: {IsCheckBranchInfoSuccess}", userCheckResult.是否檢核分行資訊成功);
            _logger.LogInformation("是否檢核關注名單成功: {IsCheckFocusListSuccess}", userCheckResult.是否檢核關注名單成功);
            _logger.LogInformation("是否檢核頻繁ID成功: {IsCheckShortTimeIDSuccess}", userCheckResult.是否檢核頻繁ID成功);
            _logger.LogInformation("是否檢核頻繁ID: {IsCheckShortTimeID}", userCheckResult.是否檢核頻繁ID);
            _logger.LogInformation("是否檢核重覆進件成功: {IsRepeatCheckSuccess}", userCheckResult.是否檢查重覆進件成功);
            _logger.LogInformation("命中檢核原持卡人: {HitCheckOriginalCardholder}", userCheckResult.命中檢核原持卡人);
            _logger.LogInformation("命中檢核姓名檢核: {HitCheckNameCheck}", userCheckResult.命中檢核姓名檢核);
            _logger.LogInformation("命中檢核行內Email: {HitCheckInternalEmail}", userCheckResult.命中檢核行內Email);
            _logger.LogInformation("命中檢核行內Mobile: {HitCheckInternalMobile}", userCheckResult.命中檢核行內Mobile);
            _logger.LogInformation("命中檢核929: {HitCheck929}", userCheckResult.命中檢核929);
            _logger.LogInformation("命中檢核分行資訊: {HitCheckBranchInfo}", userCheckResult.命中檢核分行資訊);
            _logger.LogInformation("命中檢核關注名單1: {HitCheckFocusList1}", userCheckResult.命中檢核關注名單1);
            _logger.LogInformation("命中檢核關注名單2: {HitCheckFocusList2}", userCheckResult.命中檢核關注名單2);
            _logger.LogInformation("命中檢核頻繁ID: {HitCheckShortTimeID}", userCheckResult.命中檢核頻繁ID);
            _logger.LogInformation("命中檢核重覆進件: {HitRepeatCheck}", userCheckResult.命中檢查重覆進件);
        }
    }
}
