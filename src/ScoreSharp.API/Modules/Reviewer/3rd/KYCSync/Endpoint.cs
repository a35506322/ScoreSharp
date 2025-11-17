using ScoreSharp.API.Modules.Reviewer3rd.KYCSync;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Helpers.KYC;

namespace ScoreSharp.API.Modules.Reviewer3rd
{
    public partial class Reviewer3rdController
    {
        /// <summary>
        /// 入檔KYC
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [OpenApiOperation("KYCSync")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(入檔KYC成功_2000_ResEx),
            typeof(查無資料_4001_ResEx),
            typeof(系統維護_4003_ResEx),
            typeof(發查第三方API失敗_5002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> KYCSync(KYCSyncRequest request) => Results.Ok(await _mediator.Send(new Command(request)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer3rd.KYCSync
{
    public record Command(KYCSyncRequest KYCSyncRequest) : IRequest<ResultResponse<KYCSyncResponse>>;

    public class Handler(ScoreSharpContext context, IMW3APAPIAdapter mw3Adapter, IJWTProfilerHelper _jwtHelper)
        : IRequestHandler<Command, ResultResponse<KYCSyncResponse>>
    {
        public async Task<ResultResponse<KYCSyncResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            var applyNo = request.KYCSyncRequest.ApplyNo;
            DateTime queryTime = DateTime.Now;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.SingleOrDefaultAsync(x => x.ApplyNo == applyNo, cancellationToken);
            var mainFinanceCheck = await context.Reviewer_FinanceCheckInfo.SingleOrDefaultAsync(x => x.ApplyNo == main.ApplyNo && x.ID == main.ID);

            if (main is null || mainFinanceCheck is null)
                return ApiResponseHelper.NotFound<KYCSyncResponse>(null, applyNo);

            var handles = await context.Reviewer_ApplyCreditCardInfoHandle.AsNoTracking().Where(x => x.ApplyNo == applyNo).ToListAsync();

            var sysParam = await context.SysParamManage_SysParam.AsNoTracking().SingleAsync();
            if (
                sysParam.KYCFixStartTime.HasValue
                && sysParam.KYCFixEndTime.HasValue
                && sysParam.KYCFixStartTime <= queryTime
                && queryTime <= sysParam.KYCFixEndTime
            )
            {
                return ApiResponseHelper.BusinessLogicFailed<KYCSyncResponse>(null, "入檔KYC系統維護中，請稍後再試。");
            }

            if (main.IsOriginalCardholder == "Y" && main.Source == Source.紙本)
            {
                // 紙本卡友
                var kycRequest = MapHelper.MapTo紙本件原卡友QueryCustKYCRiskLevelRequestInfo(main.ID);
                var kycResponse = await mw3Adapter.QueryCustKYCRiskLevel(kycRequest);
                var kycResult = kycResponse.Data.Info.Result.Data;

                Reviewer3rd_KYCQueryLog kycQueryLog = MapHelper.MapKYCQueryLog(
                    applyNo: main.ApplyNo,
                    cardStatus: string.Join("/", handles.Select(x => x.CardStatus.ToName())),
                    id: main.ID,
                    request: kycRequest,
                    response: kycResponse,
                    kycCode: kycResult.KYC,
                    kycRank: kycResult.Data.RARank,
                    kycMsg: kycResult.KycMsg,
                    queryTime: queryTime,
                    querySuccess: kycResponse.IsSuccess,
                    userId: _jwtHelper.UserId,
                    apiName: "KYC00QNBDRA"
                );

                Reviewer_ApplyCreditCardInfoProcess process = MapHelper.MapKYCProcess(
                    applyNo: main.ApplyNo,
                    process: ProcessConst.完成KYC入檔,
                    startTime: queryTime,
                    endTime: DateTime.Now,
                    isSuccess: kycResponse.IsSuccess ? "Y" : "N",
                    kycCode: kycResult.KYC,
                    kycRank: kycResult.Data.RARank,
                    id: main.ID,
                    userType: main.UserType,
                    kycMsg: kycResult.KycMsg,
                    userId: _jwtHelper.UserId,
                    apiCHName: "簡單查詢風險等級"
                );

                await context.Reviewer3rd_KYCQueryLog.AddAsync(kycQueryLog, cancellationToken);
                await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process, cancellationToken);

                main.AMLRiskLevel = kycResult.Data.RARank;
                mainFinanceCheck.AMLRiskLevel = kycResult.Data.RARank;
                mainFinanceCheck.KYC_Message = kycResult.KycMsg;
                mainFinanceCheck.KYC_RtnCode = kycResult.KYC;
                mainFinanceCheck.KYC_QueryTime = queryTime;
                mainFinanceCheck.KYC_Handler = null;
                mainFinanceCheck.KYC_Handler_SignTime = null;
                mainFinanceCheck.KYC_Reviewer = null;
                mainFinanceCheck.KYC_Reviewer_SignTime = null;
                mainFinanceCheck.KYC_KYCManager = null;
                mainFinanceCheck.KYC_KYCManager_SignTime = null;
                mainFinanceCheck.KYC_StrongReDetailJson = null;
                mainFinanceCheck.KYC_Suggestion = null;
                mainFinanceCheck.KYC_StrongReStatus = null;

                if (kycResponse.IsSuccess && kycResult.KYC == MW3RtnCodeConst.簡單查詢風險等級_成功)
                {
                    // Tips: 原卡友卡友不需加強審核
                    mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
                }

                // 更新UpdateUserId、UpdateTime
                main.LastUpdateTime = queryTime;
                main.LastUpdateUserId = _jwtHelper.UserId;

                await context.SaveChangesAsync();

                KYCSyncResponse response = new()
                {
                    ApplyNo = main.ApplyNo,
                    KYC_RtnCode = kycResult.KYC,
                    KYC_Message = kycResult.KycMsg,
                    KYC_RiskLevel = kycResult.Data.RARank,
                    KYC_QueryTime = queryTime,
                    KYC_Exception = kycResponse.ErrorMessage,
                };

                if (kycResponse.IsSuccess && kycResult.KYC == MW3RtnCodeConst.簡單查詢風險等級_成功)
                    return ApiResponseHelper.Success(response, "簡單查詢風險等級成功");
                else
                    return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(response, "簡單查詢風險等級失敗");
            }
            else
            {
                SyncKycMW3Info syncKycRequest = new();

                if (main.IsOriginalCardholder == "Y" && main.Source != Source.紙本)
                {
                    // 網路卡友
                    syncKycRequest = MapHelper.MapTo卡友SyncKycMW3Info(main);
                }
                else if (main.IsOriginalCardholder == "N")
                {
                    // 非卡友
                    var nameCheck = await context
                        .Reviewer3rd_NameCheckLog.AsNoTracking()
                        .OrderByDescending(x => x.StartTime)
                        .FirstOrDefaultAsync(x => x.ApplyNo == main.ApplyNo && x.ID == main.ID);

                    if (nameCheck is null)
                        return ApiResponseHelper.BusinessLogicFailed<KYCSyncResponse>(null, $"非卡友案件需有姓名檢核紀錄，請重新執行");

                    // AMLRiskLevel 有值 => 修改非卡友
                    // AMLRiskLevel 沒有值 => 新增非卡友
                    bool 是否押上強押記號 = string.IsNullOrWhiteSpace(main.AMLRiskLevel) ? false : true;

                    syncKycRequest = MapHelper.MapTo非卡友SyncKycMW3Info(是否加上強押記號: 是否押上強押記號, main: main, nameCheckLog: nameCheck);
                }

                // 呼叫MW3入檔
                var mw3Response = await mw3Adapter.SyncKYC(syncKycRequest);
                var kycResult = mw3Response.Data.Info.Result.Data;

                Reviewer3rd_KYCQueryLog kycQueryLog = MapHelper.MapKYCQueryLog(
                    applyNo: main.ApplyNo,
                    cardStatus: string.Join("/", handles.Select(x => x.CardStatus.ToName())),
                    id: main.ID,
                    request: syncKycRequest,
                    response: mw3Response,
                    kycCode: kycResult.KycCode,
                    kycRank: kycResult.RaRank,
                    kycMsg: kycResult.ErrMsg,
                    queryTime: queryTime,
                    querySuccess: mw3Response.IsSuccess,
                    userId: _jwtHelper.UserId,
                    apiName: "KYC00CREDIT"
                );

                Reviewer_ApplyCreditCardInfoProcess process = MapHelper.MapKYCProcess(
                    applyNo: main.ApplyNo,
                    process: ProcessConst.完成KYC入檔,
                    startTime: queryTime,
                    endTime: DateTime.Now,
                    isSuccess: kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功 ? "Y" : "N",
                    kycCode: kycResult.KycCode,
                    kycRank: kycResult.RaRank,
                    id: main.ID,
                    userType: main.UserType,
                    kycMsg: kycResult.ErrMsg,
                    userId: _jwtHelper.UserId,
                    apiCHName: "KYC入檔"
                );

                await context.Reviewer3rd_KYCQueryLog.AddAsync(kycQueryLog, cancellationToken);
                await context.Reviewer_ApplyCreditCardInfoProcess.AddAsync(process, cancellationToken);

                var originAMLRiskLevel = mainFinanceCheck.AMLRiskLevel;
                main.AMLRiskLevel = kycResult.RaRank;
                mainFinanceCheck.AMLRiskLevel = kycResult.RaRank;
                mainFinanceCheck.KYC_Message = kycResult.ErrMsg;
                mainFinanceCheck.KYC_RtnCode = kycResult.KycCode;
                mainFinanceCheck.KYC_QueryTime = queryTime;
                mainFinanceCheck.KYC_Handler = null;
                mainFinanceCheck.KYC_Handler_SignTime = null;
                mainFinanceCheck.KYC_Reviewer = null;
                mainFinanceCheck.KYC_Reviewer_SignTime = null;
                mainFinanceCheck.KYC_KYCManager = null;
                mainFinanceCheck.KYC_KYCManager_SignTime = null;
                mainFinanceCheck.KYC_StrongReDetailJson = null;
                mainFinanceCheck.KYC_Suggestion = null;
                mainFinanceCheck.KYC_StrongReStatus = null;

                if (mw3Response.IsSuccess && kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
                {
                    if (main.IsOriginalCardholder == "Y")
                    {
                        mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
                    }
                    else
                    {
                        if (kycResult.RaRank is not null && originAMLRiskLevel != kycResult.RaRank)
                        {
                            // Tips: KYC 風險等級為 H 或 M 時，需要加強審核執行
                            if (kycResult.RaRank == KYCRiskLevelConst.高風險 || kycResult.RaRank == KYCRiskLevelConst.中風險)
                            {
                                mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.未送審;
                                mainFinanceCheck.KYC_StrongReDetailJson = JsonHelper.序列化物件(
                                    KYCHelper.產生KYC加強審核執行表(
                                        version: mainFinanceCheck.KYC_StrongReVersion,
                                        id: main.ID,
                                        name: main.CHName,
                                        riskLevel: kycResult.RaRank
                                    )
                                );
                            }
                            else
                            {
                                mainFinanceCheck.KYC_StrongReStatus = KYCStrongReStatus.不需檢核;
                            }
                        }
                    }
                }

                // 更新UpdateUserId、UpdateTime
                main.LastUpdateTime = queryTime;
                main.LastUpdateUserId = _jwtHelper.UserId;

                await context.SaveChangesAsync();

                KYCSyncResponse response = new()
                {
                    ApplyNo = main.ApplyNo,
                    KYC_RtnCode = kycResult.KycCode,
                    KYC_Message = kycResult.ErrMsg,
                    KYC_RiskLevel = kycResult.RaRank,
                    KYC_QueryTime = queryTime,
                    KYC_Exception = mw3Response.ErrorMessage,
                };

                if (mw3Response.IsSuccess && kycResult.KycCode == MW3RtnCodeConst.入檔KYC_成功)
                    return ApiResponseHelper.Success(response, "入檔KYC成功");
                else
                    return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(response, "入檔KYC失敗");
            }
        }
    }
}
