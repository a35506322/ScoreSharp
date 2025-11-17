using ScoreSharp.API.Modules.Auth.ReviewerPermission.UpdateReviewerPermissionById;

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission
{
    public partial class ReviewerPermissionController
    {
        ///<summary>
        /// 更新單筆徵審權限 By seqNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerPermission/UpdateReviewerPermissionById/2
        ///
        /// </remarks>
        /// <param name="seqNo">PK</param>
        /// <param name="request"></param>
        ///<returns></returns>
        [HttpPut("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新單筆徵審權限_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新單筆徵審權限_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateReviewerPermissionById")]
        public async Task<IResult> UpdateReviewerPermissionById([FromRoute] int seqNo, [FromBody] UpdateReviewerPermissionByIdRequest request) =>
            Results.Ok(await _mediator.Send(new Command(seqNo, request)));
    }
}

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.UpdateReviewerPermissionById
{
    public record Command(int seqNo, UpdateReviewerPermissionByIdRequest updateReviewerPermissionByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var seqNo = request.seqNo;
            var dto = request.updateReviewerPermissionByIdRequest;

            if (seqNo != dto.SeqNo)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await context.Auth_ReviewerPermission.SingleOrDefaultAsync(x => x.SeqNo == seqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, seqNo.ToString());

            entity.MonthlyIncome_IsShowChangeCaseType = dto.MonthlyIncome_IsShowChangeCaseType;
            entity.MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = dto.MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard;
            entity.MonthlyIncome_IsShowInPermission = dto.MonthlyIncome_IsShowInPermission;
            entity.MonthlyIncome_IsShowMonthlyIncome = dto.MonthlyIncome_IsShowMonthlyIncome;
            entity.IsShowNameCheck = dto.IsShowNameCheck;
            entity.IsShowUpdatePrimaryInfo = dto.IsShowUpdatePrimaryInfo;
            entity.IsShowQueryBranchInfo = dto.IsShowQueryBranchInfo;
            entity.IsShowQuery929 = dto.IsShowQuery929;
            entity.IsShowInsertFileAttachment = dto.IsShowInsertFileAttachment;
            entity.IsShowUpdateApplyNote = dto.IsShowUpdateApplyNote;
            entity.IsCurrentHandleUserId = dto.IsCurrentHandleUserId;
            entity.InsertReviewerSummary = dto.InsertReviewerSummary;
            entity.IsShowFocus1 = dto.IsShowFocus1;
            entity.IsShowFocus2 = dto.IsShowFocus2;
            entity.IsShowWebMobileRequery = dto.IsShowWebMobileRequery;
            entity.IsShowWebEmailRequery = dto.IsShowWebEmailRequery;
            entity.IsShowUpdateReviewerSummary = dto.IsShowUpdateReviewerSummary;
            entity.IsShowDeleteReviewerSummary = dto.IsShowDeleteReviewerSummary;
            entity.IsShowDeleteApplyFileAttachment = dto.IsShowDeleteApplyFileAttachment;
            entity.IsShowCommunicationNotes = dto.IsShowCommunicationNotes;
            entity.ManualReview_IsShowInPermission = dto.ManualReview_IsShowInPermission;
            entity.ManualReview_IsShowOutPermission = dto.ManualReview_IsShowOutPermission;
            entity.ManualReview_IsShowReturnReview = dto.ManualReview_IsShowReturnReview;
            entity.ManualReview_IsShowChangeCaseType = dto.ManualReview_IsShowChangeCaseType;
            entity.IsShowUpdateSameIPCheckRecord = dto.IsShowUpdateSameIPCheckRecord;
            entity.IsShowUpdateWebEmailCheckRecord = dto.IsShowUpdateWebEmailCheckRecord;
            entity.IsShowUpdateWebMobileCheckRecord = dto.IsShowUpdateWebMobileCheckRecord;
            entity.IsShowUpdateInternalIPCheckRecord = dto.IsShowUpdateInternalIPCheckRecord;
            entity.IsShowUpdateShortTimeIDCheckRecord = dto.IsShowUpdateShortTimeIDCheckRecord;
            entity.IsShowInternalEmail = dto.IsShowInternalEmail;
            entity.IsShowInternalMobile = dto.IsShowInternalMobile;
            entity.IsShowUpdateInternalEmailCheckRecord = dto.IsShowUpdateInternalEmailCheckRecord;
            entity.IsShowUpdateInternalMobileCheckRecord = dto.IsShowUpdateInternalMobileCheckRecord;
            entity.IsShowUpdateSupplementaryInfo = dto.IsShowUpdateSupplementaryInfo;
            entity.CardStep = dto.CardStep;
            entity.IsShowKYCSync = dto.IsShowKYCSync;

            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.UpdateByIdSuccess(seqNo.ToString(), seqNo.ToString());
        }
    }
}
