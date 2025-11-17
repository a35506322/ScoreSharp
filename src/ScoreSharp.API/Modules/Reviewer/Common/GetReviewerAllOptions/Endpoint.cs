using ScoreSharp.API.Modules.Reviewer.Common.GetReviewerAllOptions;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Common
{
    public partial class ReviewerCommonController
    {
        /// <summary>
        /// 取得徵審相關所有的下拉選單
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /ReviewerCommon/GetReviewerAllOptions?IsActive=Y
        ///
        ///     對照表
        ///     AML職級別 = AMLJobLevel
        ///     AML職業別 = AMLProfession
        ///     卡片種類 = Card
        ///     國籍 = Citizenship
        ///     徵信代碼 = CreditCheckCode
        ///     身分證發證地點 = IDCardRenewalLocation
        ///     主要所得來源 = MainIncomeAndFund
        ///     專案代碼 = ProjectCode
        ///     性別 = Sex
        ///     婚姻狀況 = MarriageState
        ///     學歷 = Education
        ///     居住地所有權人 = LiveOwner
        ///     身分證換發補領狀態 = IDTakeStatus
        ///     出生地國籍 = BirthCitizenshipCode
        ///     學生與申請人關係 = StudentApplicantRelationship
        ///     擔任PEP期間 = PEPRange
        ///     卸任前為何種PEP = ResignPEPKind
        ///     公司職級別 = CompJobLevel
        ///     公司行業別 = CompTrade
        ///     姓名檢核理由碼 = NameCheckedReasonCode
        ///     寄信地址類型 = SendCardAddressType
        ///     帳單地址類型 = BillAddressType
        ///     居住地址類型 = LiveAddressType
        ///     家長居住地址類型 = ParentLiveAddressType
        ///     補件原因 = SupplementReason
        ///     退件原因 = RejectionReason
        ///     月收入確認動作 = IncomeConfirmationAction
        ///     案件狀態 = CardStatus
        ///     補聯繫紀錄_補件類別 = SupplementContactRecordsType
        ///     補聯繫紀錄_聯繫結果 = SupplementContactRecordsResult
        ///     人工徵審權限內_徵審動作 = ManualReviewAction_AuthIn
        ///     人工徵審權限外_徵審動作 = ManualReviewAction_AuthOut
        ///     與正卡人關係 = ApplicantRelationship
        ///     卡片代碼 = Card_BinCode
        ///     年費收取方式 = AnnualFeeCollectionMethod
        ///     確認關係 = SameDataRelation
        /// </remarks>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetReviewerAllOptionsResponse>))]
        [EndpointSpecificExample(
            typeof(取得全部徵審相關下拉選單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetReviewerAllOptions")]
        public async Task<IResult> GetReviewerAllOptions([FromQuery] string? isActive)
        {
            var result = await _mediator.Send(new Query(isActive));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.Common.GetReviewerAllOptions
{
    public record Query(string? isActive) : IRequest<ResultResponse<GetReviewerAllOptionsResponse>>;

    public class Handler(IReviewerHelper reviewerHelper, ScoreSharpContext scoreSharpContext)
        : IRequestHandler<Query, ResultResponse<GetReviewerAllOptionsResponse>>
    {
        public async Task<ResultResponse<GetReviewerAllOptionsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            ApplyCreditCardInfoParamsDto dto = await reviewerHelper.GetCreditCardInfoParams(request.isActive);

            var sexOptions = EnumExtenstions.GetEnumOptions<Sex>(request.isActive);
            var marriageStateOptions = EnumExtenstions.GetEnumOptions<MarriageState>(request.isActive);
            var educationOptions = EnumExtenstions.GetEnumOptions<Education>(request.isActive);
            var liveOwnerOptions = EnumExtenstions.GetEnumOptions<LiveOwner>(request.isActive);
            var idTakeStatusOptions = EnumExtenstions.GetEnumOptions<IDTakeStatus>(request.isActive);
            var birthCitizenshipCodeOptions = EnumExtenstions.GetEnumOptions<BirthCitizenshipCode>(request.isActive);
            var studentApplicantRelationshipOptions = EnumExtenstions.GetEnumOptions<StudentApplicantRelationship>(request.isActive);
            var pepRangeOptions = EnumExtenstions.GetEnumOptions<PEPRange>(request.isActive);
            var resignPEPKindOptions = EnumExtenstions.GetEnumOptions<ResignPEPKind>(request.isActive);
            var compJobLevelOptions = EnumExtenstions.GetEnumOptionsWithNameAttr<CompJobLevel>(request.isActive);
            var compTradeOptions = EnumExtenstions.GetEnumOptionsWithNameAttr<CompTrade>(request.isActive);
            var nameCheckedReasonCodelOptions = EnumExtenstions.GetEnumOptions<NameCheckedReasonCode>(request.isActive);
            var billTypeOptions = EnumExtenstions.GetEnumOptions<BillType>(request.isActive);
            var acceptTypeOptions = EnumExtenstions.GetEnumOptions<AcceptType>(request.isActive);
            var sendCardAddressTypeOptions = EnumExtenstions.GetEnumOptions<SendCardAddressType>(request.isActive);
            var billAddressTypeOptions = EnumExtenstions.GetEnumOptions<BillAddressType>(request.isActive);
            var liveAddressTypeOptions = EnumExtenstions.GetEnumOptions<LiveAddressType>(request.isActive);
            var parentLiveAddressTypeOptions = EnumExtenstions.GetEnumOptions<ParentLiveAddressType>(request.isActive);
            var incomeConfirmationActionOptions = EnumExtenstions.GetEnumOptions<IncomeConfirmationAction>(request.isActive);
            var cardStatusOptions = EnumExtenstions.GetEnumOptions<CardStatus>(request.isActive);
            var supplementContactRecordsTypeOptions = EnumExtenstions.GetEnumOptions<SupplementContactRecordsType>(request.isActive);
            var supplementContactRecordsResultOptions = EnumExtenstions.GetEnumOptions<SupplementContactRecordsResult>(request.isActive);
            var manualReviewActionAuthInOptions = EnumExtenstions.GetEnumOptions<ManualReviewAction_AuthIn>(request.isActive);
            var manualReviewActionAuthOutOptions = EnumExtenstions.GetEnumOptions<ManualReviewAction_AuthOut>(request.isActive);
            var applicantRelationshipOptions = EnumExtenstions.GetEnumOptions<ApplicantRelationship>(request.isActive);
            var shippingCardAddressTypeOptions = EnumExtenstions.GetEnumOptions<ShippingCardAddressType>(request.isActive);
            var residenceTypeOptions = EnumExtenstions.GetEnumOptions<ResidenceType>(request.isActive);
            var caseAssignmentTypeOptions = EnumExtenstions.GetEnumOptions<CaseAssignmentType>(request.isActive);
            var sameDataRelationOptions = EnumExtenstions.GetEnumOptions<SameDataRelation>(request.isActive);

            var groupedProfessionsByVersion = await scoreSharpContext
                .SetUp_AMLProfession.AsNoTracking()
                .GroupBy(x => x.Version)
                .Select(g => new
                {
                    Version = g.Key,
                    ProfessionOptions = g.Select(x => new OptionsDtoTypeString
                        {
                            Name = x.AMLProfessionName,
                            Value = x.AMLProfessionCode,
                            IsActive = x.IsActive,
                        })
                        .ToList(),
                })
                .ToListAsync();

            GetReviewerAllOptionsResponse response = new()
            {
                AMLJobLevel = dto.AMLJobLevel,
                Card = dto.Card,
                Citizenship = dto.Citizenship,
                CreditCheckCode = dto.CreditCheckCode,
                IDCardRenewalLocation = dto.IDCardRenewalLocation,
                MainIncomeAndFund = dto.MainIncomeAndFund,
                ProjectCode = dto.ProjectCode,
                BirthCitizenshipCode = birthCitizenshipCodeOptions,
                Education = educationOptions,
                IDTakeStatus = idTakeStatusOptions.OrderByDescending(x => x.Value).ToList(), // Tips: 20250710 誼娟提醒畫面是顛倒排序
                LiveOwner = liveOwnerOptions,
                MarriageState = marriageStateOptions,
                PEPRange = pepRangeOptions,
                ResignPEPKind = resignPEPKindOptions,
                Sex = sexOptions,
                StudentApplicantRelationship = studentApplicantRelationshipOptions,
                CompJobLevel = compJobLevelOptions,
                CompTrade = compTradeOptions,
                NameCheckedReasonCode = nameCheckedReasonCodelOptions,
                BillType = billTypeOptions,
                AcceptType = acceptTypeOptions,
                SendCardAddressType = sendCardAddressTypeOptions,
                BillAddressType = billAddressTypeOptions,
                LiveAddressType = liveAddressTypeOptions,
                ParentLiveAddressType = parentLiveAddressTypeOptions,
                SupplementReason = dto.SupplementReason,
                RejectionReason = dto.RejectionReason,
                IncomeConfirmationAction = incomeConfirmationActionOptions,
                CardStatus = cardStatusOptions,
                SupplementContactRecordsType = supplementContactRecordsTypeOptions,
                SupplementContactRecordsResult = supplementContactRecordsResultOptions,
                ManualReviewAction_AuthIn = manualReviewActionAuthInOptions,
                ManualReviewAction_AuthOut = manualReviewActionAuthOutOptions,
                ApplicantRelationship = applicantRelationshipOptions,
                ResidenceType = residenceTypeOptions,
                ShippingCardAddressType = shippingCardAddressTypeOptions,
                AMLProfession = groupedProfessionsByVersion
                    .Select(g => new AmlProfessionVersionDto { Version = g.Version, ProfessionOptions = g.ProfessionOptions })
                    .ToList(),
                Card_BinCode = dto.Card_BinCode,
                AnnualFeeCollectionMethod = dto.AnnualFeeCollectionMethod,
                SameDataRelation = sameDataRelationOptions,
            };

            return ApiResponseHelper.Success(response);
        }
    }
}
