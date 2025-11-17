using ScoreSharp.API.Modules.Reviewer.ReviewerCore.SignKYCStrongReview;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 簽署KYC加強審核執行
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(簽核加強審查執行_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(簽核加強審查執行_2000_ResEx),
            typeof(簽核加強審查執行_查無此申請書編號_4001_ResEx),
            typeof(簽核加強審查執行_尚未進行KYC入檔_4003_ResEx),
            typeof(簽核加強審查執行_加強審核中狀態未送審與駁回需由當前經辦處理_4003_ResEx),
            typeof(簽核加強審查執行_加強審核中狀態不允許變更_4003_ResEx),
            typeof(簽核加強審查執行_加強審核中狀態送審中需由當前主管處理_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("SignKYCStrongReview")]
        public async Task<IResult> SignKYCStrongReview([FromBody] SignKYCStrongReviewRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.SignKYCStrongReview
{
    public record Command(SignKYCStrongReviewRequest signKYCStrongReviewRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper, ILogger<Handler> logger)
        : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var req = request.signKYCStrongReviewRequest;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == req.ApplyNo);

            var mainFinanceCheck = await context.Reviewer_FinanceCheckInfo.SingleOrDefaultAsync(x =>
                x.ApplyNo == req.ApplyNo && x.UserType == UserType.正卡人
            );

            if (mainFinanceCheck is null || main is null)
                return ApiResponseHelper.NotFound<string>(null, req.ApplyNo);

            if (!mainFinanceCheck.KYC_StrongReStatus.HasValue)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"案件編號 {req.ApplyNo}，尚未進行KYC入檔");

            if (
                (mainFinanceCheck.KYC_StrongReStatus == KYCStrongReStatus.未送審 || mainFinanceCheck.KYC_StrongReStatus == KYCStrongReStatus.駁回)
                && main.CurrentHandleUserId != jwtProfilerHelper.UserId
            )
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"案件編號 {req.ApplyNo}，加強審核中狀態未送審與駁回，需由當前經辦處理");
            }

            if (mainFinanceCheck.KYC_StrongReStatus == KYCStrongReStatus.送審中 && main.CurrentHandleUserId == jwtProfilerHelper.UserId)
            {
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"案件編號 {req.ApplyNo}，加強審核中狀態送審中，需由當前主管處理");
            }

            if (!ValidateKYCStrongReStatus(mainFinanceCheck.KYC_StrongReStatus.Value, req.KYC_StrongReStatus))
                return ApiResponseHelper.BusinessLogicFailed<string>(
                    null,
                    $"案件編號 {req.ApplyNo}，加強審核中狀態不允許變更為「{req.KYC_StrongReStatus}」"
                );

            mainFinanceCheck.KYC_StrongReStatus = MapKYCStrongReStatus(req.KYC_StrongReStatus);
            mainFinanceCheck.KYC_StrongReDetailJson = req.KYC_StrongReDetailJson;
            var signTime = DateTime.Now;

            if (req.KYC_StrongReStatus == KYCStrongReStatusReq.送審中)
            {
                mainFinanceCheck.KYC_Handler = jwtProfilerHelper.UserId;
                mainFinanceCheck.KYC_Handler_SignTime = signTime;
                mainFinanceCheck.KYC_Suggestion = req.KYC_Suggestion;
            }

            if (req.KYC_StrongReStatus == KYCStrongReStatusReq.核准)
            {
                mainFinanceCheck.KYC_Reviewer = jwtProfilerHelper.UserId;
                mainFinanceCheck.KYC_Reviewer_SignTime = signTime;
                mainFinanceCheck.KYC_KYCManager = jwtProfilerHelper.UserId;
                mainFinanceCheck.KYC_KYCManager_SignTime = signTime;
                mainFinanceCheck.KYC_Suggestion = req.KYC_Suggestion;
            }

            await context.SaveChangesAsync();

            return ApiResponseHelper.Success<string>(req.ApplyNo, $"案件編號{req.ApplyNo}，加強審核簽核完成");
        }

        private KYCStrongReStatus MapKYCStrongReStatus(KYCStrongReStatusReq status)
        {
            return status switch
            {
                KYCStrongReStatusReq.送審中 => KYCStrongReStatus.送審中,
                KYCStrongReStatusReq.核准 => KYCStrongReStatus.核准,
                KYCStrongReStatusReq.駁回 => KYCStrongReStatus.駁回,
                _ => throw new ArgumentException("Invalid status"),
            };
        }

        private bool ValidateKYCStrongReStatus(KYCStrongReStatus currentStatus, KYCStrongReStatusReq reqStatus)
        {
            return (currentStatus, reqStatus) switch
            {
                (KYCStrongReStatus.未送審, KYCStrongReStatusReq.送審中) => true,
                (KYCStrongReStatus.送審中, KYCStrongReStatusReq.駁回) => true,
                (KYCStrongReStatus.送審中, KYCStrongReStatusReq.核准) => true,
                (KYCStrongReStatus.核准, KYCStrongReStatusReq.核准) => true,
                (KYCStrongReStatus.駁回, KYCStrongReStatusReq.送審中) => true,
                (KYCStrongReStatus.駁回, KYCStrongReStatusReq.駁回) => true,
                _ => false,
            };
        }
    }
}
