using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer3rd.Get929ByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 查詢929 API (查詢案件正附卡人)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Reviewer3rd/Get929ByApplyNo/20240803B0001
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("Get929ByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢929業務狀況成功_2000_ResEx),
            typeof(查詢929業務狀況_4003_ResEx),
            typeof(查詢929業務狀況失敗_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> Get929ByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.Get929ByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(
        ScoreSharpContext context,
        IMW3ProcAdapter mw3Adapter,
        IJWTProfilerHelper jwtProfilerHelper,
        IReviewerHelper reviewerHelper,
        ILogger<Handler> logger)
        : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var checkApplyNo = request.applyNo;
            var checkIDList = await 取得申請信用卡身分證(checkApplyNo);

            if (checkIDList.Count == 0)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "信用卡申請資料查無正附卡人ID");
            }

            DateTime now = DateTime.Now;

            var check929Task = checkIDList.Select(async item =>
            {
                var response = await mw3Adapter.QueryOCSI929(item.ID);
                return new
                {
                    id = item.ID,
                    userType = item.UserType,
                    response,
                };
            });
            var check929Results = await Task.WhenAll(check929Task);

            // 新增錯誤標記變數
            bool hasApiError = false;
            string? finalErrorMessage = null;

            List<Reviewer3rd_929Log> _929Logs = [];
            List<Reviewer_ApplyCreditCardInfoProcess> processes = [];

            foreach (var check929Result in check929Results)
            {
                var id = check929Result.id;
                var userType = check929Result.userType;

                // 處理 HTTP 層級失敗（IsSuccess = false）
                if (!check929Result.response.IsSuccess)
                {
                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢929業務狀況失敗";
                    var process = MapToProcess(checkApplyNo, now, ProcessNoteConst.查詢929業務狀況錯誤);
                    processes.Add(process);
                    continue; // 跳過此筆，繼續處理下一筆
                }

                // HTTP 成功，處理業務邏輯
                var oCSI929ApiResponse = check929Result.response.Data!;
                var code = oCSI929ApiResponse.RtnCode;

                if (code == MW3RtnCodeConst.成功)
                {
                    var oCSI929ApiDatas = oCSI929ApiResponse
                        .Info.Table.Select(item => new Reviewer3rd_929Log
                        {
                            ApplyNo = checkApplyNo,
                            TxnDate = item.TxnDate != null ? item.TxnDate.Trim() : string.Empty,
                            BrachCode = item.BrachCode != null ? item.BrachCode.Trim() : string.Empty,
                            BrachEmp = item.BrachEmp != null ? item.BrachEmp.Trim() : string.Empty,
                            BusinessCode = item.BusinessCode != null ? item.BusinessCode.Trim() : string.Empty,
                            ChName = item.ChName != null ? item.ChName.Trim() : string.Empty,
                            LoginDate = item.LoginDate != null ? item.LoginDate.Trim() : string.Empty,
                            ApplyCause = item.ApplyCause != null ? item.ApplyCause.Trim() : string.Empty,
                            ApplyReMark = item.ApplyReMark != null ? item.ApplyReMark.Trim() : string.Empty,
                            ID = id,
                            UserType = userType,
                        })
                        .ToList();
                    var process = MapToProcess(checkApplyNo, now, note: $"({userType}_{id})");
                    processes.Add(process);
                    _929Logs.AddRange(oCSI929ApiDatas);
                }
                else if (code == MW3RtnCodeConst.查詢929_查無資料)
                {
                    var process = MapToProcess(checkApplyNo, now, note: $"({userType}_{id})");
                    processes.Add(process);
                }
                else if (
                    code == MW3RtnCodeConst.查詢929_此服務已失效
                    || code == MW3RtnCodeConst.查詢929_交易有誤
                    || code == MW3RtnCodeConst.查詢929_傳入規格不符合
                    || code == MW3RtnCodeConst.查詢929_聯絡系統管理員
                )
                {
                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢929業務狀況失敗";
                    var process = MapToProcess(checkApplyNo, now, ProcessNoteConst.查詢929業務狀況錯誤);
                    processes.Add(process);
                }
                else
                {
                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢929業務狀況失敗";
                    var process = MapToProcess(checkApplyNo, now, ProcessNoteConst.查詢929業務狀況錯誤);
                    processes.Add(process);
                }
            }

            var financeCheck = await context.Reviewer_FinanceCheckInfo.Where(x => x.ApplyNo == checkApplyNo).ToListAsync();
            foreach (var check in financeCheck)
            {
                var check929Result = check929Results.FirstOrDefault(x => x.id == check.ID && x.userType == check.UserType);
                if (check929Result != null)
                {
                    // 如果 HTTP 失敗，記錄 log 但不寫入詳細錯誤到資料庫
                    if (!check929Result.response.IsSuccess)
                    {
                        logger.LogError(
                            "查詢929失敗 - ApplyNo: {ApplyNo}, ID: {ID}, UserType: {UserType}, ErrorMessage: {ErrorMessage}",
                            checkApplyNo,
                            check.ID,
                            check.UserType,
                            check929Result.response.ErrorMessage
                        );

                        check.Checked929 = null; // 未完成檢核，可重試
                        check.Q929_QueryTime = now;
                        check.Q929_RtnMsg = null;
                        check.Q929_RtnCode = null;
                    }
                    else
                    {
                        var rtnCode = check929Result.response.Data.RtnCode;

                        // 只有「成功」或「查無資料」才設定 Checked929
                        if (rtnCode == MW3RtnCodeConst.成功)
                        {
                            check.Checked929 = "Y";
                        }
                        else if (rtnCode == MW3RtnCodeConst.查詢929_查無資料)
                        {
                            check.Checked929 = "N";
                        }
                        else
                        {
                            // 其他業務錯誤（服務失效、交易有誤等）也保持 NULL
                            check.Checked929 = null;

                            logger.LogWarning(
                                "查詢929業務錯誤 - ApplyNo: {ApplyNo}, ID: {ID}, UserType: {UserType}, RtnCode: {RtnCode}, Response: {Response}",
                                checkApplyNo,
                                check.ID,
                                check.UserType,
                                rtnCode,
                                check929Result.response.Data.Response
                            );
                        }

                        check.Q929_QueryTime = now;
                        check.Q929_RtnMsg = check929Result.response.Data.Response;
                        check.Q929_RtnCode = rtnCode;
                    }
                }
            }

            await context.Reviewer3rd_929Log.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();

            if (_929Logs.Count > 0)
            {
                await context.Reviewer3rd_929Log.AddRangeAsync(_929Logs);
            }

            await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
            await context.SaveChangesAsync();

            var isUpdateSuccessful = await reviewerHelper.UpdateMainLastModified(checkApplyNo, jwtProfilerHelper.UserId, now) == 1;
            if (!isUpdateSuccessful)
            {
                return ApiResponseHelper.InternalServerError<string>(null, "更新Main最後異動資訊失敗");
            }

            // 根據是否有錯誤返回不同結果
            if (hasApiError)
            {
                return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, finalErrorMessage ?? "查詢929業務狀況時發生錯誤");
            }

            return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢929業務狀況完畢");
        }

        private async Task<List<Query929IDDto>> 取得申請信用卡身分證(string applyNo)
        {
            return await context
                .Reviewer_FinanceCheckInfo.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .Select(x => new Query929IDDto { UserType = x.UserType, ID = x.ID })
                .ToListAsync();
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string? note = null) =>
            new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                ProcessUserId = jwtProfilerHelper.UserId,
                Process = ProcessConst.完成929業務狀況查詢,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
            };
    }
}
