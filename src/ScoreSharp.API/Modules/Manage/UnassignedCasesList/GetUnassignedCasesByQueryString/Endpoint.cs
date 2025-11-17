using ScoreSharp.API.Modules.Manage.Common.Helpers;
using ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCasesByQueryString;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList
{
    public partial class UnassignedCasesListController
    {
        ///<summary>
        /// 取得待派案案件 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /UnassignedCasesList/GetUnassignedCasesByQueryString?CaseAssignmentType=1&amp;AssignedUserId=arthurlin
        ///
        /// 分案類型:
        ///
        ///     1. 網路件月收入預審_姓名檢核Y清單
        ///     2. 網路件月收入預審_姓名檢核N清單
        ///     3. 網路件人工徵信中
        ///     4. 紙本件月收入預審_姓名檢核Y清單
        ///     5. 紙本件月收入預審_姓名檢核N清單
        ///     6. 紙本件人工徵信中
        ///
        /// </remarks>
        /// <response code="200">
        /// 檢查與資料庫定義值相同
        /// Card (申請卡別) => 關聯 SetUp_Card
        /// </response>
        /// <response code="400">
        /// 分案類型=網路件人工徵信中時，被派案人員為必填
        /// </response>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetUnassignedCasesByQueryStringResponse>>))]
        [EndpointSpecificExample(typeof(取得待派案案件清單_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(取得待派案案件清單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ResultResponse<Dictionary<string, IEnumerable<string>>>))]
        [OpenApiOperation("GetUnassignedCasesByQueryString")]
        public async Task<IResult> GetUnassignedCasesByQueryString([FromQuery] GetUnassignedCasesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList.GetUnassignedCasesByQueryString
{
    public record Query(GetUnassignedCasesByQueryStringRequest getUnassignedCasesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetUnassignedCasesByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IReviewerHelper reviewerHelper, IManageHelper manageHelper)
        : IRequestHandler<Query, ResultResponse<List<GetUnassignedCasesByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetUnassignedCasesByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            CaseAssignmentType? caseAssignmentType = request.getUnassignedCasesByQueryStringRequest.CaseAssignmentType;
            string? assignedUserId = request.getUnassignedCasesByQueryStringRequest.AssignedUserId ?? string.Empty;

            var baseDatas = await reviewerHelper.GetApplyCreditCardBaseData(轉換取得徵審案件資料Dto(caseAssignmentType));

            var isNameCheckedFilter = 是否過濾姓名檢核結果為Y(caseAssignmentType);
            var filterBaseDatas = baseDatas
                .Where(x =>
                    isNameCheckedFilter == string.Empty ? true
                    : isNameCheckedFilter == "Y" ? x.HasNameCheckedY
                    : !x.HasNameCheckedY
                )
                .ToList();

            var stakeholders = await context.Reviewer_Stakeholder.AsNoTracking().Where(x => x.IsActive == "Y").ToListAsync(cancellationToken);
            var stakeholderLookup = stakeholders.ToLookup(x => x.ID, StringComparer.OrdinalIgnoreCase);

            string employeeNo = string.Empty;
            if (!string.IsNullOrEmpty(assignedUserId))
            {
                var user = await context.OrgSetUp_User.AsNoTracking().SingleAsync(x => x.UserId == assignedUserId, cancellationToken);
                employeeNo = user.EmployeeNo ?? string.Empty;
            }

            List<GetUnassignedCasesByQueryStringResponse> responses = filterBaseDatas
                .Select(caseData =>
                {
                    List<NameCheckResultDto> nameCheckResultList =
                    [
                        new NameCheckResultDto { UserType = UserType.正卡人, NameCheckResult = caseData.M_NameChecked },
                    ];
                    List<IsDuplicateSubmissionDto> isDuplicateSubmissionDtoList =
                    [
                        new IsDuplicateSubmissionDto { UserType = UserType.正卡人, IsDuplicateSubmission = caseData.M_IsRepeatApply },
                    ];
                    List<IsOriginalCardholderDto> isOriginalCardholderList =
                    [
                        new IsOriginalCardholderDto { UserType = UserType.正卡人, IsOriginalCardholder = caseData.M_IsOriginalCardholder },
                    ];
                    List<IsNewAccountDto> isNewAccountList =
                    [
                        new IsNewAccountDto { UserType = UserType.正卡人, IsNewAccount = caseData.M_IsOriginalCardholder is "Y" ? "N" : "Y" },
                    ];

                    if (caseData.CardOwner == CardOwner.附卡 || caseData.CardOwner == CardOwner.正卡_附卡)
                    {
                        nameCheckResultList.Add(new NameCheckResultDto { UserType = UserType.附卡人, NameCheckResult = caseData.S1_NameChecked });
                        isDuplicateSubmissionDtoList.Add(
                            new IsDuplicateSubmissionDto { UserType = UserType.附卡人, IsDuplicateSubmission = caseData.S1_IsRepeatApply }
                        );
                        isOriginalCardholderList.Add(
                            new IsOriginalCardholderDto { UserType = UserType.附卡人, IsOriginalCardholder = caseData.S1_IsOriginalCardholder }
                        );
                        isNewAccountList.Add(
                            new IsNewAccountDto { UserType = UserType.附卡人, IsNewAccount = caseData.S1_IsOriginalCardholder is "Y" ? "N" : "Y" }
                        );
                    }

                    List<string> 篩選原因 = [];
                    if (!string.IsNullOrEmpty(assignedUserId))
                    {
                        if (manageHelper.與推廣人員是否相同(caseData.PromotionUser, assignedUserId, employeeNo))
                            篩選原因.Add(FilterReasonConst.推廣員編相同);

                        var idSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { caseData.M_ID };
                        if (!string.IsNullOrEmpty(caseData.S1_ID))
                            idSet.Add(caseData.S1_ID);

                        if (manageHelper.檢核利害關係人(stakeholderLookup, idSet, assignedUserId, employeeNo))
                            篩選原因.Add(FilterReasonConst.利害關係人);

                        if (是否為人工徵信中案件(caseAssignmentType))
                        {
                            var monthlyIncomeCheckUserIds = caseData
                                .ApplyCardList.Select(card => card.MonthlyIncomeCheckUserId)
                                .ToHashSet(StringComparer.OrdinalIgnoreCase);

                            if (manageHelper.與月收入確認人員是否相同(monthlyIncomeCheckUserIds, assignedUserId))
                                篩選原因.Add(FilterReasonConst.月收入確認人員相同);
                        }
                    }

                    return new GetUnassignedCasesByQueryStringResponse
                    {
                        ApplyNo = caseData.ApplyNo,
                        CHName = caseData.M_CHName,
                        NameCheckResultList = nameCheckResultList,
                        ID = caseData.M_ID,
                        ApplyCardTypeList = caseData
                            .ApplyCardList.Select(card => new ApplyCardTypeDto
                            {
                                UserType = card.UserType,
                                ApplyCardType = card.ApplyCardType,
                                ApplyCardTypeName = card.ApplyCardName,
                            })
                            .ToList(),
                        CardStatusList = caseData
                            .ApplyCardList.Select(card => new CardStatusDto { UserType = card.UserType, CardStatus = card.CardStatus })
                            .ToList(),
                        IsDuplicateSubmissionList = isDuplicateSubmissionDtoList,
                        IsOriginalCardholderList = isOriginalCardholderList,
                        IsNewAccountList = isNewAccountList,
                        ApplyDate = caseData.ApplyDate,
                        FilterReason = string.Join("、", 篩選原因),
                        CaseType = caseData.CaseType,
                    };
                })
                .ToList();

            return ApiResponseHelper.Success(responses);
        }

        private string 是否過濾姓名檢核結果為Y(CaseAssignmentType? caseAssignmentType) =>
            caseAssignmentType switch
            {
                CaseAssignmentType.網路件月收入預審_姓名檢核Y清單 => "Y",
                CaseAssignmentType.網路件月收入預審_姓名檢核N清單 => "N",
                CaseAssignmentType.網路件人工徵信中 => string.Empty,
                CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單 => "Y",
                CaseAssignmentType.紙本件月收入預審_姓名檢核N清單 => "N",
                CaseAssignmentType.紙本件人工徵信中 => string.Empty,
                _ => throw new ArgumentException("invalid case assignment type"),
            };

        private bool 是否為人工徵信中案件(CaseAssignmentType? caseAssignmentType) =>
            caseAssignmentType is CaseAssignmentType.網路件人工徵信中 or CaseAssignmentType.紙本件人工徵信中;

        private GetApplyCreditCardBaseDataDto 轉換取得徵審案件資料Dto(CaseAssignmentType? caseAssignmentType) =>
            caseAssignmentType switch
            {
                CaseAssignmentType.網路件月收入預審_姓名檢核Y清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審],
                    CurrentHandleUserId = "<NULL>",
                },
                CaseAssignmentType.網路件月收入預審_姓名檢核N清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.網路件_待月收入預審],
                    CurrentHandleUserId = "<NULL>",
                },
                CaseAssignmentType.網路件人工徵信中 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.人工徵信中],
                    Source = [Source.ECARD, Source.APP],
                    CurrentHandleUserId = "<NULL>",
                },
                CaseAssignmentType.紙本件月收入預審_姓名檢核Y清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.紙本件_待月收入預審],
                    CurrentHandleUserId = "<NULL>",
                },
                CaseAssignmentType.紙本件月收入預審_姓名檢核N清單 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.紙本件_待月收入預審],
                    CurrentHandleUserId = "<NULL>",
                },
                CaseAssignmentType.紙本件人工徵信中 => new GetApplyCreditCardBaseDataDto
                {
                    CardStatus = [CardStatus.人工徵信中],
                    Source = [Source.紙本],
                    CurrentHandleUserId = "<NULL>",
                },
                _ => throw new ArgumentException($"Invalid CaseAssignmentType: {caseAssignmentType}"),
            };
    }
}
