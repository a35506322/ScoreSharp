using ScoreSharp.API.Modules.Auth.ReviewerPermission.GetApplyPermissionById;

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission
{
    public partial class ReviewerPermissionController
    {
        ///<summary>
        /// 查詢單筆申請書權限 By applyNo
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerPermission/GetApplyPermissionById/20250123X0003
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyPermissionByIdResponse>))]
        [EndpointSpecificExample(
            typeof(查詢單筆申請書權限_成功查詢_唯讀_2000_ResEx),
            typeof(查詢單筆申請書權限_成功查詢_登入角色為當前經辦本人_2000_ResEx),
            typeof(查詢單筆申請書權限查無此ID_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyPermissionById")]
        public async Task<IResult> GetApplyPermissionById([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Auth.ReviewerPermission.GetApplyPermissionById
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetApplyPermissionByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper, IJWTProfilerHelper jwtHelper, ILogger<Handler> logger)
        : IRequestHandler<Query, ResultResponse<GetApplyPermissionByIdResponse>>
    {
        public async Task<ResultResponse<GetApplyPermissionByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyNo = request.applyNo;

            var main = await context.Reviewer_ApplyCreditCardInfoMain.AsNoTracking().SingleOrDefaultAsync(x => x.ApplyNo == applyNo);

            var handleList = await context
                .Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
                .Where(x => x.ApplyNo == applyNo)
                .Select(x => new { cardStatus = x.CardStatus, cardStep = x.CardStep })
                .ToListAsync();

            if (main == null)
                return ApiResponseHelper.NotFound<GetApplyPermissionByIdResponse>(null, applyNo);

            var currentHandlerUserId = main.CurrentHandleUserId;

            var defaultApplyPermission = new GetApplyPermissionByIdResponse()
            {
                MonthlyIncome_IsShowChangeCaseType = "N",
                MonthlyIncome_IsShowChangeCardTypeOnlyNationalTourismCard = "N",
                MonthlyIncome_IsShowInPermission = "N",
                MonthlyIncome_IsShowMonthlyIncome = "N",
                IsShowNameCheck = "N",
                IsShowUpdatePrimaryInfo = "N",
                IsShowQueryBranchInfo = "N",
                IsShowQuery929 = "N",
                IsShowInsertFileAttachment = "N",
                IsShowUpdateApplyNote = "N",
                InsertReviewerSummary = "N",
                IsShowFocus1 = "N",
                IsShowFocus2 = "N",
                IsShowWebMobileRequery = "N",
                IsShowWebEmailRequery = "N",
                IsShowUpdateReviewerSummary = "N",
                IsShowDeleteReviewerSummary = "N",
                IsShowDeleteApplyFileAttachment = "N",
                IsShowCommunicationNotes = "N",
                ManualReview_IsShowInPermission = "N",
                ManualReview_IsShowOutPermission = "N",
                ManualReview_IsShowReturnReview = "N",
                ManualReview_IsShowChangeCaseType = "N",
                IsShowUpdateSameIPCheckRecord = "Y",
                IsShowUpdateWebEmailCheckRecord = "Y",
                IsShowUpdateWebMobileCheckRecord = "Y",
                IsShowUpdateInternalIPCheckRecord = "Y",
                IsShowUpdateShortTimeIDCheckRecord = "Y",
                IsShowInternalEmail = "N",
                IsShowInternalMobile = "N",
                IsShowUpdateInternalEmailCheckRecord = "Y",
                IsShowUpdateInternalMobileCheckRecord = "Y",
                IsShowUpdateSupplementaryInfo = "N",
                IsShowKYCSync = "N",
            };

            try
            {
                var handleCardStatuses = handleList.Select(x => x.cardStatus).ToList();

                var authReviewerPermissions = await context
                    .Auth_ReviewerPermission.AsNoTracking()
                    .Where(x => handleCardStatuses.Contains(x.CardStatus))
                    .ToListAsync();

                // 如果沒有任何權限，直接回傳 defaultResponse
                if (authReviewerPermissions.Count == 0)
                {
                    return ApiResponseHelper.Success(defaultApplyPermission);
                }

                List<GetApplyPermissionByIdResponse> collectedPermissions = new();

                // 逐項狀態檢查徵審權限
                foreach (var handle in handleList)
                {
                    var matchingPermissions = authReviewerPermissions.Where(x => x.CardStatus == handle.cardStatus).ToList();

                    // 查無此狀態於 Auth_ReviewerPermission 直接回傳 defaultResponse
                    if (!matchingPermissions.Any())
                    {
                        collectedPermissions.Add(defaultApplyPermission);
                        continue;
                    }

                    var reviewerPermission = new Auth_ReviewerPermission();

                    // 有兩個狀態代表 cardStep 有值，主因某些狀態在各階段是重複地用於方便辨別
                    if (matchingPermissions.Count > 1)
                    {
                        reviewerPermission = matchingPermissions.SingleOrDefault(x => x.CardStep == handle.cardStep);
                    }
                    else
                    {
                        reviewerPermission = matchingPermissions.FirstOrDefault();
                    }

                    bool isNeedToCheckCurrentHandler = reviewerPermission.IsCurrentHandleUserId == "Y";

                    if (isNeedToCheckCurrentHandler && jwtHelper.UserId != currentHandlerUserId)
                    {
                        collectedPermissions.Add(defaultApplyPermission);
                    }
                    else
                    {
                        var applyCasePermissionResponse = mapper.Map<GetApplyPermissionByIdResponse>(reviewerPermission);
                        collectedPermissions.Add(applyCasePermissionResponse);
                    }
                }

                // 這邊的 permissions 會有多筆的資料，利用逐項檢查Y最大
                var finalPermissionResponse = new GetApplyPermissionByIdResponse();

                var propertyInfos = typeof(GetApplyPermissionByIdResponse).GetProperties().Where(p => p.PropertyType == typeof(string)); // 只處理字符串類型屬性

                foreach (var property in propertyInfos)
                {
                    // 檢查是否有任何一個權限具有該屬性值為"Y"
                    var isPermitted = collectedPermissions.Any(permission =>
                    {
                        var value = property.GetValue(permission) as string;
                        return value == "Y";
                    });

                    // 設置最終權限值
                    property.SetValue(finalPermissionResponse, isPermitted ? "Y" : "N");
                }

                return ApiResponseHelper.Success(finalPermissionResponse);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "查詢申請書權限時發生錯誤，申請書編號: {ApplyNo}", applyNo);
                return ApiResponseHelper.Success(defaultApplyPermission);
            }
        }
    }
}
