using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase;

public class PaperCheckNewCaseService : IPaperCheckNewCaseService
{
    private readonly ILogger<PaperCheckNewCaseService> _logger;
    private readonly IPaperCheckNewCaseRepository _repository;
    private readonly IMW3APAPIAdapter _mw3APIAdapter;
    private readonly IMW3ProcAdapter _mw3ProcAdapter;
    private readonly IMW3MSAPIAdapter _mw3MSAdapter;
    private readonly ScoreSharpContext _context;

    public PaperCheckNewCaseService(
        ILogger<PaperCheckNewCaseService> logger,
        IPaperCheckNewCaseRepository repository,
        IMW3APAPIAdapter mw3Adapter,
        IMW3ProcAdapter mw3ProcAdapter,
        IMW3MSAPIAdapter mw3MSAdapter,
        ScoreSharpContext context
    )
    {
        _logger = logger;
        _repository = repository;
        _mw3APIAdapter = mw3Adapter;
        _mw3ProcAdapter = mw3ProcAdapter;
        _mw3MSAdapter = mw3MSAdapter;
        _context = context;
    }

    private void LogCheckResult(LogLevel logLevel, string message, string applyNo, string id, UserType userType, bool isSuccess = false)
    {
        _logger.Log(
            logLevel,
            "申請號碼: {ApplyNo}, ID: {ID}, 正附卡人類型: {UserType}, 檢核是否成功: {Result} 訊息: {Message}",
            applyNo,
            id,
            userType,
            isSuccess ? "成功" : "失敗",
            message
        );
    }

