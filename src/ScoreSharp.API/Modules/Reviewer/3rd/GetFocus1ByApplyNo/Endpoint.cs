using System.Collections.Generic;
using System.Diagnostics;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer3rd.GetFocus1ByApplyNo;
using ScoreSharp.Common.Enums;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 查詢關注名單1 API (查詢案件正附卡人)
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Reviewer3rd/GetFocus1ByApplyNo/20240803B0001
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [OpenApiOperation("GetFocus1ByApplyNo")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢關注名單1成功_2000_ResEx),
            typeof(查詢關注名單1失敗_4003_ResEx),
            typeof(查詢關注名單1失敗_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetFocus1ByApplyNo([FromRoute] string applyNo) => Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.GetFocus1ByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<string>>;

    public class Handler(
        ScoreSharpContext context,
        IMW3MSAPIAdapter mw3Adapter,
        IJWTProfilerHelper jwtProfilerHelper,
        IReviewerHelper reviewerHelper,
        ILogger<Handler> logger
    ) : IRequestHandler<Query, ResultResponse<string>>
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
            bool hasApiError = false;
            string? finalErrorMessage = null;

            var checkConcernDeailTask = checkIDList.Select(async item =>
            {
                var response = await mw3Adapter.QueryConcernDetail(item.ID);
                return new
                {
                    id = item.ID,
                    userType = item.UserType,
                    response,
                };
            });

            var checkConcernDeailResults = await Task.WhenAll(checkConcernDeailTask);

            var concernDetailInfoDtos = new List<ConcernDetailInfoDto>();
            List<Reviewer3rd_WarnCompLog> warningCompanyLogs = new();
            List<Reviewer3rd_RiskAccountLog> riskAccountLogs = new();
            List<Reviewer3rd_FrdIdLog> frdIdLogs = new();
            List<Reviewer_ApplyCreditCardInfoProcess> processes = new();

            foreach (var result in checkConcernDeailResults)
            {
                var id = result.id != null ? result.id.Trim() : string.Empty;
                var userType = result.userType;

                // HTTP 失敗處理
                if (!result.response.IsSuccess)
                {
                    logger.LogError(
                        "查詢關注名單1失敗 - ApplyNo: {ApplyNo}, ID: {ID}, UserType: {UserType}, ErrorMessage: {ErrorMessage}",
                        checkApplyNo,
                        id,
                        userType,
                        result.response.ErrorMessage
                    );

                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢關注名單1失敗";

                    // 使用 DTO 統一處理（失敗情況）
                    ConcernDetailInfoDto concernDetailInfoDto = new()
                    {
                        ApplyNo = checkApplyNo,
                        ID = id,
                        UserType = userType,
                        RtnCode = null, // HTTP 失敗設為 null
                        RtnMsg = null,
                        QueryTime = now, // 仍然記錄查詢時間
                        TraceId = null,
                        // Focus1HitList 保持空的，Focus1Checked 會自動變成 "N"
                    };

                    concernDetailInfoDtos.Add(concernDetailInfoDto);
                    processes.Add(MapToProcess(checkApplyNo, now, note: $"關注名單1查詢失敗({userType}_{id})"));
                    continue;
                }

                var queryConcernDetailResponse = result.response.Data!;
                var code = queryConcernDetailResponse.RtnCode != null ? queryConcernDetailResponse.RtnCode.Trim() : string.Empty;
                var msg = queryConcernDetailResponse.Msg != null ? queryConcernDetailResponse.Msg.Trim() : string.Empty;
                var traceId = queryConcernDetailResponse.TraceId != null ? queryConcernDetailResponse.TraceId.Trim() : string.Empty;

                // Focus1 只看 B、C、H關注名單，其他關注名單失敗沒差
                bool isFocus1Success =
                    code == MW3RtnCodeConst.成功
                    || code == MW3RtnCodeConst.查詢關注名單_查詢A關注名單失敗
                    || code == MW3RtnCodeConst.查詢關注名單_查詢G關注名單失敗;

                if (isFocus1Success)
                {
                    ConcernDetailInfoDto concernDetailInfoDto = new()
                    {
                        ApplyNo = checkApplyNo,
                        ID = id,
                        UserType = userType,
                        RtnCode = code,
                        RtnMsg = msg,
                        QueryTime = now,
                        TraceId = traceId,
                    };

                    var info = queryConcernDetailResponse.Info;

                    if (info.WarningCompany.Count > 0)
                    {
                        // 受警示企業戶之負責人 (B)
                        warningCompanyLogs.AddRange(
                            info.WarningCompany.Select(item => new Reviewer3rd_WarnCompLog
                            {
                                ApplyNo = checkApplyNo,
                                Account = item.Account != null ? item.Account.Trim() : string.Empty,
                                CorporateID = item.CorporateID != null ? item.CorporateID.Trim() : string.Empty,
                                PID = item.PID != null ? item.PID.Trim() : string.Empty,
                                AccountDate = item.AccountDate != null ? item.AccountDate.Trim() : string.Empty,
                                AccidentCode = item.AccidentCode != null ? item.AccidentCode.Trim() : string.Empty,
                                AccidentDate = item.AccidentDate != null ? item.AccidentDate.Trim() : string.Empty,
                                AccidentCancelDate = item.AccidentCancelDate != null ? item.AccidentCancelDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = userType,
                                ID = id,
                            })
                        );

                        concernDetailInfoDto.Focus1HitList.Add("B");
                    }

                    if (info.RiskAccount.Count > 0)
                    {
                        // 風險帳戶 (C)
                        riskAccountLogs.AddRange(
                            info.RiskAccount.Select(item => new Reviewer3rd_RiskAccountLog
                            {
                                ApplyNo = checkApplyNo,
                                Account = item.Account != null ? item.Account.Trim() : string.Empty,
                                PID = item.PID != null ? item.PID.Trim() : string.Empty,
                                AccountDate = item.AccountDate != null ? item.AccountDate.Trim() : string.Empty,
                                AccidentDate = item.AccidentDate != null ? item.AccidentDate.Trim() : string.Empty,
                                AccidentCancelDate = item.AccidentCancelDate != null ? item.AccidentCancelDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = userType,
                                Memo = item.Memo != null ? item.Memo.Trim() : string.Empty,
                                ID = id,
                            })
                        );

                        concernDetailInfoDto.Focus1HitList.Add("C");
                    }

                    if (info.FrdId.Count > 0)
                    {
                        // 疑似涉詐境內帳戶 (H)
                        frdIdLogs.AddRange(
                            info.FrdId.Select(item => new Reviewer3rd_FrdIdLog
                            {
                                ApplyNo = checkApplyNo,
                                Account = item.Account != null ? item.Account.Trim() : string.Empty,
                                ID = item.ID != null ? item.ID.Trim() : string.Empty,
                                NotifyDate = item.NotifyDate != null ? item.NotifyDate.Trim() : string.Empty,
                                CreateDate = item.CreateDate != null ? item.CreateDate.Trim() : string.Empty,
                                UserType = userType,
                            })
                        );

                        concernDetailInfoDto.Focus1HitList.Add("H");
                    }

                    processes.Add(MapToProcess(checkApplyNo, now, note: $"({userType}_{id})"));
                    concernDetailInfoDtos.Add(concernDetailInfoDto);
                }
                else
                {
                    // 業務錯誤情況
                    logger.LogWarning(
                        "查詢關注名單1業務錯誤 - ApplyNo: {ApplyNo}, ID: {ID}, UserType: {UserType}, RtnCode: {RtnCode}, Msg: {Msg}",
                        checkApplyNo,
                        id,
                        userType,
                        code,
                        msg
                    );

                    hasApiError = true;
                    finalErrorMessage = "此申請書編號查詢關注名單1失敗";

                    // 使用 DTO 統一處理（業務錯誤情況）
                    ConcernDetailInfoDto concernDetailInfoDto = new()
                    {
                        ApplyNo = checkApplyNo,
                        ID = id,
                        UserType = userType,
                        RtnCode = code, // 記錄實際的業務錯誤碼
                        RtnMsg = msg,
                        QueryTime = now,
                        TraceId = traceId,
                        // Focus1HitList 保持空的，Focus1Checked 會自動變成 "N"
                    };

                    concernDetailInfoDtos.Add(concernDetailInfoDto);
                    processes.Add(MapToProcess(checkApplyNo, now, note: $"關注名單1查詢失敗({userType}_{id})"));
                }
            }

            // 刪除原有紀錄

            await context.Reviewer3rd_RiskAccountLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_WarnCompLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();
            await context.Reviewer3rd_FrdIdLog.Where(x => x.ApplyNo == checkApplyNo).ExecuteDeleteAsync();

            // 新增detail
            if (warningCompanyLogs.Count > 0)
            {
                context.Reviewer3rd_WarnCompLog.AddRange(warningCompanyLogs);
            }
            if (riskAccountLogs.Count > 0)
            {
                context.Reviewer3rd_RiskAccountLog.AddRange(riskAccountLogs);
            }
            if (frdIdLogs.Count > 0)
            {
                context.Reviewer3rd_FrdIdLog.AddRange(frdIdLogs);
            }

            context.Reviewer_ApplyCreditCardInfoProcess.AddRange(processes);

            // 更新是否命中及命中代號（包含成功和失敗的項目）
            foreach (var concernDetailInfoDto in concernDetailInfoDtos)
            {
                var checkInfo = await context.Reviewer_FinanceCheckInfo.SingleOrDefaultAsync(x =>
                    x.ApplyNo == checkApplyNo && x.UserType == concernDetailInfoDto.UserType && x.ID == concernDetailInfoDto.ID
                );

                if (checkInfo != null)
                {
                    // 判斷是否為 Focus1 成功情況（含允許的部分失敗）
                    var code = concernDetailInfoDto.RtnCode;
                    bool isFocus1Success =
                        code == MW3RtnCodeConst.成功
                        || code == MW3RtnCodeConst.查詢關注名單_查詢A關注名單失敗
                        || code == MW3RtnCodeConst.查詢關注名單_查詢G關注名單失敗;

                    // 成功時設定實際值，失敗時設定 null（可重試）
                    checkInfo.Focus1Check = isFocus1Success ? concernDetailInfoDto.Focus1Checked : null;
                    checkInfo.Focus1_RtnCode = concernDetailInfoDto.RtnCode;
                    checkInfo.Focus1_RtnMsg = concernDetailInfoDto.RtnMsg;
                    checkInfo.Focus1_QueryTime = concernDetailInfoDto.QueryTime;
                    checkInfo.Focus1_TraceId = concernDetailInfoDto.TraceId;
                    checkInfo.Focus1Hit = isFocus1Success ? String.Join("、", concernDetailInfoDto.Focus1HitList) : null;
                }
            }

            await context.SaveChangesAsync();

            var isUpdateSuccessful = await reviewerHelper.UpdateMainLastModified(checkApplyNo, jwtProfilerHelper.UserId, now) == 1;
            if (!isUpdateSuccessful)
            {
                return ApiResponseHelper.InternalServerError<string>(null, "更新Main最後異動資訊失敗");
            }

            // 根據是否有錯誤決定最終回傳
            if (hasApiError)
            {
                return ApiResponseHelper.CheckThirdPartyApiError<string>(checkApplyNo, finalErrorMessage);
            }

            return ApiResponseHelper.Success<string>(checkApplyNo, "此申請書編號查詢關注名單1完畢");
        }

        private async Task<List<QueryConernDetailIDDto>> 取得申請信用卡身分證(string applyNo)
        {
            return await context
                .Reviewer_FinanceCheckInfo.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .Select(x => new QueryConernDetailIDDto { UserType = x.UserType, ID = x.ID })
                .ToListAsync();
        }

        private Reviewer_ApplyCreditCardInfoProcess MapToProcess(string applyNo, DateTime startTime, string? note = null) =>
            new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = applyNo,
                ProcessUserId = jwtProfilerHelper.UserId,
                Process = ProcessConst.完成關注名單1查詢,
                StartTime = startTime,
                EndTime = DateTime.Now,
                Notes = note,
            };
    }
}
