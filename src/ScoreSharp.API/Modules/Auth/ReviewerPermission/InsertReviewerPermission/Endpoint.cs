using ScoreSharp.API.Modules.Auth.ReviewerPermission.InsertReviewerPermission;

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission
{
    public partial class ReviewerPermissionController
    {
        /// <summary>
        /// 新增單筆徵審權限
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回 Key 值</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string?>))]
        [EndpointSpecificExample(typeof(新增單筆徵審權限_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增單筆徵審權限_2000_ResEx),
            typeof(新增單筆徵審權限資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertReviewerPermission")]
        public async Task<IResult> InsertReviewerPermission([FromBody] InsertReviewerPermissionRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.InsertReviewerPermission
{
    public record Command(InsertReviewerPermissionRequest insertReviewerPermissionRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertReviewerPermissionRequest;

            var single = await _context
                .Auth_ReviewerPermission.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CardStatus == dto.CardStatus && x.CardStep == dto.CardStep);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, single.SeqNo.ToString());

            Auth_ReviewerPermission auth_ReviewerPermission = new Auth_ReviewerPermission()
            {
                CardStatus = dto.CardStatus,
                MonthlyIncome_IsShowChangeCaseType = dto.MonthlyIncome_IsShowChangeCaseType,
                MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = dto.MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard,
                MonthlyIncome_IsShowInPermission = dto.MonthlyIncome_IsShowInPermission,
                MonthlyIncome_IsShowMonthlyIncome = dto.MonthlyIncome_IsShowMonthlyIncome,
                IsShowNameCheck = dto.IsShowNameCheck,
                IsShowUpdatePrimaryInfo = dto.IsShowUpdatePrimaryInfo,
                IsShowQueryBranchInfo = dto.IsShowQueryBranchInfo,
                IsShowQuery929 = dto.IsShowQuery929,
                IsShowInsertFileAttachment = dto.IsShowInsertFileAttachment,
                IsShowUpdateApplyNote = dto.IsShowUpdateApplyNote,
                IsCurrentHandleUserId = dto.IsCurrentHandleUserId,
                InsertReviewerSummary = dto.InsertReviewerSummary,
                IsShowFocus1 = dto.IsShowFocus1,
                IsShowFocus2 = dto.IsShowFocus2,
                IsShowWebMobileRequery = dto.IsShowWebMobileRequery,
                IsShowWebEmailRequery = dto.IsShowWebEmailRequery,
                IsShowUpdateReviewerSummary = dto.IsShowUpdateReviewerSummary,
                IsShowDeleteReviewerSummary = dto.IsShowDeleteReviewerSummary,
                IsShowDeleteApplyFileAttachment = dto.IsShowDeleteApplyFileAttachment,
                IsShowCommunicationNotes = dto.IsShowCommunicationNotes,
                CardStep = dto.CardStep,
                ManualReview_IsShowChangeCaseType = dto.ManualReview_IsShowChangeCaseType,
                ManualReview_IsShowInPermission = dto.ManualReview_IsShowInPermission,
                ManualReview_IsShowOutPermission = dto.ManualReview_IsShowOutPermission,
                ManualReview_IsShowReturnReview = dto.ManualReview_IsShowReturnReview,
                IsShowUpdateSameIPCheckRecord = dto.IsShowUpdateSameIPCheckRecord,
                IsShowUpdateWebEmailCheckRecord = dto.IsShowUpdateWebEmailCheckRecord,
                IsShowUpdateWebMobileCheckRecord = dto.IsShowUpdateWebMobileCheckRecord,
                IsShowUpdateInternalIPCheckRecord = dto.IsShowUpdateInternalIPCheckRecord,
                IsShowUpdateShortTimeIDCheckRecord = dto.IsShowUpdateShortTimeIDCheckRecord,
                IsShowInternalEmail = dto.IsShowInternalEmail,
                IsShowInternalMobile = dto.IsShowInternalMobile,
                IsShowUpdateInternalEmailCheckRecord = dto.IsShowUpdateInternalEmailCheckRecord,
                IsShowUpdateInternalMobileCheckRecord = dto.IsShowUpdateInternalMobileCheckRecord,
                IsShowUpdateSupplementaryInfo = dto.IsShowUpdateSupplementaryInfo,
                IsShowKYCSync = dto.IsShowKYCSync,
            };

            await _context.Auth_ReviewerPermission.AddAsync(auth_ReviewerPermission);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(auth_ReviewerPermission.SeqNo.ToString(), auth_ReviewerPermission.SeqNo.ToString());
        }
    }
}