    public async Task 檢核單筆案件(PaperCheckJobContext context)
    {
        var MainID = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.正卡人)?.ID ?? string.Empty;
        var supplementaryID = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.附卡人)?.ID ?? string.Empty;
        _logger.LogInformation(
            "開始檢核案件 - 申請號碼: {ApplyNo}, 正卡人ID: {MainID}, 附卡人ID: {SupplementaryID}",
            context.ApplyNo,
            MainID,
            supplementaryID
        );

        // 步驟 1: 原持卡人查詢（必須先執行），因需判斷是否檢核姓名檢核，如有錯誤則終止之後的檢核流程
        if (context.案件是否檢核原持卡人)
        {
            await 檢核_原持卡人資料(context);
            if (context.UserCheckResults.Any(x => x.是否檢核原持卡人成功 == 檢核結果.失敗))
            {
                LogCheckResult(
                    LogLevel.Error,
                    "原持卡人查詢失敗，終止檢核流程，請確認 MW3 是否正常",
                    context.ApplyNo,
                    MainID,
                    UserType.正卡人,
                    false
                );
                return;
            }
        }

        /*
         * 步驟 2:
         * 姓名檢核 (正附卡)
         * 行內Email (正卡)
         * 行內Mobile (正卡)
         * 929查詢 (正附卡)
         * 分行資訊查詢 (正卡)
         * 關注名單查詢 (正附卡)
         * 頻繁ID檢核 (正卡)
         * 重覆進件檢核 (正附卡)
         */
        var taskDict = new Dictionary<string, Task>();
        if (context.案件是否檢核姓名檢核)
        {
            taskDict["NameCheck"] = 檢核_姓名檢核(context);
        }

        if (context.案件是否檢核行內Email)
        {
            taskDict["InternalEmail"] = 檢核_行內Email(context);
        }

        if (context.案件是否檢核行內Mobile)
        {
            taskDict["InternalMobile"] = 檢核_行內Mobile(context);
        }

        if (context.案件是否檢核929)
        {
            taskDict["Query929"] = 檢核_發查929(context);
        }

        if (context.案件是否檢核分行資訊)
        {
            taskDict["BranchInfo"] = 檢核_分行資訊查詢(context);
        }

        if (context.案件是否檢核關注名單)
        {
            taskDict["FocusList"] = 檢核_關注名單查詢(context);
        }

        if (context.案件是否檢核頻繁ID)
        {
            taskDict["ShortTimeID"] = 檢核_頻繁ID申請(context);
        }

        if (context.案件是否檢查重覆進件)
        {
            taskDict["RepeatApply"] = 檢核_重覆進件檢核(context);
        }

        await Task.WhenAll(taskDict.Values);

        _logger.LogInformation("案件檢核完成 - 申請號碼: {ApplyNo}", context.ApplyNo);
    }

    private async Task 檢核_原持卡人資料(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            var result = new CheckCaseRes<QueryOriginalCardholderData>();
            result.SetStartTime();

            try
            {
                var response = await _mw3ProcAdapter.QueryOriginalCardholderData(info.ID);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢原持卡人資料_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );
                    LogCheckResult(LogLevel.Error, "查詢原持卡人資料_回傳結果IsSuccess為N", context.ApplyNo, info.ID, info.UserType, false);
                    info.設定原持卡人查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    continue;
                }
                var queryOriginalCardholderData = response.Data!;

                if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.成功)
                {
                    var originalCardholderData = queryOriginalCardholderData.Info.Table.FirstOrDefault();
                    QueryOriginalCardholderData data = MapHelper.MapToQueryOriginalCardholderData(originalCardholderData);
                    result.SetSuccess(data);
                    info.設定原持卡人查詢結果(檢核結果.成功, 命中檢核結果.命中);
                }
                else if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.查詢原持卡人資料_查無資料)
                {
                    info.設定原持卡人查詢結果(檢核結果.成功, 命中檢核結果.未命中);
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢原持卡人資料_回傳結果RtnCode為其他",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定原持卡人查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢原持卡人資料_回傳結果RtnCode為其他 RtnCode: {queryOriginalCardholderData.RtnCode}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢原持卡人資料_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                        Response = JsonHelper.序列化物件(result),
                    }
                );

                LogCheckResult(LogLevel.Error, $"查詢原持卡人資料_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
                info.設定原持卡人查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
            }
            finally
            {
                result.SetEndTime();
                info.原持卡人查詢結果 = result;
            }
        }
    }

    private async Task 檢核_行內Email(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            var result = new CheckCaseRes<List<Reviewer_BankInternalSameLog>>();
            result.SetStartTime();

            try
            {
                if (!info.是否檢核行內Email)
                {
                    string message = info.UserType == UserType.附卡人 ? "不檢核行內Email，因為附卡人" : "不檢核行內Email，因為正卡人，不是電子帳單";
                    LogCheckResult(LogLevel.Information, message, context.ApplyNo, info.ID, info.UserType);
                    info.設定行內Email檢核結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(info.Email))
                {
                    LogCheckResult(LogLevel.Information, "不檢核行內Email，Email為空", context.ApplyNo, info.ID, info.UserType);
                    info.設定行內Email檢核結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    continue;
                }

                var response = await _mw3ProcAdapter.QueryEBill(email: info.Email);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢電子帳單For行內Email_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(new { Email = info.Email }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定行內Email檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(LogLevel.Error, "查詢電子帳單For行內Email_回傳結果IsSuccess為N", context.ApplyNo, info.ID, info.UserType, false);
                    continue;
                }

                var queryEBillData = response.Data!;

                if (queryEBillData.RtnCode == MW3RtnCodeConst.查詢電子帳單_該信箱已存在)
                {
                    var ebillDataList = new List<Reviewer_BankInternalSameLog>();
                    var mainList = queryEBillData.Info.Table.Where(x => x.ID != info.ID);

                    foreach (var data in mainList)
                    {
                        var cardholderResponse = await _mw3ProcAdapter.QueryOriginalCardholderData(id: data.ID);
                        var billAddr = cardholderResponse.Data?.Info?.Table?.FirstOrDefault()?.BillAddr;

                        var ebillData = new Reviewer_BankInternalSameLog()
                        {
                            ApplyNo = context.ApplyNo,
                            ID = info.ID,
                            UserType = UserType.正卡人,
                            SameID = data.ID,
                            SameName = data.Name,
                            CheckType = BankInternalSameCheckType.Email,
                            SameBillAddr = billAddr,
                        };

                        ebillDataList.Add(ebillData);
                    }

                    info.設定行內Email檢核結果(檢核結果.成功, 命中檢核結果.命中);
                    result.SetSuccess(ebillDataList);
                }
                else if (queryEBillData.RtnCode == MW3RtnCodeConst.查詢電子帳單_無相同信箱會員)
                {
                    info.設定行內Email檢核結果(檢核結果.成功, 命中檢核結果.未命中);
                    result.SetSuccess([]);
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢電子帳單For行內Email_回傳結果RtnCode為其他",
                            Request = JsonHelper.序列化物件(new { Email = info.Email }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );
                    info.設定行內Email檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢電子帳單For行內Email_回傳結果RtnCode為其他 RtnCode: {queryEBillData.RtnCode}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢電子帳單For行內Email_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Email = info.Email }),
                        Response = JsonHelper.序列化物件(result),
                    }
                );
                info.設定行內Email檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                LogCheckResult(LogLevel.Error, $"查詢電子帳單For行內Email_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
            }
            finally
            {
                result.SetEndTime();
                info.行內Email檢核結果 = result;
            }
        }
    }

    private async Task 檢核_行內Mobile(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            var result = new CheckCaseRes<List<Reviewer_BankInternalSameLog>>();
            result.SetStartTime();

            try
            {
                if (!info.是否檢核行內Mobile)
                {
                    LogCheckResult(LogLevel.Information, "不檢核行內Mobile，因為附卡人", context.ApplyNo, info.ID, info.UserType);
                    info.設定行內Mobile檢核結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    continue;
                }

                if (string.IsNullOrWhiteSpace(info.Mobile))
                {
                    LogCheckResult(LogLevel.Information, "不檢核行內Mobile，Mobile為空", context.ApplyNo, info.ID, info.UserType);
                    info.設定行內Mobile檢核結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    continue;
                }
                var response = await _mw3ProcAdapter.QueryOriginalCardholderData(id: "", email: "", mobile: info.Mobile);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢原持卡人資料For行內Mobile_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(new { Mobile = info.Mobile }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定行內Mobile檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        "查詢原持卡人資料For行內Mobile_回傳結果IsSuccess為N",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                    continue;
                }

                var queryOriginalCardholderData = response.Data!;

                if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.成功)
                {
                    var originalCardholderData = queryOriginalCardholderData
                        .Info.Table.Select(x => MapHelper.MapToQueryOriginalCardholderData(x))
                        .Where(x => x.ID != info.ID)
                        .Select(x => new Reviewer_BankInternalSameLog()
                        {
                            ApplyNo = context.ApplyNo,
                            ID = info.ID,
                            UserType = info.UserType,
                            SameID = x.ID,
                            SameName = x.ChineseName,
                            CheckType = BankInternalSameCheckType.手機,
                            SameBillAddr = x.BillAddr,
                        })
                        .ToList();

                    info.設定行內Mobile檢核結果(檢核結果.成功, 命中檢核結果.命中);
                    result.SetSuccess(originalCardholderData);
                }
                else if (queryOriginalCardholderData.RtnCode == MW3RtnCodeConst.查詢原持卡人資料_查無資料)
                {
                    info.設定行內Mobile檢核結果(檢核結果.成功, 命中檢核結果.未命中);
                    result.SetSuccess([]);
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢原持卡人資料For行內Mobile_回傳結果RtnCode為其他",
                            Request = JsonHelper.序列化物件(new { Mobile = info.Mobile }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );
                    info.設定行內Mobile檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢原持卡人資料For行內Mobile_回傳結果RtnCode為其他 RtnCode: {queryOriginalCardholderData.RtnCode}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢原持卡人資料For行內Mobile_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Mobile = info.Mobile }),
                        Response = JsonHelper.序列化物件(result),
                    }
                );
                info.設定行內Mobile檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                LogCheckResult(
                    LogLevel.Error,
                    $"查詢原持卡人資料For行內Mobile_發生例外: {ex.ToString()}",
                    context.ApplyNo,
                    info.ID,
                    info.UserType,
                    false
                );
            }
            finally
            {
                result.SetEndTime();
                info.行內Mobile檢核結果 = result;
            }
        }
    }

    private async Task 檢核_姓名檢核(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            CheckCaseRes<QueryCheckName> result = new();
            string ulid = Ulid.NewUlid().ToString();
            result.SetStartTime();

            try
            {
                if (info.命中檢核原持卡人 == 命中檢核結果.命中)
                {
                    info.設定姓名檢核結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    LogCheckResult(LogLevel.Information, "原持卡人檢核命中，不檢核姓名檢核", context.ApplyNo, info.ID, info.UserType);
                    continue;
                }

                /* 🔔 會多判斷這個是因為有可能當第一次已經執行過原持卡人檢核，已有結果是 Y/N，但第一次因姓名檢核失敗，
                因此第二次執行時，雖然原持卡人檢核結果是 Y，但因為已經命中，所以不會再執行姓名檢核 */
                if (info.IsOriginalCardholder == "Y")
                {
                    info.設定姓名檢核結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    LogCheckResult(LogLevel.Information, "原持卡人已檢核過為命中，不檢核姓名檢核", context.ApplyNo, info.ID, info.UserType);
                    continue;
                }

                var response = await _mw3APIAdapter.QueryNameCheck(info.Name, UserIdConst.ScoreSharpBatch, ulid);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢姓名檢核_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(
                                new
                                {
                                    Name = info.Name,
                                    CallUser = UserIdConst.ScoreSharpBatch,
                                    TraceId = ulid,
                                }
                            ),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定姓名檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(LogLevel.Error, "查詢姓名檢核_回傳結果IsSuccess為N", context.ApplyNo, info.ID, info.UserType, false);
                    continue;
                }

                var nameCheckData = response.Data!.Info!.Result!.Data;
                var matchResult = nameCheckData.MatchedResult;
                var msg = response.Data!.Response.Trim();

                if (matchResult == MW3RtnCodeConst.查詢姓名檢核_命中 || matchResult == MW3RtnCodeConst.查詢姓名檢核_未命中)
                {
                    QueryCheckName queryCheckName = new()
                    {
                        RtnCode = matchResult,
                        RtnMsg = msg,
                        QueryTime = result.StartTime,
                        Reviewer3rd_NameCheckLog = new()
                        {
                            ApplyNo = context.ApplyNo,
                            UserType = info.UserType,
                            ID = info.ID,
                            StartTime = result.StartTime,
                            EndTime = DateTime.Now,
                            ResponseResult = nameCheckData.MatchedResult,
                            RcPoint = int.Parse(nameCheckData.RCScore),
                            AMLId = nameCheckData.AMLReference,
                            TraceId = nameCheckData.ReferenceNumber,
                            Name = info.Name,
                        },
                        TraceId = nameCheckData.ReferenceNumber,
                    };

                    result.SetSuccess(queryCheckName);

                    if (matchResult == MW3RtnCodeConst.查詢姓名檢核_命中)
                    {
                        info.設定姓名檢核結果(檢核結果.成功, 命中檢核結果.命中);
                    }
                    else if (matchResult == MW3RtnCodeConst.查詢姓名檢核_未命中)
                    {
                        info.設定姓名檢核結果(檢核結果.成功, 命中檢核結果.未命中);
                    }
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢姓名檢核_回傳結果MatchResult為其他",
                            Request = JsonHelper.序列化物件(
                                new
                                {
                                    Name = info.Name,
                                    CallUser = UserIdConst.ScoreSharpBatch,
                                    TraceId = ulid,
                                }
                            ),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定姓名檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢姓名檢核_回傳結果MatchResult為其他 MatchedResult: {nameCheckData.MatchedResult}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢姓名檢核_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                        Response = JsonHelper.序列化物件(result),
                    }
                );

                info.設定姓名檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
                LogCheckResult(LogLevel.Error, $"查詢姓名檢核_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
            }
            finally
            {
                result.SetEndTime();
                info.姓名檢核結果 = result;
            }
        }
    }

    private async Task 檢核_發查929(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            CheckCaseRes<Query929Info> result = new();
            result.SetStartTime();

            try
            {
                var response = await _mw3ProcAdapter.QueryOCSI929(info.ID);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢929查詢_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定929查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(LogLevel.Error, "查詢929查詢_回傳結果IsSuccess為N", context.ApplyNo, info.ID, info.UserType, false);
                    continue;
                }

                var oCSI929ApiResponse = response.Data!;
                var code = oCSI929ApiResponse.RtnCode;
                var msg = oCSI929ApiResponse.Response.Trim();

                if (code == MW3RtnCodeConst.成功)
                {
                    var query929Info = new Query929Info();
                    query929Info.RtnCode = code;
                    query929Info.RtnMsg = msg;
                    query929Info.QueryTime = result.StartTime;
                    query929Info.Reviewer3rd_929Logs.AddRange(
                        oCSI929ApiResponse.Info.Table.Select(item => new Reviewer3rd_929Log
                        {
                            ApplyNo = context.ApplyNo,
                            TxnDate = item.TxnDate != null ? item.TxnDate.Trim() : string.Empty,
                            BrachCode = item.BrachCode != null ? item.BrachCode.Trim() : string.Empty,
                            BrachEmp = item.BrachEmp != null ? item.BrachEmp.Trim() : string.Empty,
                            BusinessCode = item.BusinessCode != null ? item.BusinessCode.Trim() : string.Empty,
                            ChName = item.ChName != null ? item.ChName.Trim() : string.Empty,
                            LoginDate = item.LoginDate != null ? item.LoginDate.Trim() : string.Empty,
                            ApplyCause = item.ApplyCause != null ? item.ApplyCause.Trim() : string.Empty,
                            ApplyReMark = item.ApplyReMark != null ? item.ApplyReMark.Trim() : string.Empty,
                            ID = info.ID,
                            UserType = info.UserType,
                        })
                    );

                    result.SetSuccess(query929Info);
                    info.設定929查詢結果(檢核結果.成功, 命中檢核結果.命中);
                }
                else if (code == MW3RtnCodeConst.查詢929_查無資料)
                {
                    var query929Info = new Query929Info();
                    query929Info.RtnCode = code;
                    query929Info.RtnMsg = msg;
                    query929Info.QueryTime = result.StartTime;

                    result.SetSuccess(query929Info);
                    info.設定929查詢結果(檢核結果.成功, 命中檢核結果.未命中);
                }
                else if (
                    code == MW3RtnCodeConst.查詢929_交易有誤
                    || code == MW3RtnCodeConst.查詢929_聯絡系統管理員
                    || code == MW3RtnCodeConst.查詢929_傳入規格不符合
                    || code == MW3RtnCodeConst.查詢929_此服務已失效
                )
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢929查詢_回傳結果RtnCode為9999、Er01、Er02、Er03",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定929查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢929查詢_回傳結果RtnCode為9999、Er01、Er02、Er03 RtnCode: {code}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢929查詢_回傳結果RtnCode為其他",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定929查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢929查詢_回傳結果RtnCode為其他 RtnCode: {code}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢929查詢_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                        Response = JsonHelper.序列化物件(result),
                    }
                );

                info.設定929查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                LogCheckResult(LogLevel.Error, $"查詢929查詢_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
            }
            finally
            {
                result.SetEndTime();
                info.查詢929結果 = result;
            }
        }
    }

    private async Task 檢核_分行資訊查詢(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            CheckCaseRes<QueryBranchInfo> result = new();
            result.SetStartTime();

            try
            {
                if (!info.是否檢核分行資訊)
                {
                    LogCheckResult(LogLevel.Information, "檢核分行資訊查詢_不檢核，檢核完畢", context.ApplyNo, info.ID, info.UserType);
                    info.設定分行資訊查詢結果(檢核結果.不須檢核, 命中檢核結果.不須檢核);
                    continue;
                }

                var response = await _mw3ProcAdapter.QuerySearchCusData(info.ID);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢分行資訊_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定分行資訊查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    _logger.LogError("查詢分行資訊_回傳結果IsSuccess為N");
                    continue;
                }

                var searchCusDataApiResponse = response.Data!;

                if (searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.成功)
                {
                    QueryBranchInfo queryBranchInfoResponse = new()
                    {
                        RtnCode = searchCusDataApiResponse.RtnCode,
                        RtnMsg = searchCusDataApiResponse.Response != null ? searchCusDataApiResponse.Response.Trim() : string.Empty,
                        QueryTime = result.StartTime,
                    };

                    queryBranchInfoResponse.BranchCusCusInfo.AddRange(
                        searchCusDataApiResponse.Info.客戶資訊.Select(item => new Reviewer3rd_BranchCusCusInfo
                        {
                            ApplyNo = context.ApplyNo,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            SN = item.SN != null ? item.SN.Trim() : string.Empty,
                            Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                            UserType = info.UserType,
                        })
                    );

                    queryBranchInfoResponse.BranchCusWMCust.AddRange(
                        searchCusDataApiResponse.Info.財富管理客戶.Select(item => new Reviewer3rd_BranchCusWMCust
                        {
                            ApplyNo = context.ApplyNo,
                            ID = info.ID != null ? info.ID.Trim() : string.Empty,
                            ICountFlag = item.ICountFlag != null ? item.ICountFlag.Trim() : string.Empty,
                            UserType = info.UserType,
                        })
                    );

                    queryBranchInfoResponse.BranchCusCD.AddRange(
                        searchCusDataApiResponse.Info.定存明細資訊.Select(item => new Reviewer3rd_BranchCusCD
                        {
                            ApplyNo = context.ApplyNo,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            Currency = item.Currency != null ? item.Currency.Trim() : string.Empty,
                            InterestD = item.InterestD != null ? item.InterestD.Trim() : string.Empty,
                            ExpirationD = item.ExpirationD != null ? item.ExpirationD.Trim() : string.Empty,
                            Amount = item.Amount,
                            Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                            UserType = info.UserType,
                        })
                    );

                    queryBranchInfoResponse.BranchCusDD.AddRange(
                        searchCusDataApiResponse.Info.活期存款明細資訊.Select(item => new Reviewer3rd_BranchCusDD
                        {
                            ApplyNo = context.ApplyNo,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                            Currency = item.Currency != null ? item.Currency.Trim() : string.Empty,
                            Account = item.Account != null ? item.Account.Trim() : string.Empty,
                            OpenAccountD = item.OpenAcountD != null ? item.OpenAcountD.Trim() : string.Empty,
                            CreditD = item.CreditD != null ? item.CreditD.Trim() : string.Empty,
                            Last3MavgCredit = item.Last3MavgCredit,
                            ThreeMavgCredit = item.ThreeMavgCredit,
                            TwoMavgCredit = item.TwoMavgCredit,
                            OneMavgCredit = item.OneMavgCredit,
                            Credit = item.Credit,
                            UserType = info.UserType,
                        })
                    );

                    queryBranchInfoResponse.BranchCusCAD.AddRange(
                        searchCusDataApiResponse.Info.支票存款明細資訊.Select(item => new Reviewer3rd_BranchCusCAD
                        {
                            ApplyNo = context.ApplyNo,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            Cate = item.Cate != null ? item.Cate.Trim() : string.Empty,
                            Account = item.Account != null ? item.Account.Trim() : string.Empty,
                            OpenAccountD = item.OpenAcountD != null ? item.OpenAcountD.Trim() : string.Empty,
                            CreditD = item.CreditD != null ? item.CreditD.Trim() : string.Empty,
                            Last3MavgCredit = item.Last3MavgCredit,
                            ThreeMavgCredit = item.ThreeMavgCredit,
                            TwoMavgCredit = item.TwoMavgCredit,
                            OneMavgCredit = item.OneMavgCredit,
                            Credit = item.Credit,
                            UserType = info.UserType,
                        })
                    );

                    queryBranchInfoResponse.BranchCusCreditOver.AddRange(
                        searchCusDataApiResponse.Info.授信逾期狀況.Select(item => new Reviewer3rd_BranchCusCreditOver
                        {
                            ApplyNo = context.ApplyNo,
                            ID = item.ID != null ? item.ID.Trim() : string.Empty,
                            Account = item.Account != null ? item.Account.Trim() : string.Empty,
                            OverStatus = item.OverStatus != null ? item.OverStatus.Trim() : string.Empty,
                            UserType = info.UserType,
                        })
                    );

                    result.SetSuccess(queryBranchInfoResponse);

                    info.設定分行資訊查詢結果(檢核結果.成功, 命中檢核結果.命中);
                }
                else if (searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_查無資料)
                {
                    QueryBranchInfo queryBranchInfoResponse = new()
                    {
                        RtnCode = searchCusDataApiResponse.RtnCode,
                        RtnMsg = searchCusDataApiResponse.Response != null ? searchCusDataApiResponse.Response.Trim() : string.Empty,
                        QueryTime = result.StartTime,
                    };

                    result.SetSuccess(queryBranchInfoResponse);

                    info.設定分行資訊查詢結果(檢核結果.成功, 命中檢核結果.未命中);
                }
                else if (
                    searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_聯絡系統管理員
                    || searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_傳入規格不符合
                    || searchCusDataApiResponse.RtnCode == MW3RtnCodeConst.查詢分行資訊_此服務已失效
                )
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢分行資訊_回傳結果RtnCode為Er01、Er02、Er03",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定分行資訊查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢分行資訊_回傳結果RtnCode為Er01、Er02、Er03 RtnCode: {searchCusDataApiResponse.RtnCode}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢分行資訊_回傳結果RtnCode為其他",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定分行資訊查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢分行資訊_回傳結果RtnCode為其他 RtnCode: {searchCusDataApiResponse.RtnCode}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢分行資訊_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                    }
                );

                info.設定分行資訊查詢結果(檢核結果.失敗, 命中檢核結果.未命中);
                LogCheckResult(LogLevel.Error, $"查詢分行資訊_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
            }
            finally
            {
                result.SetEndTime();
                info.分行資訊查詢結果 = result;
            }
        }
    }

    private async Task 檢核_關注名單查詢(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            CheckCaseRes<ConcernDetailInfo> result = new();
            result.SetStartTime();

            try
            {
                var response = await _mw3MSAdapter.QueryConcernDetail(info.ID);

                if (!response.IsSuccess)
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = "查詢關注名單_回傳結果IsSuccess為N",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定關注名單查詢結果(
                        檢核結果: 檢核結果.失敗,
                        命中關注名單1結果: 命中檢核結果.未命中,
                        命中關注名單2結果: 命中檢核結果.未命中
                    );
                    LogCheckResult(LogLevel.Error, "查詢關注名單_回傳結果IsSuccess為N", context.ApplyNo, info.ID, info.UserType, false);
                    continue;
                }

                var queryConcernDetailResponse = response.Data!;
                var code = queryConcernDetailResponse.RtnCode;
                var msg = queryConcernDetailResponse.Msg.Trim();
                var traceId = queryConcernDetailResponse.TraceId;

                if (code == MW3RtnCodeConst.成功)
                {
                    ConcernDetailInfo concernDetailInfo = new()
                    {
                        RtnCode = code,
                        RtnMsg = msg,
                        QueryTime = result.StartTime,
                        TraceId = traceId,
                    };

                    var concernDetailInfoData = queryConcernDetailResponse.Info;

                    if (concernDetailInfoData.Restriction.Count > 0)
                    {
                        // 告誡名單 (A)
                        concernDetailInfo.WarnLogs.AddRange(
                            concernDetailInfoData.Restriction.Select(item => new Reviewer3rd_WarnLog
                            {
                                ApplyNo = context.ApplyNo,
                                DataType = item.DataType != null ? item.DataType.Trim() : string.Empty,
                                ID = item.ID != null ? item.ID.Trim() : string.Empty,
                                WarningDate = item.WarningDate != null ? item.WarningDate.Trim() : string.Empty,
                                ExpireDate = item.ExpireDate != null ? item.ExpireDate.Trim() : string.Empty,
                                Issuer = item.Issuer != null ? item.Issuer.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = info.UserType,
                            })
                        );

                        concernDetailInfo.Focus2HitList.Add("A");
                    }

                    if (concernDetailInfoData.WarningCompany.Count > 0)
                    {
                        // 受警示企業戶之負責人 (B)
                        concernDetailInfo.WarningCompanyLogs.AddRange(
                            concernDetailInfoData.WarningCompany.Select(item => new Reviewer3rd_WarnCompLog
                            {
                                ApplyNo = context.ApplyNo,
                                Account = item.Account != null ? item.Account.Trim() : string.Empty,
                                CorporateID = item.CorporateID != null ? item.CorporateID.Trim() : string.Empty,
                                PID = item.PID != null ? item.PID.Trim() : string.Empty,
                                AccountDate = item.AccountDate != null ? item.AccountDate.Trim() : string.Empty,
                                AccidentCode = item.AccidentCode != null ? item.AccidentCode.Trim() : string.Empty,
                                AccidentDate = item.AccidentDate != null ? item.AccidentDate.Trim() : string.Empty,
                                AccidentCancelDate = item.AccidentCancelDate != null ? item.AccidentCancelDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = info.UserType,
                                ID = info.ID,
                            })
                        );

                        concernDetailInfo.Focus1HitList.Add("B");
                    }

                    if (concernDetailInfoData.RiskAccount.Count > 0)
                    {
                        // 風險帳戶 (C)
                        concernDetailInfo.RiskAccountLogs.AddRange(
                            concernDetailInfoData.RiskAccount.Select(item => new Reviewer3rd_RiskAccountLog
                            {
                                ApplyNo = context.ApplyNo,
                                Account = item.Account != null ? item.Account.Trim() : string.Empty,
                                PID = item.PID != null ? item.PID.Trim() : string.Empty,
                                AccountDate = item.AccountDate != null ? item.AccountDate.Trim() : string.Empty,
                                AccidentDate = item.AccidentDate != null ? item.AccidentDate.Trim() : string.Empty,
                                AccidentCancelDate = item.AccidentCancelDate != null ? item.AccidentCancelDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = info.UserType,
                                Memo = item.Memo != null ? item.Memo.Trim() : string.Empty,
                                ID = info.ID,
                            })
                        );

                        concernDetailInfo.Focus1HitList.Add("C");
                    }

                    if (concernDetailInfoData.FrdId.Count > 0)
                    {
                        // 疑似涉詐境內帳戶 (H)
                        concernDetailInfo.FrdIdLogs.AddRange(
                            concernDetailInfoData.FrdId.Select(item => new Reviewer3rd_FrdIdLog
                            {
                                ApplyNo = context.ApplyNo,
                                UserType = info.UserType,
                                ID = item.ID != null ? item.ID.Trim() : string.Empty,
                                Account = item.Account != null ? item.Account.Trim() : string.Empty,
                                NotifyDate = item.NotifyDate != null ? item.NotifyDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            })
                        );

                        concernDetailInfo.Focus1HitList.Add("H");
                    }

                    if (concernDetailInfoData.Fled.Count > 0)
                    {
                        // 聯徵資料─行方不明 (D)
                        concernDetailInfo.FledLogs.AddRange(
                            concernDetailInfoData.Fled.Select(item => new Reviewer3rd_FledLog
                            {
                                ApplyNo = context.ApplyNo,
                                ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                                ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                                PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                                Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                                BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                                Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                                FledDate = item.FledDate != null ? item.FledDate.Trim() : string.Empty,
                                CatchingDate = item.CatchingDate != null ? item.CatchingDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = info.UserType,
                                ID = info.ID,
                            })
                        );

                        concernDetailInfo.Focus2HitList.Add("D");
                    }

                    if (concernDetailInfoData.Punish.Count > 0)
                    {
                        // 聯徵資料─收容遣返 (E)
                        concernDetailInfo.PunishLogs.AddRange(
                            concernDetailInfoData.Punish.Select(item => new Reviewer3rd_PunishLog
                            {
                                ApplyNo = context.ApplyNo,
                                ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                                ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                                PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                                Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                                BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                                Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                                CatchingDate = item.CatchingDate != null ? item.CatchingDate.Trim() : string.Empty,
                                ImmigrateDate = item.ImmigrateDate != null ? item.ImmigrateDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = info.UserType,
                                ID = info.ID,
                            })
                        );

                        concernDetailInfo.Focus2HitList.Add("E");
                    }

                    if (concernDetailInfoData.Immi.Count > 0)
                    {
                        // 聯徵資料─出境 (F)
                        concernDetailInfo.ImmiLogs.AddRange(
                            concernDetailInfoData.Immi.Select(item => new Reviewer3rd_ImmiLog
                            {
                                ApplyNo = context.ApplyNo,
                                ResidentIdNo = item.ResidentIdNo != null ? item.ResidentIdNo.Trim() : string.Empty,
                                ENName = item.ENName != null ? item.ENName.Trim() : string.Empty,
                                PassportNo = item.PassportNo != null ? item.PassportNo.Trim() : string.Empty,
                                Nationality = item.Nationality != null ? item.Nationality.Trim() : string.Empty,
                                BirthDate = item.BirthDate != null ? item.BirthDate.Trim() : string.Empty,
                                Gender = item.Gender != null ? item.Gender.Trim() : string.Empty,
                                ImmigrateDate = item.ImmigrateDate != null ? item.ImmigrateDate.Trim() : string.Empty,
                                InTW = item.InTW != null ? item.InTW.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = info.UserType,
                                ID = info.ID,
                            })
                        );

                        concernDetailInfo.Focus2HitList.Add("F");
                    }

                    if (concernDetailInfoData.MissingPersons != null)
                    {
                        // 失蹤人口 (G)
                        concernDetailInfo.MissingPersonsLogs = new Reviewer3rd_MissingPersonsLog
                        {
                            ApplyNo = context.ApplyNo,
                            ID = info.ID,
                            UserType = info.UserType,
                            YnmpInfo = concernDetailInfoData.MissingPersons.YnmpInfo.Trim(),
                            CreateDate = concernDetailInfoData.MissingPersons.CreateDate.Trim(),
                        };

                        if (
                            !string.IsNullOrEmpty(concernDetailInfo.MissingPersonsLogs.YnmpInfo)
                            && concernDetailInfo.MissingPersonsLogs.YnmpInfo == "Y"
                        )
                        {
                            concernDetailInfo.Focus2HitList.Add("G");
                        }
                    }

                    if (concernDetailInfoData.LayOff.Count > 0)
                    {
                        // 聯徵資料─解聘 (I)
                        concernDetailInfo.LayOffLogs.AddRange(
                            concernDetailInfoData.LayOff.Select(item => new Reviewer3rd_LayOffLog
                            {
                                ApplyNo = context.ApplyNo,
                                UserType = info.UserType,
                                ID = info.ID,
                                TransDate = item.TransDate != null ? item.TransDate.Trim() : string.Empty,
                                ChngId = item.ChngId != null ? item.ChngId.Trim() : string.Empty,
                                NatCode = item.NatCode != null ? item.NatCode.Trim() : string.Empty,
                                PassNo = item.PassNo != null ? item.PassNo.Trim() : string.Empty,
                                ExpirWkNo = item.ExpirWkNo != null ? item.ExpirWkNo.Trim() : string.Empty,
                                KnowDate = item.KnowDate != null ? item.KnowDate.Trim() : string.Empty,
                                DynaDate = item.DynaDate != null ? item.DynaDate.Trim() : string.Empty,
                                HappCode = item.HappCode != null ? item.HappCode.Trim() : string.Empty,
                                VendCode = item.VendCode != null ? item.VendCode.Trim() : string.Empty,
                                LaborCode = item.LaborCode != null ? item.LaborCode.Trim() : string.Empty,
                                ExpireDate = item.ExpireDate != null ? item.ExpireDate.Trim() : string.Empty,
                                ImmiType = item.ImmiType != null ? item.ImmiType.Trim() : string.Empty,
                                ImmiTypeDesc = item.ImmiTypeDesc != null ? item.ImmiTypeDesc.Trim() : string.Empty,
                                HappCodeDesc = item.HappCodeDesc != null ? item.HappCodeDesc.Trim() : string.Empty,
                                VendCodeDesc = item.VendCodeDesc != null ? item.VendCodeDesc.Trim() : string.Empty,
                                LaborCodeDesc = item.LaborCodeDesc != null ? item.LaborCodeDesc.Trim() : string.Empty,
                                WpCode = item.WpCode != null ? item.WpCode.Trim() : string.Empty,
                                WpCodeDesc = item.WpCodeDesc != null ? item.WpCodeDesc.Trim() : string.Empty,
                                Resnum = item.Resnum != null ? item.Resnum.Trim() : string.Empty,
                                ImmigartionDate = item.ImmigartionDate != null ? item.ImmigartionDate.Trim() : string.Empty,
                                SystemDate = item.SystemDate != null ? item.SystemDate.Trim() : string.Empty,
                                NiaResidenceNo = item.NiaResidenceNo != null ? item.NiaResidenceNo.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                            })
                        );

                        concernDetailInfo.Focus2HitList.Add("I");
                    }

                    result.SetSuccess(concernDetailInfo);
                    bool isFocus1Hit = concernDetailInfo.Focus1HitList.Count > 0;
                    bool isFocus2Hit = concernDetailInfo.Focus2HitList.Count > 0;
                    info.設定關注名單查詢結果(
                        檢核結果: 檢核結果.成功,
                        命中關注名單1結果: isFocus1Hit ? 命中檢核結果.命中 : 命中檢核結果.未命中,
                        命中關注名單2結果: isFocus2Hit ? 命中檢核結果.命中 : 命中檢核結果.未命中
                    );
                }
                else
                {
                    result.SetError(
                        new()
                        {
                            ApplyNo = context.ApplyNo,
                            ErrorMessage = $"查詢關注名單_回傳結果RtnCode為{code},TraceId:{traceId}",
                            Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                            Response = JsonHelper.序列化物件(response),
                            Type = SystemErrorLogTypeConst.第三方API執行錯誤,
                        }
                    );

                    info.設定關注名單查詢結果(
                        檢核結果: 檢核結果.失敗,
                        命中關注名單1結果: 命中檢核結果.未命中,
                        命中關注名單2結果: 命中檢核結果.未命中
                    );

                    LogCheckResult(
                        LogLevel.Error,
                        $"查詢關注名單_回傳結果RtnCode為{code},TraceId:{traceId}",
                        context.ApplyNo,
                        info.ID,
                        info.UserType,
                        false
                    );
                }
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "查詢關注名單_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                    }
                );

                info.設定關注名單查詢結果(檢核結果: 檢核結果.失敗, 命中關注名單1結果: 命中檢核結果.未命中, 命中關注名單2結果: 命中檢核結果.未命中);

                LogCheckResult(LogLevel.Error, $"查詢關注名單_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
            }
            finally
            {
                result.SetEndTime();
                info.關注名單查詢結果 = result;
            }
        }
    }

    private async Task 檢核_頻繁ID申請(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;

        foreach (var info in userCheckResults)
        {
            var result = new CheckCaseRes<List<Reviewer_CheckTrace>>();
            result.SetStartTime();

            try
            {
                if (!info.是否檢核頻繁ID)
                {
                    LogCheckResult(LogLevel.Information, "不檢核頻繁ID申請，因為附卡人", context.ApplyNo, info.ID, info.UserType);
                    continue;
                }

                var response = await _repository.查詢_頻繁ID(context.ApplyNo);

                var reviewer_CheckTraces = response
                    .HitApplyNoInfos.Select(x => new Reviewer_CheckTrace()
                    {
                        SameApplyNo = x.SameApplyNo,
                        CurrentID = x.CurrentID,
                        CurrentUserType = x.CurrentUserType,
                        CheckType = x.CheckType,
                        CurrentApplyNo = x.CurrentApplyNo,
                    })
                    .ToList();

                result.SetSuccess(reviewer_CheckTraces);
                bool isHit = response.IsHit == "Y";
                info.設定頻繁ID檢核結果(檢核結果.成功, isHit ? 命中檢核結果.命中 : 命中檢核結果.未命中);
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "檢核_頻繁ID申請_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                    }
                );
                info.設定頻繁ID檢核結果(檢核結果.失敗, 命中檢核結果.未命中);

                LogCheckResult(LogLevel.Error, $"檢核_頻繁ID申請_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
            }
            finally
            {
                result.SetEndTime();
                info.頻繁ID檢核結果 = result;
            }
        }
    }

    private async Task 檢核_重覆進件檢核(PaperCheckJobContext context)
    {
        var userCheckResults = context.UserCheckResults;
        foreach (var info in userCheckResults)
        {
            var result = new CheckCaseRes<string>();
            result.SetStartTime();

            try
            {
                bool isContainID = await _context.Reviewer_ApplyCreditCardInfoHandle.AnyAsync(x => x.ID == info.ID && x.ApplyNo != context.ApplyNo);
                string resultStr = isContainID ? "Y" : "N";
                result.SetSuccess(resultStr);
                info.設定重覆進件檢核結果(檢核結果.成功, resultStr == "Y" ? 命中檢核結果.命中 : 命中檢核結果.未命中);
            }
            catch (Exception ex)
            {
                result.SetError(
                    new()
                    {
                        ApplyNo = context.ApplyNo,
                        ErrorMessage = "檢核_重覆進件檢核_發生例外",
                        ErrorDetail = ex.ToString(),
                        Type = SystemErrorLogTypeConst.內部程式錯誤,
                        Request = JsonHelper.序列化物件(new { Id = info.ID, UserType = info.UserType }),
                    }
                );

                LogCheckResult(LogLevel.Error, $"檢核_重覆進件檢核_發生例外: {ex.ToString()}", context.ApplyNo, info.ID, info.UserType, false);
                info.設定重覆進件檢核結果(檢核結果.失敗, 命中檢核結果.未命中);
            }
            finally
            {
                result.SetEndTime();
                info.重覆進件檢核結果 = result;
            }
        }
    }

    public async Task 寄信給達錯誤2次案件(List<string> applyNo)
    {
        _logger.LogInformation("寄信給達錯誤2次案件，申請書編號: {ApplyNo}", string.Join(",", applyNo));
        _context.ChangeTracker.Clear();

        var errorLogs = applyNo.Select(x => new System_ErrorLog()
        {
            ApplyNo = x,
            Project = SystemErrorLogProjectConst.BATCH,
            Source = "PaperCheckNewCase",
            Type = SystemErrorLogTypeConst.達錯誤2次案件需寄信,
            ErrorMessage = "達錯誤2次案件需寄信",
            ErrorDetail = "達錯誤2次案件需寄信",
            AddTime = DateTime.Now,
            SendStatus = SendStatus.等待,
        });

        await _context.System_ErrorLog.AddRangeAsync(errorLogs);

        await _context.SaveChangesAsync();
    }

    public async Task<int> 案件處理異常(string applyNo, string type, string errorTitle, Exception ex)
    {
        _logger.LogError(
            "案件處理異常，申請書編號: {ApplyNo}, 錯誤類型: {Type}, 錯誤標題: {ErrorTitle} Error: {Error}",
            applyNo,
            type,
            errorTitle,
            ex.ToString()
        );

        _context.ChangeTracker.Clear();
        DateTime now = DateTime.Now;
        CardStatus afterStatus = CardStatus.紙本件_待月收入預審_檢核異常;

        var handleInfos = await _context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == applyNo).ToListAsync();
        foreach (var handleInfo in handleInfos)
        {
            handleInfo.CardStatus = afterStatus;
        }

        // 單筆案件解鎖並且錯誤次數加1
        var pedding = await _context.ReviewerPedding_PaperApplyCardCheckJob.FirstOrDefaultAsync(x => x.ApplyNo == applyNo);
        pedding.ErrorCount++;

        await _context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(
            MapHelper.MapToProcess(applyNo: applyNo, action: afterStatus.ToString(), startTime: now, endTime: now)
        );

        await _context.System_ErrorLog.AddAsync(
            new System_ErrorLog()
            {
                ApplyNo = applyNo,
                Project = SystemErrorLogProjectConst.BATCH,
                Source = "PaperCheckNewCase",
                Type = type,
                ErrorMessage = errorTitle,
                ErrorDetail = ex.ToString(),
                AddTime = now,
                SendStatus = SendStatus.等待,
            }
        );

        await _context.SaveChangesAsync();

        return pedding.ErrorCount;
    }

    public async Task<(bool isSuccess, int errorCount)> 更新案件資料(PaperCheckJobContext context, List<SetUp_AddressInfo> addressInfos)
    {
        try
        {
            await UpdateHandle(context);
            await UpdateMain(context, addressInfos);
            await UpdateSupplementary(context, addressInfos);
            await UpdateBankTrace(context);
            await UpdateFinanceCheck(context);
            await InsertCheckProcess(context);
            var errorCount = UpdateCheckLog(context);

            await _context.SaveChangesAsync();
            _context.ChangeTracker.Clear();

            return (true, errorCount);
        }
        catch (Exception ex)
        {
            var caseErrorCount = await 案件處理異常(context.ApplyNo, SystemErrorLogTypeConst.儲存資料庫有誤, "更新案件資料庫發生例外", ex);
            return (false, caseErrorCount);
        }
        finally
        {
            _logger.LogInformation("更新案件資料_完成，申請書編號: {ApplyNo}", context.ApplyNo);
        }
    }

    private string? GetIsOriginalCardholder(命中檢核結果 命中檢核原持卡人) =>
        命中檢核原持卡人 switch
        {
            命中檢核結果.命中 => "Y",
            命中檢核結果.未命中 => "N",
            _ => null,
        };

    private void 正卡與原持卡人資料比對當未填寫時補齊資料(Reviewer_ApplyCreditCardInfoMain main, QueryOriginalCardholderData origin)
    {
        // logger.LogInformation("與原持卡人資料比對當未填寫時補齊資料");
        // logger.LogInformation("原持卡人資料：{origin}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(origin)));
        // logger.LogInformation("原主檔資料：{main}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(main)));

        main.ENName = string.IsNullOrWhiteSpace(main.ENName) ? origin.EnglishName : main.ENName;
        main.CHName = string.IsNullOrWhiteSpace(main.CHName) ? origin.ChineseName : main.CHName;
        main.BirthDay = string.IsNullOrWhiteSpace(main.BirthDay) ? origin.BirthDate : main.BirthDay;
        main.Bill_ZipCode = string.IsNullOrWhiteSpace(main.Bill_ZipCode) ? origin.BillZip : main.Bill_ZipCode;
        main.LivePhone = string.IsNullOrWhiteSpace(main.LivePhone) ? origin.HomeTel : main.LivePhone;
        main.CompPhone = string.IsNullOrWhiteSpace(main.CompPhone) ? origin.CompanyTel : main.CompPhone;
        main.Mobile = string.IsNullOrWhiteSpace(main.Mobile) ? origin.CellTel : main.Mobile;
        main.CompID = string.IsNullOrWhiteSpace(main.CompID) ? origin.UniformNumber : main.CompID;
        main.EMail = string.IsNullOrWhiteSpace(main.EMail) ? origin.Email : main.EMail;
        main.Sex = main.Sex ??= origin.Sex;
        if (
            (string.IsNullOrWhiteSpace(main.Bill_City) && string.IsNullOrWhiteSpace(main.Bill_Road) && string.IsNullOrWhiteSpace(main.Bill_District))
            && string.IsNullOrWhiteSpace(main.Bill_FullAddr)
        )
        {
            var (zipCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.BillAddr);
            main.Bill_ZipCode = zipCode;
            main.Bill_FullAddr = address;
        }

        if (
            (string.IsNullOrWhiteSpace(main.Reg_City) && string.IsNullOrWhiteSpace(main.Reg_Road) && string.IsNullOrWhiteSpace(main.Reg_District))
            && string.IsNullOrWhiteSpace(main.Reg_FullAddr)
        )
        {
            var (zipCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.HomeAddr);
            main.Reg_ZipCode = zipCode;
            main.Reg_FullAddr = address;
        }

        if (
            (string.IsNullOrWhiteSpace(main.Comp_City) && string.IsNullOrWhiteSpace(main.Comp_Road) && string.IsNullOrWhiteSpace(main.Comp_District))
            && string.IsNullOrWhiteSpace(main.Comp_FullAddr)
        )
        {
            var (zipCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.CompanyAddr);
            main.Comp_ZipCode = zipCode;
            main.Comp_FullAddr = address;
        }

        if (
            (
                string.IsNullOrWhiteSpace(main.SendCard_City)
                && string.IsNullOrWhiteSpace(main.SendCard_Road)
                && string.IsNullOrWhiteSpace(main.SendCard_District)
            ) && string.IsNullOrWhiteSpace(main.SendCard_FullAddr)
        )
        {
            var (zipCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.SendAddr);
            main.SendCard_ZipCode = zipCode;
            main.SendCard_FullAddr = address;
        }

        main.CompName = string.IsNullOrWhiteSpace(main.CompName) ? origin.CompanyName : main.CompName;
        main.CompJobTitle = string.IsNullOrWhiteSpace(main.CompJobTitle) ? origin.CompanyTitle : main.CompJobTitle;
        main.Education ??= origin.EducateCode;
        main.MarriageState ??= origin.MarriageCode;
        main.CompSeniority ??= origin.ProfessionPeriod;
        main.CurrentMonthIncome ??= origin.MonthlySalary;
        main.CitizenshipCode = string.IsNullOrWhiteSpace(main.CitizenshipCode) ? origin.National : main.CitizenshipCode;
        main.PassportNo = string.IsNullOrWhiteSpace(main.PassportNo) ? origin.Passport : main.PassportNo;
        main.PassportDate = string.IsNullOrWhiteSpace(main.PassportDate) ? origin.PassportDate : main.PassportDate;
        main.ResidencePermitIssueDate = string.IsNullOrWhiteSpace(main.ResidencePermitIssueDate)
            ? origin.ForeignerIssueDate
            : main.ResidencePermitIssueDate;
        main.GraduatedElementarySchool = string.IsNullOrWhiteSpace(main.GraduatedElementarySchool)
            ? origin.SchoolName
            : main.GraduatedElementarySchool;
        if (
            (string.IsNullOrWhiteSpace(main.Live_City) && string.IsNullOrWhiteSpace(main.Live_Road) && string.IsNullOrWhiteSpace(main.Live_District))
            && string.IsNullOrWhiteSpace(main.Live_FullAddr)
        )
        {
            var (zipCode, address) = AddressHelper.GetPostalCodeAndAddress(origin.ContactAddr);
            main.Live_ZipCode = zipCode;
            main.Live_FullAddr = address;
        }
        main.LiveYear ??= origin.ResideNBR;
        main.CompTrade ??= origin.CompTrade;
        main.CompJobLevel ??= origin.CompJobLevel;

        // logger.LogInformation("比對後主檔資料：{main}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(main)));
    }

    private void 附卡與原持卡人資料比對當未填寫時補齊資料(Reviewer_ApplyCreditCardInfoSupplementary supplementary, QueryOriginalCardholderData origin)
    {
        // logger.LogInformation("與原持卡人資料比對當未填寫時補齊資料");
        // logger.LogInformation("原持卡人資料：{origin}", JsonHelper.序列化物件(MapHelper.MapToCompareMain(origin)));
        // logger.LogInformation("原附檔資料：{supplementary}", JsonHelper.序列化物件(MapHelper.MapToCompareSupplementary(supplementary)));

        supplementary.ENName = string.IsNullOrWhiteSpace(supplementary.ENName) ? origin.EnglishName : supplementary.ENName;
        supplementary.CHName = string.IsNullOrWhiteSpace(supplementary.CHName) ? origin.ChineseName : supplementary.CHName;
        supplementary.BirthDay = string.IsNullOrWhiteSpace(supplementary.BirthDay) ? origin.BirthDate : supplementary.BirthDay;
        supplementary.LivePhone = string.IsNullOrWhiteSpace(supplementary.LivePhone) ? origin.HomeTel : supplementary.LivePhone;
        supplementary.CompPhone = string.IsNullOrWhiteSpace(supplementary.CompPhone) ? origin.CompanyTel : supplementary.CompPhone;
        supplementary.Mobile = string.IsNullOrWhiteSpace(supplementary.Mobile) ? origin.CellTel : supplementary.Mobile;
        supplementary.Sex = supplementary.Sex ??= origin.Sex;

        if (
            (
                string.IsNullOrWhiteSpace(supplementary.SendCard_City)
                && string.IsNullOrWhiteSpace(supplementary.SendCard_Road)
                && string.IsNullOrWhiteSpace(supplementary.SendCard_District)
            ) && string.IsNullOrWhiteSpace(supplementary.SendCard_FullAddr)
        )
        {
            supplementary.SendCard_FullAddr = origin.SendAddr;
        }

        supplementary.CompName = string.IsNullOrWhiteSpace(supplementary.CompName) ? origin.CompanyName : supplementary.CompName;
        supplementary.CompJobTitle = string.IsNullOrWhiteSpace(supplementary.CompJobTitle) ? origin.CompanyTitle : supplementary.CompJobTitle;
        supplementary.MarriageState ??= origin.MarriageCode;
        supplementary.CitizenshipCode = string.IsNullOrWhiteSpace(supplementary.CitizenshipCode) ? origin.National : supplementary.CitizenshipCode;
        supplementary.PassportNo = string.IsNullOrWhiteSpace(supplementary.PassportNo) ? origin.Passport : supplementary.PassportNo;
        supplementary.PassportDate = string.IsNullOrWhiteSpace(supplementary.PassportDate) ? origin.PassportDate : supplementary.PassportDate;
        supplementary.ResidencePermitIssueDate = string.IsNullOrWhiteSpace(supplementary.ResidencePermitIssueDate)
            ? origin.ForeignerIssueDate
            : supplementary.ResidencePermitIssueDate;
        if (
            (
                string.IsNullOrWhiteSpace(supplementary.Live_City)
                && string.IsNullOrWhiteSpace(supplementary.Live_Road)
                && string.IsNullOrWhiteSpace(supplementary.Live_District)
            ) && string.IsNullOrWhiteSpace(supplementary.Live_FullAddr)
        )
        {
            supplementary.Live_FullAddr = origin.ContactAddr;
        }

        // logger.LogInformation("比對後附卡檔資料：{supplementary}", JsonHelper.序列化物件(MapHelper.MapToCompareSupplementary(supplementary)));
    }

    private string 計算郵遞區號(AddressContext address, List<SetUp_AddressInfo> addressInfos)
    {
        var filterAddressInfos = addressInfos.Where(x => x.City == address.City && x.Area == address.District && x.Road == address.Road).ToList();

        if (filterAddressInfos.Count == 0 || filterAddressInfos is null)
        {
            _logger.LogError("縣市：{city} 區域：{district} 街道：{road} 查無郵遞區號", address.City, address.District, address.Road);
            return "";
        }

        var convertAddressInfos = filterAddressInfos
            .Select(x => new AddressInfoDto()
            {
                City = address.City,
                Area = address.District,
                Road = address.Road,
                Scope = x.Scope,
                ZipCode = x.ZIPCode,
            })
            .ToList();

        if (!int.TryParse(address.Number, out int number))
        {
            _logger.LogError("門牌號碼：{number} 無法轉換為數字", address.Number);
            return "";
        }

        var searchAddressInfo = new SearchAddressInfoDto
        {
            City = address.City,
            District = address.District,
            Road = address.Road,
            Number = number,
            SubNumber = int.TryParse(address.SubNumber, out int subNumber) ? subNumber : 0,
            Lane = int.TryParse(address.Lane, out int lane) ? lane : 0,
        };

        var zipCode = AddressHelper.FindZipCode(convertAddressInfos, searchAddressInfo);

        if (string.IsNullOrEmpty(zipCode))
        {
            _logger.LogError(
                "縣市：{city} 區域：{district} 街道：{road} 門牌號碼：{number} 無法轉換郵遞區號",
                address.City,
                address.District,
                address.Road,
                address.Number
            );
        }

        return zipCode;
    }

    private bool 是否計算郵遞區號(AddressContext address)
    {
        if (!string.IsNullOrEmpty(address.ZipCode))
        {
            return false;
        }

        if (
            string.IsNullOrEmpty(address.City)
            || string.IsNullOrEmpty(address.District)
            || string.IsNullOrEmpty(address.Road)
            || string.IsNullOrEmpty(address.Number)
        )
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 更新案件狀態 <br/>
    /// 當任何檢核項目失敗時，更新案件狀態為一般件待月收入預審_檢核異常，若檢核成功則為一般件待月收入預審
    /// </summary>
    /// <param name="context">檢核上下文</param>
    private async Task UpdateHandle(PaperCheckJobContext context)
    {
        bool isCheckCaseSuccess = !context.HasAnyCheckFailed();
        CardStatus afterCardStatus = isCheckCaseSuccess ? CardStatus.紙本件_待月收入預審 : CardStatus.紙本件_待月收入預審_檢核異常;

        _logger.LogInformation("案件更新案件狀態，申請書編號: {ApplyNo} 更新案件狀態為：{afterCardStatus}", context.ApplyNo, afterCardStatus);

        // update handle
        var handles = await _context.Reviewer_ApplyCreditCardInfoHandle.Where(x => x.ApplyNo == context.ApplyNo).ToListAsync();
        foreach (var handle in handles)
        {
            handle.CardStatus = afterCardStatus;
        }
    }

    /// <summary>
    /// 更新正卡人資料 <br/>
    /// 1. 更新 AML 版本 (AMLProfessionCode_Version) <br/>
    /// 2. 更新原持卡人資料，如有命中原持卡人，將使用者未填資料用原持卡人資料補齊 <br/>
    /// 3. 更新是否確認姓名檢核 (IsDunyangBlackList) <br/>
    /// 4. 更新是否為分行客戶 (IsBranchCustomer) <br/>
    /// 5. 更新所有地址郵遞區號
    /// </summary>
    /// <param name="context">檢核上下文</param>
    /// <param name="addressInfos">地址資訊</param>
    /// <param name="maxAMLVersion">最大 AML 版本</param>
    private async Task UpdateMain(PaperCheckJobContext context, List<SetUp_AddressInfo> addressInfos)
    {
        var mainContext = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.正卡人);
        var main = await _context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == context.ApplyNo);
        main.LastUpdateUserId = UserIdConst.SYSTEM;
        main.LastUpdateTime = DateTime.Now;

        if (mainContext.是否檢核原持卡人成功 == 檢核結果.成功)
        {
            main.IsOriginalCardholder = GetIsOriginalCardholder(mainContext.命中檢核原持卡人);
            if (GetIsOriginalCardholder(mainContext.命中檢核原持卡人) == "Y")
            {
                正卡與原持卡人資料比對當未填寫時補齊資料(main, mainContext.原持卡人查詢結果.SuccessData);
                正卡人完整地址計算郵遞區號(main, addressInfos);
            }
        }

        if (mainContext.是否檢核姓名檢核成功 == 檢核結果.成功)
        {
            main.IsDunyangBlackList = mainContext.命中檢核姓名檢核 == 命中檢核結果.命中 ? "Y" : "N";
            main.NameChecked = mainContext.命中檢核姓名檢核 == 命中檢核結果.命中 ? "Y" : "N";
            if (mainContext.命中檢核姓名檢核 == 命中檢核結果.命中 || mainContext.命中檢核姓名檢核 == 命中檢核結果.未命中)
            {
                await _context.Reviewer3rd_NameCheckLog.AddRangeAsync(mainContext.姓名檢核結果.SuccessData.Reviewer3rd_NameCheckLog);
            }
        }

        if (mainContext.是否檢核分行資訊成功 == 檢核結果.成功)
        {
            main.IsBranchCustomer = mainContext.命中檢核分行資訊 == 命中檢核結果.命中 ? "Y" : "N";
        }

        if (mainContext.是否檢查重覆進件成功 == 檢核結果.成功)
        {
            main.IsRepeatApply = mainContext.命中檢查重覆進件 == 命中檢核結果.命中 ? "Y" : "N";
        }

        // update main address 計算郵遞區號
        var regAddress = MapHelper.MapToRegAddress(main);
        if (是否計算郵遞區號(regAddress))
        {
            main.Reg_ZipCode = 計算郵遞區號(regAddress, addressInfos);
        }

        var liveAddress = MapHelper.MapToLiveAddress(main);
        if (是否計算郵遞區號(liveAddress))
        {
            main.Live_ZipCode = 計算郵遞區號(liveAddress, addressInfos);
        }

        var parentLiveAddress = MapHelper.MapToParentLiveAddress(main);
        if (是否計算郵遞區號(parentLiveAddress))
        {
            main.ParentLive_ZipCode = 計算郵遞區號(parentLiveAddress, addressInfos);
        }

        var compAddress = MapHelper.MapToCompAddress(main);
        if (是否計算郵遞區號(compAddress))
        {
            main.Comp_ZipCode = 計算郵遞區號(compAddress, addressInfos);
        }

        var billAddress = MapHelper.MapToBillAddress(main);
        if (是否計算郵遞區號(billAddress))
        {
            main.Bill_ZipCode = 計算郵遞區號(billAddress, addressInfos);
        }

        var sendCardAddress = MapHelper.MapToSendCardAddress(main);
        if (是否計算郵遞區號(sendCardAddress))
        {
            main.SendCard_ZipCode = 計算郵遞區號(sendCardAddress, addressInfos);
        }
    }

    /// <summary>
    /// 更新附卡人資料 <br/>
    /// 1.更新原持卡人資料，如有命中原持卡人，將使用者未填資料用原持卡人資料補齊 <br/>
    /// 2.更新是否確認姓名檢核 (IsDunyangBlackList) <br/>
    /// 3.更新所有地址郵遞區號
    /// </summary>
    /// <param name="context">檢核上下文</param>
    /// <param name="addressInfos">地址資訊</param>
    /// <param name="maxAMLVersion">最大 AML 版本</param>
    private async Task UpdateSupplementary(PaperCheckJobContext context, List<SetUp_AddressInfo> addressInfos)
    {
        if (context.CardOwner == CardOwner.正卡)
        {
            _logger.LogInformation("此案子無附卡人資料，不更新附卡人資料");
            return;
        }

        var supplementaryContext = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.附卡人);
        var supplementary = await _context.Reviewer_ApplyCreditCardInfoSupplementary.FirstOrDefaultAsync(x => x.ApplyNo == context.ApplyNo);

        if (supplementaryContext.是否檢核姓名檢核成功 == 檢核結果.成功)
        {
            supplementary.IsDunyangBlackList = supplementaryContext.命中檢核姓名檢核 == 命中檢核結果.命中 ? "Y" : "N";
            supplementary.NameChecked = supplementaryContext.命中檢核姓名檢核 == 命中檢核結果.命中 ? "Y" : "N";
            if (supplementaryContext.命中檢核姓名檢核 == 命中檢核結果.命中 || supplementaryContext.命中檢核姓名檢核 == 命中檢核結果.未命中)
            {
                await _context.Reviewer3rd_NameCheckLog.AddRangeAsync(supplementaryContext.姓名檢核結果.SuccessData.Reviewer3rd_NameCheckLog);
            }
        }

        if (supplementaryContext.是否檢核原持卡人成功 == 檢核結果.成功)
        {
            supplementary.IsOriginalCardholder = GetIsOriginalCardholder(supplementaryContext.命中檢核原持卡人);
            if (GetIsOriginalCardholder(supplementaryContext.命中檢核原持卡人) == "Y")
            {
                附卡與原持卡人資料比對當未填寫時補齊資料(supplementary, supplementaryContext.原持卡人查詢結果.SuccessData);
            }

            if (supplementaryContext.是否檢查重覆進件成功 == 檢核結果.成功)
            {
                supplementary.IsRepeatApply = supplementaryContext.命中檢查重覆進件 == 命中檢核結果.命中 ? "Y" : "N";
            }
        }

        if (supplementaryContext.是否檢核姓名檢核成功 == 檢核結果.成功)
        {
            supplementary.IsDunyangBlackList = supplementaryContext.命中檢核姓名檢核 == 命中檢核結果.命中 ? "Y" : "N";
        }

        // update supplementary address 計算郵遞區號
        var liveAddress_supplementary = MapHelper.MapToLiveAddressForSupplementary(supplementary);
        if (是否計算郵遞區號(liveAddress_supplementary))
        {
            supplementary.Live_ZipCode = 計算郵遞區號(liveAddress_supplementary, addressInfos);
        }

        var sendCardAddress_supplementary = MapHelper.MapToSendCardAddressForSupplementary(supplementary);
        if (是否計算郵遞區號(sendCardAddress_supplementary))
        {
            supplementary.SendCard_ZipCode = 計算郵遞區號(sendCardAddress_supplementary, addressInfos);
        }
    }

    /// <summary>
    /// 更新銀行內部資料(目前只有正卡人) <br/>
    /// 1. 更新相同 Email (InternalEmailSame_Flag) <br/>
    /// 2. 更新相同 Mobile (InternalMobileSame_Flag) <br/>
    /// 3. 更新相同 IP (ShortTimeID_Flag)
    /// </summary>
    /// <param name="context">檢核上下文</param>
    private async Task UpdateBankTrace(PaperCheckJobContext context)
    {
        /*
            紙本件無以下驗證項目
            1. 相同 Email
            2. 相同 Mobile
            3. 相同 IP
        */
        var mainContext = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.正卡人);

        var bankTrace = new Reviewer_BankTrace()
        {
            ApplyNo = context.ApplyNo,
            ID = mainContext.ID,
            UserType = mainContext.UserType,
        };
        _context.Attach(bankTrace);

        if (mainContext.是否檢核行內Email成功 == 檢核結果.成功)
        {
            _context.Entry(bankTrace).Property(x => x.InternalEmailSame_Flag).IsModified = true;
            bankTrace.InternalEmailSame_Flag = mainContext.命中檢核行內Email == 命中檢核結果.命中 ? "Y" : "N";
            if (mainContext.命中檢核行內Email == 命中檢核結果.命中)
            {
                await _context.Reviewer_BankInternalSameLog.AddRangeAsync(mainContext.行內Email檢核結果.SuccessData!);
            }
        }

        if (mainContext.是否檢核行內Mobile成功 == 檢核結果.成功)
        {
            _context.Entry(bankTrace).Property(x => x.InternalMobileSame_Flag).IsModified = true;
            bankTrace.InternalMobileSame_Flag = mainContext.命中檢核行內Mobile == 命中檢核結果.命中 ? "Y" : "N";
            if (mainContext.命中檢核行內Email == 命中檢核結果.命中)
            {
                await _context.Reviewer_BankInternalSameLog.AddRangeAsync(mainContext.行內Email檢核結果.SuccessData!);
            }
        }

        if (mainContext.是否檢核頻繁ID成功 == 檢核結果.成功)
        {
            _context.Entry(bankTrace).Property(x => x.ShortTimeID_Flag).IsModified = true;
            bankTrace.ShortTimeID_Flag = mainContext.命中檢核頻繁ID == 命中檢核結果.命中 ? "Y" : "N";
            if (mainContext.命中檢核頻繁ID == 命中檢核結果.命中)
            {
                await _context.Reviewer_CheckTrace.AddRangeAsync(mainContext.頻繁ID檢核結果.SuccessData!);
            }
        }
    }

    /// <summary>
    /// 更新財力檢核資料(正附卡人皆需要) <br/>
    /// 1. 更新姓名檢核 (NameChecked) <br/>
    /// 2. 更新929檢核 (Checked929) <br/>
    /// 3. 更新關注名單檢核 (Focus1Check, Focus2Check) <br/>
    /// 4. 更新分行資訊檢核 (IsBranchCustomer)
    /// </summary>
    /// <param name="context">檢核上下文</param>
    private async Task UpdateFinanceCheck(PaperCheckJobContext context)
    {
        var mainContext = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.正卡人);

        // update main finance check
        var mainFinanceCheck = new Reviewer_FinanceCheckInfo()
        {
            ApplyNo = context.ApplyNo,
            ID = mainContext.ID,
            UserType = mainContext.UserType,
        };
        _context.Attach(mainFinanceCheck);

        if (mainContext.是否檢核929成功 == 檢核結果.成功)
        {
            _context.Entry(mainFinanceCheck).Property(x => x.Checked929).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Q929_RtnCode).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Q929_RtnMsg).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Q929_QueryTime).IsModified = true;

            var check929ResData = mainContext.查詢929結果.SuccessData;
            mainFinanceCheck.Checked929 = mainContext.命中檢核929 == 命中檢核結果.命中 ? "Y" : "N";
            mainFinanceCheck.Q929_RtnCode = check929ResData.RtnCode;
            mainFinanceCheck.Q929_RtnMsg = check929ResData.RtnMsg;
            mainFinanceCheck.Q929_QueryTime = check929ResData.QueryTime;

            if (mainContext.命中檢核929 == 命中檢核結果.命中)
            {
                await _context.Reviewer3rd_929Log.AddRangeAsync(check929ResData.Reviewer3rd_929Logs);
            }
        }

        if (mainContext.是否檢核關注名單成功 == 檢核結果.成功)
        {
            _context.Entry(mainFinanceCheck).Property(x => x.Focus1Check).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus1Hit).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus1_RtnCode).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus1_RtnMsg).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus1_QueryTime).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus1_TraceId).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus2Check).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus2Hit).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus2_RtnCode).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus2_RtnMsg).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus2_QueryTime).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.Focus2_TraceId).IsModified = true;

            var focusResData = mainContext.關注名單查詢結果.SuccessData;
            mainFinanceCheck.Focus1Check = mainContext.命中檢核關注名單1 == 命中檢核結果.命中 ? "Y" : "N";
            mainFinanceCheck.Focus1_RtnCode = focusResData.RtnCode;
            mainFinanceCheck.Focus1_RtnMsg = focusResData.RtnMsg;
            mainFinanceCheck.Focus1_QueryTime = focusResData.QueryTime;
            mainFinanceCheck.Focus1_TraceId = focusResData.TraceId;
            mainFinanceCheck.Focus1Hit = String.Join("、", focusResData.Focus1HitList);

            mainFinanceCheck.Focus2Check = mainContext.命中檢核關注名單2 == 命中檢核結果.命中 ? "Y" : "N";
            mainFinanceCheck.Focus2_RtnCode = focusResData.RtnCode;
            mainFinanceCheck.Focus2_RtnMsg = focusResData.RtnMsg;
            mainFinanceCheck.Focus2_QueryTime = focusResData.QueryTime;
            mainFinanceCheck.Focus2_TraceId = focusResData.TraceId;
            mainFinanceCheck.Focus2Hit = String.Join("、", focusResData.Focus2HitList);

            // insert 關注名單
            if (mainContext.命中檢核關注名單1 == 命中檢核結果.命中 || mainContext.命中檢核關注名單2 == 命中檢核結果.命中)
            {
                if (focusResData.WarningCompanyLogs.Count > 0)
                {
                    await _context.Reviewer3rd_WarnCompLog.AddRangeAsync(focusResData.WarningCompanyLogs);
                }

                if (focusResData.RiskAccountLogs.Count > 0)
                {
                    await _context.Reviewer3rd_RiskAccountLog.AddRangeAsync(focusResData.RiskAccountLogs);
                }

                if (focusResData.WarnLogs.Count > 0)
                {
                    await _context.Reviewer3rd_WarnLog.AddRangeAsync(focusResData.WarnLogs);
                }

                if (focusResData.FledLogs.Count > 0)
                {
                    await _context.Reviewer3rd_FledLog.AddRangeAsync(focusResData.FledLogs);
                }

                if (focusResData.PunishLogs.Count > 0)
                {
                    await _context.Reviewer3rd_PunishLog.AddRangeAsync(focusResData.PunishLogs);
                }

                if (focusResData.ImmiLogs.Count > 0)
                {
                    await _context.Reviewer3rd_ImmiLog.AddRangeAsync(focusResData.ImmiLogs);
                }

                if (focusResData.FrdIdLogs.Count > 0)
                {
                    await _context.Reviewer3rd_FrdIdLog.AddRangeAsync(focusResData.FrdIdLogs);
                }

                if (focusResData.LayOffLogs.Count > 0)
                {
                    await _context.Reviewer3rd_LayOffLog.AddRangeAsync(focusResData.LayOffLogs);
                }
            }

            // 失蹤人口 (G) 無論如何都會有資料
            if (focusResData.MissingPersonsLogs != null)
            {
                await _context.Reviewer3rd_MissingPersonsLog.AddAsync(focusResData.MissingPersonsLogs);
            }
        }

        if (mainContext.是否檢核分行資訊成功 == 檢核結果.成功)
        {
            _context.Entry(mainFinanceCheck).Property(x => x.IsBranchCustomer).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.BranchCus_RtnCode).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.BranchCus_RtnMsg).IsModified = true;
            _context.Entry(mainFinanceCheck).Property(x => x.BranchCus_QueryTime).IsModified = true;

            var queryBranchInfoResData = mainContext.分行資訊查詢結果.SuccessData;
            mainFinanceCheck.IsBranchCustomer = mainContext.命中檢核分行資訊 == 命中檢核結果.命中 ? "Y" : "N";
            mainFinanceCheck.BranchCus_RtnCode = queryBranchInfoResData.RtnCode;
            mainFinanceCheck.BranchCus_RtnMsg = queryBranchInfoResData.RtnMsg;
            mainFinanceCheck.BranchCus_QueryTime = queryBranchInfoResData.QueryTime;

            if (mainContext.命中檢核分行資訊 == 命中檢核結果.命中)
            {
                if (queryBranchInfoResData.BranchCusCusInfo.Count > 0)
                {
                    await _context.Reviewer3rd_BranchCusCusInfo.AddRangeAsync(queryBranchInfoResData.BranchCusCusInfo);
                }

                if (queryBranchInfoResData.BranchCusWMCust.Count > 0)
                {
                    await _context.Reviewer3rd_BranchCusWMCust.AddRangeAsync(queryBranchInfoResData.BranchCusWMCust);
                }

                if (queryBranchInfoResData.BranchCusCD.Count > 0)
                {
                    await _context.Reviewer3rd_BranchCusCD.AddRangeAsync(queryBranchInfoResData.BranchCusCD);
                }

                if (queryBranchInfoResData.BranchCusDD.Count > 0)
                {
                    await _context.Reviewer3rd_BranchCusDD.AddRangeAsync(queryBranchInfoResData.BranchCusDD);
                }

                if (queryBranchInfoResData.BranchCusCAD.Count > 0)
                {
                    await _context.Reviewer3rd_BranchCusCAD.AddRangeAsync(queryBranchInfoResData.BranchCusCAD);
                }

                if (queryBranchInfoResData.BranchCusCreditOver.Count > 0)
                {
                    await _context.Reviewer3rd_BranchCusCreditOver.AddRangeAsync(queryBranchInfoResData.BranchCusCreditOver);
                }
            }
        }

        if (context.CardOwner == CardOwner.正卡)
        {
            return;
        }

        var supplementaryContext = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.附卡人);
        if (supplementaryContext == null)
        {
            throw new Exception($"案件編號：{context.ApplyNo}，卡片類型：{context.CardOwner}，但附卡人資料不存在，請檢查資料");
        }

        var supplementaryFinanceCheck = new Reviewer_FinanceCheckInfo()
        {
            ApplyNo = context.ApplyNo,
            ID = supplementaryContext.ID,
            UserType = supplementaryContext.UserType,
        };
        _context.Attach(supplementaryFinanceCheck);

        if (supplementaryContext.是否檢核929成功 == 檢核結果.成功)
        {
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Checked929).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Q929_RtnCode).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Q929_RtnMsg).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Q929_QueryTime).IsModified = true;

            var check929ResData = supplementaryContext.查詢929結果.SuccessData;
            supplementaryFinanceCheck.Checked929 = supplementaryContext.命中檢核929 == 命中檢核結果.命中 ? "Y" : "N";
            supplementaryFinanceCheck.Q929_RtnCode = check929ResData.RtnCode;
            supplementaryFinanceCheck.Q929_RtnMsg = check929ResData.RtnMsg;
            supplementaryFinanceCheck.Q929_QueryTime = check929ResData.QueryTime;

            if (supplementaryContext.命中檢核929 == 命中檢核結果.命中)
            {
                await _context.Reviewer3rd_929Log.AddRangeAsync(check929ResData.Reviewer3rd_929Logs);
            }
        }

        if (supplementaryContext.是否檢核關注名單成功 == 檢核結果.成功)
        {
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus1Check).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus1Hit).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus1_RtnCode).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus1_RtnMsg).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus1_QueryTime).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus1_TraceId).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus2Check).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus2Hit).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus2_RtnCode).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus2_RtnMsg).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus2_QueryTime).IsModified = true;
            _context.Entry(supplementaryFinanceCheck).Property(x => x.Focus2_TraceId).IsModified = true;

            var focusResData = supplementaryContext.關注名單查詢結果.SuccessData;
            supplementaryFinanceCheck.Focus1Check = supplementaryContext.命中檢核關注名單1 == 命中檢核結果.命中 ? "Y" : "N";
            supplementaryFinanceCheck.Focus1_RtnCode = focusResData.RtnCode;
            supplementaryFinanceCheck.Focus1_RtnMsg = focusResData.RtnMsg;
            supplementaryFinanceCheck.Focus1_QueryTime = focusResData.QueryTime;
            supplementaryFinanceCheck.Focus1_TraceId = focusResData.TraceId;
            supplementaryFinanceCheck.Focus1Hit = String.Join("、", focusResData.Focus1HitList);

            supplementaryFinanceCheck.Focus2Check = supplementaryContext.命中檢核關注名單2 == 命中檢核結果.命中 ? "Y" : "N";
            supplementaryFinanceCheck.Focus2_RtnCode = focusResData.RtnCode;
            supplementaryFinanceCheck.Focus2_RtnMsg = focusResData.RtnMsg;
            supplementaryFinanceCheck.Focus2_QueryTime = focusResData.QueryTime;
            supplementaryFinanceCheck.Focus2_TraceId = focusResData.TraceId;
            supplementaryFinanceCheck.Focus2Hit = String.Join("、", focusResData.Focus2HitList);

            // insert 關注名單
            if (supplementaryContext.命中檢核關注名單1 == 命中檢核結果.命中 || supplementaryContext.命中檢核關注名單2 == 命中檢核結果.命中)
            {
                if (focusResData.WarningCompanyLogs.Count > 0)
                {
                    await _context.Reviewer3rd_WarnCompLog.AddRangeAsync(focusResData.WarningCompanyLogs);
                }

                if (focusResData.RiskAccountLogs.Count > 0)
                {
                    await _context.Reviewer3rd_RiskAccountLog.AddRangeAsync(focusResData.RiskAccountLogs);
                }

                if (focusResData.WarnLogs.Count > 0)
                {
                    await _context.Reviewer3rd_WarnLog.AddRangeAsync(focusResData.WarnLogs);
                }

                if (focusResData.FledLogs.Count > 0)
                {
                    await _context.Reviewer3rd_FledLog.AddRangeAsync(focusResData.FledLogs);
                }

                if (focusResData.PunishLogs.Count > 0)
                {
                    await _context.Reviewer3rd_PunishLog.AddRangeAsync(focusResData.PunishLogs);
                }

                if (focusResData.ImmiLogs.Count > 0)
                {
                    await _context.Reviewer3rd_ImmiLog.AddRangeAsync(focusResData.ImmiLogs);
                }

                if (focusResData.FrdIdLogs.Count > 0)
                {
                    await _context.Reviewer3rd_FrdIdLog.AddRangeAsync(focusResData.FrdIdLogs);
                }

                if (focusResData.LayOffLogs.Count > 0)
                {
                    await _context.Reviewer3rd_LayOffLog.AddRangeAsync(focusResData.LayOffLogs);
                }
            }

            // 失蹤人口 (G) 無論如何都會有資料
            if (focusResData.MissingPersonsLogs != null)
            {
                await _context.Reviewer3rd_MissingPersonsLog.AddAsync(focusResData.MissingPersonsLogs);
            }
        }
    }

    /// <summary>
    /// 新增檢核流程
    /// 當檢核項目有成功或失敗時，才新增檢核流程
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task InsertCheckProcess(PaperCheckJobContext context)
    {
        List<Reviewer_ApplyCreditCardInfoProcess> processes = new();
        foreach (var userCheckResult in context.UserCheckResults)
        {
            if (userCheckResult.是否檢核原持卡人成功 == 檢核結果.成功 || userCheckResult.是否檢核原持卡人成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成原持卡人資料查詢,
                    userCheckResult.原持卡人查詢結果.StartTime,
                    userCheckResult.原持卡人查詢結果.EndTime,
                    userCheckResult.是否檢核原持卡人成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.查詢原持卡人資料錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢核行內Email成功 == 檢核結果.成功 || userCheckResult.是否檢核行內Email成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成行內Email檢核,
                    userCheckResult.行內Email檢核結果.StartTime,
                    userCheckResult.行內Email檢核結果.EndTime,
                    userCheckResult.是否檢核行內Email成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.行內Email檢核錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢核行內Mobile成功 == 檢核結果.成功 || userCheckResult.是否檢核行內Mobile成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成行內手機檢核,
                    userCheckResult.行內Mobile檢核結果.StartTime,
                    userCheckResult.行內Mobile檢核結果.EndTime,
                    userCheckResult.是否檢核行內Mobile成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.行內手機檢核錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢核姓名檢核成功 == 檢核結果.成功 || userCheckResult.是否檢核姓名檢核成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成姓名檢核查詢,
                    userCheckResult.姓名檢核結果.StartTime,
                    userCheckResult.姓名檢核結果.EndTime,
                    userCheckResult.是否檢核姓名檢核成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.查詢姓名檢核錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢核929成功 == 檢核結果.成功 || userCheckResult.是否檢核929成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成929業務狀況查詢,
                    userCheckResult.查詢929結果.StartTime,
                    userCheckResult.查詢929結果.EndTime,
                    userCheckResult.是否檢核929成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.查詢929業務狀況錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢核分行資訊成功 == 檢核結果.成功 || userCheckResult.是否檢核分行資訊成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成分行資訊查詢,
                    userCheckResult.分行資訊查詢結果.StartTime,
                    userCheckResult.分行資訊查詢結果.EndTime,
                    userCheckResult.是否檢核分行資訊成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.分行資訊查詢錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢核關注名單成功 == 檢核結果.成功 || userCheckResult.是否檢核關注名單成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成關注名單1查詢,
                    userCheckResult.關注名單查詢結果.StartTime,
                    userCheckResult.關注名單查詢結果.EndTime,
                    userCheckResult.是否檢核關注名單成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.關注名單1查詢失敗})"
                );
                processes.Add(process);
                var process2 = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成關注名單2查詢,
                    userCheckResult.關注名單查詢結果.StartTime,
                    userCheckResult.關注名單查詢結果.EndTime,
                    userCheckResult.是否檢核關注名單成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.關注名單2查詢失敗})"
                );
                processes.Add(process2);
            }

            if (userCheckResult.是否檢核頻繁ID成功 == 檢核結果.成功 || userCheckResult.是否檢核頻繁ID成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成頻繁ID檢核,
                    userCheckResult.頻繁ID檢核結果.StartTime,
                    userCheckResult.頻繁ID檢核結果.EndTime,
                    userCheckResult.是否檢核頻繁ID成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.頻繁ID檢核錯誤})"
                );
                processes.Add(process);
            }

            if (userCheckResult.是否檢查重覆進件成功 == 檢核結果.成功 || userCheckResult.是否檢查重覆進件成功 == 檢核結果.失敗)
            {
                var process = MapHelper.MapToProcess(
                    context.ApplyNo,
                    ProcessConst.完成重覆進件檢核,
                    userCheckResult.重覆進件檢核結果.StartTime,
                    userCheckResult.重覆進件檢核結果.EndTime,
                    userCheckResult.是否檢查重覆進件成功 == 檢核結果.成功
                        ? $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})"
                        : $"({userCheckResult.UserType.ToString()}_{userCheckResult.ID})({ProcessNoteConst.重覆進件檢核錯誤})"
                );
                processes.Add(process);
            }
        }

        bool isCheckCaseSuccess = context.HasAnyCheckFailed();
        CardStatus afterCardStatus = isCheckCaseSuccess ? CardStatus.紙本件_待月收入預審_檢核異常 : CardStatus.紙本件_待月收入預審;
        processes.Add(MapHelper.MapToProcess(context.ApplyNo, afterCardStatus.ToString(), DateTime.Now, DateTime.Now));
        await _context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
    }

    private int UpdateCheckLog(PaperCheckJobContext context)
    {
        var checkLog = new ReviewerPedding_PaperApplyCardCheckJob() { ApplyNo = context.ApplyNo };
        _context.Attach(checkLog);
        /*
            我們檢核裡包刮正卡人以及附卡人(看案件)，
            只要任一失敗就算該項目檢核失敗，
            查詢時間押上正卡人的時間即可，
            因為正卡人項目 > 附卡人項目
        */
        var mainContext = context.UserCheckResults.FirstOrDefault(x => x.UserType == UserType.正卡人);

        if (context.案件是否檢核原持卡人)
        {
            _context.Entry(checkLog).Property(x => x.IsQueryOriginalCardholderData).IsModified = true;
            _context.Entry(checkLog).Property(x => x.QueryOriginalCardholderDataLastTime).IsModified = true;
            checkLog.IsQueryOriginalCardholderData = context.UserCheckResults.Any(x =>
                x.是否檢核原持卡人成功 == 檢核結果.失敗 || x.是否檢核原持卡人成功 == 檢核結果.等待
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.QueryOriginalCardholderDataLastTime = mainContext.原持卡人查詢結果.EndTime;
        }

        if (context.案件是否檢核行內Email)
        {
            _context.Entry(checkLog).Property(x => x.IsCheckInternalEmail).IsModified = true;
            _context.Entry(checkLog).Property(x => x.CheckInternalEmailLastTime).IsModified = true;
            checkLog.IsCheckInternalEmail = context.UserCheckResults.Any(x =>
                x.是否檢核行內Email成功 != 檢核結果.成功 && x.是否檢核行內Email成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.CheckInternalEmailLastTime = mainContext.行內Email檢核結果.EndTime;
        }

        if (context.案件是否檢核行內Mobile)
        {
            _context.Entry(checkLog).Property(x => x.IsCheckInternalMobile).IsModified = true;
            _context.Entry(checkLog).Property(x => x.CheckInternalMobileLastTime).IsModified = true;
            checkLog.IsCheckInternalMobile = context.UserCheckResults.Any(x =>
                x.是否檢核行內Mobile成功 != 檢核結果.成功 && x.是否檢核行內Mobile成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.CheckInternalMobileLastTime = mainContext.行內Mobile檢核結果.EndTime;
        }

        if (context.案件是否檢核姓名檢核)
        {
            _context.Entry(checkLog).Property(x => x.IsCheckName).IsModified = true;
            _context.Entry(checkLog).Property(x => x.CheckNameLastTime).IsModified = true;
            checkLog.IsCheckName = context.UserCheckResults.Any(x =>
                x.是否檢核姓名檢核成功 != 檢核結果.成功 && x.是否檢核姓名檢核成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.CheckNameLastTime = mainContext.姓名檢核結果.EndTime;
        }

        if (context.案件是否檢核929)
        {
            _context.Entry(checkLog).Property(x => x.IsCheck929).IsModified = true;
            _context.Entry(checkLog).Property(x => x.Check929LastTime).IsModified = true;
            checkLog.IsCheck929 = context.UserCheckResults.Any(x => x.是否檢核929成功 != 檢核結果.成功 && x.是否檢核929成功 != 檢核結果.不須檢核)
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.Check929LastTime = mainContext.查詢929結果.EndTime;
        }

        if (context.案件是否檢核分行資訊)
        {
            _context.Entry(checkLog).Property(x => x.IsQueryBranchInfo).IsModified = true;
            _context.Entry(checkLog).Property(x => x.QueryBranchInfoLastTime).IsModified = true;
            checkLog.IsQueryBranchInfo = context.UserCheckResults.Any(x =>
                x.是否檢核分行資訊成功 != 檢核結果.成功 && x.是否檢核分行資訊成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.QueryBranchInfoLastTime = mainContext.分行資訊查詢結果.EndTime;
        }

        if (context.案件是否檢核關注名單)
        {
            _context.Entry(checkLog).Property(x => x.IsCheckFocus).IsModified = true;
            _context.Entry(checkLog).Property(x => x.CheckFocusLastTime).IsModified = true;
            checkLog.IsCheckFocus = context.UserCheckResults.Any(x =>
                x.是否檢核關注名單成功 != 檢核結果.成功 && x.是否檢核關注名單成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.CheckFocusLastTime = mainContext.關注名單查詢結果.EndTime;
        }

        if (context.案件是否檢核頻繁ID)
        {
            _context.Entry(checkLog).Property(x => x.IsCheckShortTimeID).IsModified = true;
            _context.Entry(checkLog).Property(x => x.CheckShortTimeIDLastTime).IsModified = true;
            checkLog.IsCheckShortTimeID = context.UserCheckResults.Any(x =>
                x.是否檢核頻繁ID成功 != 檢核結果.成功 && x.是否檢核頻繁ID成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.CheckShortTimeIDLastTime = mainContext.頻繁ID檢核結果.EndTime;
        }

        if (context.案件是否檢查重覆進件)
        {
            _context.Entry(checkLog).Property(x => x.IsCheckRepeatApply).IsModified = true;
            _context.Entry(checkLog).Property(x => x.CheckRepeatApplyLastTime).IsModified = true;
            checkLog.IsCheckRepeatApply = context.UserCheckResults.Any(x =>
                x.是否檢查重覆進件成功 != 檢核結果.成功 && x.是否檢查重覆進件成功 != 檢核結果.不須檢核
            )
                ? CaseCheckStatus.需檢核_失敗
                : CaseCheckStatus.需檢核_成功;
            checkLog.CheckRepeatApplyLastTime = mainContext.重覆進件檢核結果.EndTime;
        }

        _context.Entry(checkLog).Property(x => x.IsChecked).IsModified = true;
        _context.Entry(checkLog).Property(x => x.ErrorCount).IsModified = true;
        checkLog.IsChecked = !context.HasAnyCheckFailed() ? CaseCheckedStatus.完成 : CaseCheckedStatus.未完成;
        int errorCount = context.HasAnyCheckFailed() ? context.ErrorCount + 1 : 0;
        checkLog.ErrorCount = errorCount;

        return errorCount;
    }

    public async Task 新增系統錯誤紀錄(List<System_ErrorLog> systemErrorLogs)
    {
        await _context.System_ErrorLog.AddRangeAsync(systemErrorLogs);
        await _context.SaveChangesAsync();
    }

    private string 完整地址計算郵遞區號(string fullAddress, List<SetUp_AddressInfo> addressInfos)
    {
        try
        {
            var (city, area) = AddressHelper.GetCityAndDistrict(fullAddress);
            var addressInfo = addressInfos.FirstOrDefault(x => x.City == city && x.Area == area);

            if (addressInfo == null)
                return string.Empty;

            return AddressHelper.ZipCodeFormatZero(addressInfo.ZIPCode, 2);
        }
        catch (Exception ex)
        {
            _logger.LogError("計算郵遞區號發生例外，{FullAddress}，{Exception}", fullAddress, ex);
            return string.Empty;
        }
    }

    private void 正卡人完整地址計算郵遞區號(Reviewer_ApplyCreditCardInfoMain main, List<SetUp_AddressInfo> addressInfos)
    {
        if (string.IsNullOrWhiteSpace(main.Reg_ZipCode) && !string.IsNullOrWhiteSpace(main.Reg_FullAddr))
        {
            var result = 完整地址計算郵遞區號(main.Reg_FullAddr, addressInfos);
            if (!string.IsNullOrWhiteSpace(result))
            {
                main.Reg_ZipCode = result;
            }
        }

        if (string.IsNullOrWhiteSpace(main.SendCard_ZipCode) && !string.IsNullOrWhiteSpace(main.SendCard_FullAddr))
        {
            var result = 完整地址計算郵遞區號(main.SendCard_FullAddr, addressInfos);
            if (!string.IsNullOrWhiteSpace(result))
            {
                main.SendCard_ZipCode = result;
            }
        }

        if (string.IsNullOrWhiteSpace(main.Bill_ZipCode) && !string.IsNullOrWhiteSpace(main.Bill_FullAddr))
        {
            var result = 完整地址計算郵遞區號(main.Bill_FullAddr, addressInfos);
            if (!string.IsNullOrWhiteSpace(result))
            {
                main.Bill_ZipCode = result;
            }
        }

        if (string.IsNullOrWhiteSpace(main.Live_ZipCode) && !string.IsNullOrWhiteSpace(main.Live_FullAddr))
        {
            var result = 完整地址計算郵遞區號(main.Live_FullAddr, addressInfos);
            if (!string.IsNullOrWhiteSpace(result))
            {
                main.Live_ZipCode = result;
            }
        }

        if (string.IsNullOrWhiteSpace(main.Comp_ZipCode) && !string.IsNullOrWhiteSpace(main.Comp_FullAddr))
        {
            var result = 完整地址計算郵遞區號(main.Comp_FullAddr, addressInfos);
            if (!string.IsNullOrWhiteSpace(result))
            {
                main.Comp_ZipCode = result;
            }
        }
    }
}
