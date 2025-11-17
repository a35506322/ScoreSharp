using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer3rd.GetNameCheck;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 查詢姓名檢核 API (查詢案件正附卡人姓名檢核)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     正卡人: /Reviewer3rd/GetNameCheck/20240803B0001?CHName=王小明&amp;UserType=1
        ///     附卡人: /Reviewer3rd/GetNameCheck/20240803B0001?CHName=王小美&amp;UserType=2
        ///
        /// 注意：對於附卡人，系統會根據申請書編號和姓名來識別特定的附卡人資料
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢姓名檢核成功_2000_ResEx),
            typeof(查詢姓名檢核成功_附卡人_2000_ResEx),
            typeof(查詢姓名檢核失敗_5002_ResEx),
            typeof(查詢姓名檢核失敗_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetNameCheck")]
        public async Task<IResult> GetNameCheck([FromRoute] string applyNo, [FromQuery] GetNameCheckRequest request) =>
            Results.Ok(await _mediator.Send(new Query(applyNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.GetNameCheck
{
    public record Query(string applyNo, GetNameCheckRequest getNameCheckRequest) : IRequest<ResultResponse<string>>;

    public class Handler(
        ScoreSharpContext context,
        IMW3APAPIAdapter mw3Adapter,
        IJWTProfilerHelper jwtProfilerHelper,
        IReviewerHelper reviewerHelper,
        ILogger<Handler> logger
    ) : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var 申請書編號 = request.applyNo;
            var getNameCheckReq = request.getNameCheckRequest;
            var 使用者類型 = getNameCheckReq.UserType;
            var 檢核姓名 = getNameCheckReq.CHName;
            Reviewer_ApplyCreditCardInfoMain? main = new();
            Reviewer_ApplyCreditCardInfoSupplementary? supplementary = new();
            var startTime = DateTime.Now;

            if (使用者類型 == UserType.正卡人)
            {
                main = await context.Reviewer_ApplyCreditCardInfoMain.FirstOrDefaultAsync(x => x.ApplyNo == 申請書編號);
            }
            else
            {
                supplementary = await context.Reviewer_ApplyCreditCardInfoSupplementary.FirstOrDefaultAsync(x => x.ApplyNo == 申請書編號);
            }

            string 身分證 = main?.ID ?? supplementary?.ID;

            if (main == null && supplementary == null)
            {
                return ApiResponseHelper.NotFound<string>($"找不到申請書編號 {申請書編號} 的正卡人或附卡人資料", 申請書編號);
            }

            // 標記變數：記錄錯誤狀態
            bool hasApiError = false;
            string? finalErrorMessage = null;

            // 呼叫 MW3 API
            var mw3Response = await mw3Adapter.QueryNameCheck(檢核姓名, jwtProfilerHelper.UserId, Ulid.NewUlid().ToString());

            // 處理 HTTP 失敗情況
            if (!mw3Response.IsSuccess)
            {
                hasApiError = true;
                finalErrorMessage = mw3Response.ErrorMessage;

                logger.LogError(
                    "查詢姓名檢核失敗 - ApplyNo: {ApplyNo}, UserType: {UserType}, CHName: {CHName}, ErrorMessage: {ErrorMessage}",
                    申請書編號,
                    使用者類型,
                    檢核姓名,
                    mw3Response.ErrorMessage
                );

                // HTTP 失敗：設定 NameChecked = null（可重試）
                if (使用者類型 == UserType.正卡人)
                {
                    main.NameChecked = null;
                }
                else
                {
                    supplementary.NameChecked = null;
                }

                // 記錄流程 (錯誤)
                context.Reviewer_ApplyCreditCardInfoProcess.Add(MapToProcess(申請書編號, startTime, note: ProcessNoteConst.查詢姓名檢核錯誤));
            }
            else
            {
                // 取得業務回應資料
                var nameCheckResponse = mw3Response.Data.Info.Result.Data!;
                var matchResult = nameCheckResponse.MatchedResult;
                var amlReference = nameCheckResponse.AMLReference;
                var referenceNumber = nameCheckResponse.ReferenceNumber;

                // 判斷業務結果
                if (matchResult == MW3RtnCodeConst.查詢姓名檢核_命中 || matchResult == MW3RtnCodeConst.查詢姓名檢核_未命中)
                {
                    // 業務成功：明確得到「命中」或「未命中」
                    if (使用者類型 == UserType.正卡人)
                    {
                        main.NameChecked = matchResult;
                    }
                    else
                    {
                        supplementary.NameChecked = matchResult;
                    }

                    // 新增詳細的檢核日誌
                    context.Reviewer3rd_NameCheckLog.Add(
                        MapToLog(
                            申請書編號,
                            使用者類型,
                            身分證,
                            startTime,
                            matchResult,
                            int.Parse(nameCheckResponse.RCScore),
                            amlReference,
                            referenceNumber,
                            檢核姓名
                        )
                    );

                    // 記錄流程 (成功)
                    context.Reviewer_ApplyCreditCardInfoProcess.Add(MapToProcess(申請書編號, startTime, note: $"({使用者類型}_{身分證})"));
                }
                else
                {
                    // 業務錯誤：收到非預期的 matchResult
                    hasApiError = true;
                    finalErrorMessage = $"查詢姓名檢核回傳非預期結果: {matchResult}";

                    logger.LogWarning(
                        "查詢姓名檢核業務錯誤 - ApplyNo: {ApplyNo}, UserType: {UserType}, CHName: {CHName}, MatchResult: {MatchResult}",
                        申請書編號,
                        使用者類型,
                        檢核姓名,
                        matchResult
                    );

                    // 業務錯誤：設定 NameChecked = null（可重試）
                    if (使用者類型 == UserType.正卡人)
                    {
                        main.NameChecked = null;
                    }
                    else
                    {
                        supplementary.NameChecked = null;
                    }

                    // 記錄流程 (錯誤)
                    context.Reviewer_ApplyCreditCardInfoProcess.Add(MapToProcess(申請書編號, startTime, ProcessNoteConst.查詢姓名檢核錯誤));
                }
            }

            // 必定執行資料庫儲存
            await context.SaveChangesAsync();

            // 使用 reviewerHelper 統一更新 Main 最後異動資訊
            var isUpdateSuccessful = await reviewerHelper.UpdateMainLastModified(申請書編號, jwtProfilerHelper.UserId, startTime) == 1;
            if (!isUpdateSuccessful)
            {
                return ApiResponseHelper.InternalServerError<string>(null, "更新Main最後異動資訊失敗");
            }

            // 根據錯誤狀態回傳結果
            if (hasApiError)
            {
                return ApiResponseHelper.CheckThirdPartyApiError<string>(申請書編號, 申請書編號);
            }

            return ApiResponseHelper.Success<string>(申請書編號, "此申請書查詢姓名檢核成功");
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string note) =>
            new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                ProcessUserId = jwtProfilerHelper.UserId,
                Process = ProcessConst.完成姓名檢核查詢,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
            };

        private Reviewer3rd_NameCheckLog MapToLog(
            string applyNo,
            UserType userType,
            string Id,
            DateTime startTime,
            string result,
            int rcPoint,
            string amlReference,
            string referenceNumber,
            string chName
        ) =>
            new Reviewer3rd_NameCheckLog
            {
                ApplyNo = applyNo,
                UserType = userType,
                ID = Id,
                StartTime = startTime,
                EndTime = DateTime.Now,
                ResponseResult = result,
                RcPoint = rcPoint,
                AMLId = amlReference,
                TraceId = referenceNumber,
                Name = chName,
            };
    }
}
