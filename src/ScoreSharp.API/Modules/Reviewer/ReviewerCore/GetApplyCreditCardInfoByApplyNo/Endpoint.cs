using System.Data;
using ScoreSharp.API.Modules.Reviewer.Helpers;
using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得單筆申請信用卡資料 By 申請編號
        /// </summary>
        /// <remarks>
        /// 當查詢信用卡資料時，會新增查詢紀錄
        ///
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetApplyCreditCardInfoByApplyNo/20240803B0001
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetApplyCreditCardInfoByApplyNoResponse>))]
        [EndpointSpecificExample(
            typeof(取得單筆申請案件_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyCreditCardInfoByApplyNo")]
        public async Task<IResult> GetApplyCreditCardInfoByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetApplyCreditCardInfoByApplyNoResponse>>;

    public class Handler(
        IScoreSharpDapperContext scoreSharpDapperContext,
        IReviewerHelper reviewerHelper,
        IMapper mapper,
        ScoreSharpContext scoreSharpContext,
        IJWTProfilerHelper jwtProfilerHelper
    ) : IRequestHandler<Query, ResultResponse<GetApplyCreditCardInfoByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetApplyCreditCardInfoByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            /*
                TODO:2025.03.20欄位尚未驗證
                1. 家事法訊息驗證
                3. 身分查驗結果
                6. 原持卡人JCIC補充註記
                7. 長循分期戶
                9. 評分結果
                11. 轉換卡別
            */


            var applyNo = request.applyNo;

            (
                Reviewer_ApplyCreditCardInfoMain main,
                List<Reviewer_ApplyCreditCardInfoHandle> handles,
                List<Reviewer_BankTrace> bankTraceInfos,
                List<Reviewer_FinanceCheckInfo> financeCheckInfos,
                Reviewer_ApplyCreditCardInfoSupplementary supplementary
            ) = await GetApplyCreditCardInfoByApplyNo(applyNo);

            if (main is null)
            {
                return ApiResponseHelper.NotFound<GetApplyCreditCardInfoByApplyNoResponse>(null, applyNo);
            }

            var applyCreditCardInfoParams = await reviewerHelper.GetCreditCardInfoParams();
            var userDic = await scoreSharpContext.OrgSetUp_User.ToDictionaryAsync(x => x.UserId, x => x.UserName);

            // 正卡主要資訊 (天)
            var idCardRenewalLocationDic = applyCreditCardInfoParams.IDCardRenewalLocation.ToDictionary(t => t.Value, t => t.Name);
            var citizenshipDic = applyCreditCardInfoParams.Citizenship.ToDictionary(t => t.Value, t => t.Name);
            var cardDic = applyCreditCardInfoParams.Card.ToDictionary(t => t.Value, t => t.Name);
            var creditCheckCodeDic = applyCreditCardInfoParams.CreditCheckCode.ToDictionary(t => t.Value, t => t.Name);

            var mainInfo = mapper.Map<MainInfo>(main);
            mainInfo.CHName = main.CHName;
            mainInfo.ID = main.ID;
            mainInfo.ApplyDate = main.ApplyDate;
            mainInfo.ApplyNo = main.ApplyNo;
            mainInfo.CardStatusList = handles
                .Select(h => new CardStatusDto
                {
                    CardStatus = h.CardStatus,
                    ID = h.ID,
                    UserType = h.UserType,
                })
                .ToList();

            mainInfo.FamilyMessageCheckedList = financeCheckInfos
                .Select(f => new FamilyMessageCheckedDto
                {
                    ID = f.ID,
                    UserType = f.UserType,
                    FamilyMessageChecked = string.IsNullOrEmpty(f.FamilyMessageChecked) ? String.Empty : f.FamilyMessageChecked,
                })
                .ToList();

            var isRepeatApplyList = new List<IsRepeatApplyDto>
            {
                new IsRepeatApplyDto
                {
                    IsRepeatApply = string.IsNullOrEmpty(main.IsRepeatApply) ? String.Empty : main.IsRepeatApply,
                    ID = main.ID,
                    UserType = UserType.正卡人,
                },
                supplementary is not null
                    ? new IsRepeatApplyDto
                    {
                        IsRepeatApply = string.IsNullOrEmpty(supplementary.IsRepeatApply) ? String.Empty : supplementary.IsRepeatApply,
                        ID = supplementary.ID,
                        UserType = UserType.附卡人,
                    }
                    : null,
            }
                .Where(x => x != null)
                .OrderBy(x => x.UserType);
            mainInfo.IsRepeatApplyList = isRepeatApplyList.ToList();

            var isOriginalCardholderList = new List<IsOriginalCardholderDto>
            {
                new IsOriginalCardholderDto
                {
                    IsOriginalCardholder = string.IsNullOrEmpty(main.IsOriginalCardholder) ? String.Empty : main.IsOriginalCardholder,
                    ID = main.ID,
                    UserType = UserType.正卡人,
                },
                supplementary is not null
                    ? new IsOriginalCardholderDto
                    {
                        IsOriginalCardholder = string.IsNullOrEmpty(supplementary.IsOriginalCardholder)
                            ? String.Empty
                            : supplementary.IsOriginalCardholder,
                        ID = supplementary.ID,
                        UserType = UserType.附卡人,
                    }
                    : null,
            }
                .Where(x => x != null)
                .OrderBy(x => x.UserType);
            mainInfo.IsOriginalCardholderList = isOriginalCardholderList.ToList();

            mainInfo.ApplyCardTypeList = handles
                .Select(h => new ApplyCardTypeDto
                {
                    ApplyCardType = h.ApplyCardType,
                    ApplyCardTypeName = cardDic.GetValueOrDefault(h.ApplyCardType ?? String.Empty),
                    UserType = h.UserType,
                    ID = h.ID,
                })
                .ToList();
            mainInfo.Checked929List = financeCheckInfos
                .Select(f => new Checked929Dto
                {
                    ID = f.ID,
                    UserType = f.UserType,
                    Checked929 = f.Checked929,
                })
                .ToList();
            mainInfo.CustomerSpecialNotes = main.CustomerSpecialNotes;
            mainInfo.IDCheckResultCheckedList = financeCheckInfos
                .Select(f => new IDCheckResultCheckedDto
                {
                    ID = f.ID,
                    UserType = f.UserType,
                    IDCheckResultChecked = f.IDCheckResultChecked ?? String.Empty,
                })
                .ToList();

            mainInfo.Focus1CheckedList = financeCheckInfos
                .Select(f => new Focus1CheckedDto
                {
                    ID = f.ID,
                    UserType = f.UserType,
                    Focus1Hit = f.Focus1Hit,
                    Focus1Checked = f.Focus1Check,
                })
                .ToList()
                .ToList();

            mainInfo.BlackListNote = main.BlackListNote;
            mainInfo.CaseType = main.CaseType;
            mainInfo.OriginCardholderJCICNotesList = handles
                .Select(h => new OriginCardholderJCICNotesDto
                {
                    ID = h.ID,
                    UserType = h.UserType,
                    OriginCardholderJCICNotes = string.IsNullOrEmpty(h.OriginCardholderJCICNotes) ? String.Empty : h.OriginCardholderJCICNotes,
                })
                .ToList();

            mainInfo.Focus2CheckedList = financeCheckInfos
                .Select(f => new Focus2CheckedDto
                {
                    ID = f.ID,
                    UserType = f.UserType,
                    Focus2Hit = f.Focus2Hit,
                    Focus2Checked = f.Focus2Check,
                })
                .ToList();

            // 目前月收入確認正卡人及附卡人簽核都會壓上同一人
            mainInfo.MonthlyIncomeCheckUserId = handles.Select(x => x.MonthlyIncomeCheckUserId).Distinct().FirstOrDefault();
            mainInfo.MonthlyIncomeCheckUserName = !string.IsNullOrEmpty(mainInfo.MonthlyIncomeCheckUserId)
                ? userDic.GetValueOrDefault(mainInfo.MonthlyIncomeCheckUserId ?? String.Empty)
                : String.Empty;

            mainInfo.CardOwner = main.CardOwner;
            mainInfo.LongTerm = main.LongTerm;
            mainInfo.NameCheckedList = new List<NameCheckedDto>();
            if (main != null)
            {
                mainInfo.NameCheckedList.Add(
                    new NameCheckedDto
                    {
                        ID = main.ID,
                        UserType = UserType.正卡人,
                        NameChecked = string.IsNullOrEmpty(main.NameChecked) ? String.Empty : main.NameChecked,
                    }
                );
            }
            if (supplementary != null)
            {
                mainInfo.NameCheckedList.Add(
                    new NameCheckedDto
                    {
                        ID = supplementary.ID,
                        UserType = UserType.附卡人,
                        NameChecked = string.IsNullOrEmpty(supplementary.NameChecked) ? String.Empty : supplementary.NameChecked,
                    }
                );
            }

            mainInfo.ReviewerUserList = handles
                .Select(x => new ReviewerUserDto
                {
                    ID = x.ID,
                    UserType = x.UserType,
                    ReviewerUserId = x.ReviewerUserId,
                    ReviewerUserName = !string.IsNullOrEmpty(x.ReviewerUserId) ? userDic.GetValueOrDefault(x.ReviewerUserId) : String.Empty,
                    ApplyCardType = x.ApplyCardType,
                })
                .ToList();

            mainInfo.CreditLimit_RatingAdviceList = handles
                .Select(x => new CreditLimit_RatingAdviceDto
                {
                    ID = x.ID,
                    UserType = x.UserType,
                    CreditLimit_RatingAdvice = x.CreditLimit_RatingAdvice,
                })
                .ToList();
            mainInfo.IsBranchCustomer = main.IsBranchCustomer;
            mainInfo.AMLRiskLevel = main.AMLRiskLevel;
            mainInfo.ApproveUserList = handles
                .Select(x => new ApproveUserDto
                {
                    ID = x.ID,
                    UserType = x.UserType,
                    ApproveUserId = x.ApproveUserId,
                    ApproveUserName = !string.IsNullOrEmpty(x.ApproveUserId) ? userDic.GetValueOrDefault(x.ApproveUserId) : String.Empty,
                    ApplyCardType = x.ApplyCardType,
                })
                .ToList();
            mainInfo.CustomerServiceNotes = main.CustomerServiceNotes;
            mainInfo.CreditCheckCodeList = handles
                .Select(x => new CreditCheckCodeDto
                {
                    SeqNo = x.SeqNo,
                    CreditCheckCode = x.CreditCheckCode,
                    CreditCheckName = creditCheckCodeDic.GetValueOrDefault(x.CreditCheckCode ?? String.Empty) ?? String.Empty,
                })
                .ToList();

            mainInfo.CurrentHandleUserId = main.CurrentHandleUserId;
            mainInfo.CurrentHandleUserName = !string.IsNullOrEmpty(mainInfo.CurrentHandleUserId)
                ? userDic.GetValueOrDefault(mainInfo.CurrentHandleUserId)
                : String.Empty;
            mainInfo.IsSelf = mainInfo.CurrentHandleUserId == jwtProfilerHelper.UserId ? "Y" : "N";
            mainInfo.PreviousHandleUserId = main.PreviousHandleUserId;
            mainInfo.PreviousHandleUserName = !string.IsNullOrEmpty(mainInfo.PreviousHandleUserId)
                ? userDic.GetValueOrDefault(mainInfo.PreviousHandleUserId)
                : String.Empty;
            mainInfo.CardStep = handles.FirstOrDefault().CardStep;

            // 正卡基本資訊
            var primary_BasicInfo = mapper.Map<Primary_BasicInfo>(main);
            primary_BasicInfo.CitizenshipName = !string.IsNullOrEmpty(main.CitizenshipCode)
                ? citizenshipDic.GetValueOrDefault(main.CitizenshipCode ?? String.Empty)
                : String.Empty;
            primary_BasicInfo.IDCardRenewalLocationName = !string.IsNullOrEmpty(main.IDCardRenewalLocationCode)
                ? idCardRenewalLocationDic.GetValueOrDefault(main.IDCardRenewalLocationCode)
                : String.Empty;
            primary_BasicInfo.NameCheckedReasonCodeList = string.IsNullOrEmpty(main.NameCheckedReasonCodes)
                ? new List<NameCheckdReasonDto>()
                : main
                    .NameCheckedReasonCodes.Split(",")
                    .Select(x => new NameCheckdReasonDto
                    {
                        NameCheckedReasonCode = (NameCheckedReasonCode)Enum.Parse(typeof(NameCheckedReasonCode), x),
                    })
                    .ToList();

            // 正卡職業資料
            var mainIncomeAndFundDic = applyCreditCardInfoParams.MainIncomeAndFund.ToDictionary(t => t.Value, t => t.Name);
            var amlJobLevelDic = applyCreditCardInfoParams.AMLJobLevel.ToDictionary(t => t.Value, t => t.Name);

            Primary_JobInfo primary_JobInfo = mapper.Map<Primary_JobInfo>(main);

            var caseVersion = primary_JobInfo.AMLProfessionCode_Version;
            var AMLProfessionInfo = await scoreSharpContext
                .SetUp_AMLProfession.AsNoTracking()
                .Where(x => x.Version == caseVersion)
                .SingleOrDefaultAsync(x => x.AMLProfessionCode == primary_JobInfo.AMLProfessionCode);

            primary_JobInfo.AMLProfessionName = AMLProfessionInfo?.AMLProfessionName;

            primary_JobInfo.AMLJobLevelName = !string.IsNullOrEmpty(main.AMLJobLevelCode)
                ? amlJobLevelDic.GetValueOrDefault(main.AMLJobLevelCode)
                : null;

            var mainIncomeAndFundNameArrays = !string.IsNullOrEmpty(main.MainIncomeAndFundCodes)
                ? main.MainIncomeAndFundCodes.Split(",").Select(t => mainIncomeAndFundDic.GetValueOrDefault(t)).ToArray()
                : Array.Empty<string>();

            primary_JobInfo.MainIncomeAndFundNames = string.Join(",", mainIncomeAndFundNameArrays);
            // 此為月收入預審時用的徵信代碼
            // TODO:想要問云溪這個可不可以刪除，因為多卡判斷有點麻煩
            var primaryHandle = handles.FirstOrDefault(h => h.UserType == UserType.正卡人);
            primary_JobInfo.CreditCheckCode = primaryHandle?.CreditCheckCode;
            primary_JobInfo.CreditCheckName = creditCheckCodeDic.GetValueOrDefault(primaryHandle?.CreditCheckCode ?? String.Empty);

            // 正卡人學生資料
            Primary_StudentInfo primary_StudentInfo = mapper.Map<Primary_StudentInfo>(main);

            // 正卡人網路資料
            Primary_WebCardInfo primary_WebCardInfo = mapper.Map<Primary_WebCardInfo>(main);
            var primaryBankTraceInfo = bankTraceInfos.SingleOrDefault(d => d.UserType == UserType.正卡人);
            if (primaryBankTraceInfo is not null)
            {
                primary_WebCardInfo.SameIPChecked = primaryBankTraceInfo.SameIP_Flag;
                primary_WebCardInfo.SameWebCaseMobileChecked = primaryBankTraceInfo.SameMobile_Flag;
                primary_WebCardInfo.SameWebCaseEmailChecked = primaryBankTraceInfo.SameEmail_Flag;
                primary_WebCardInfo.IsEqualInternalIP = primaryBankTraceInfo.EqualInternalIP_Flag;
            }

            Primary_BankTraceInfo primary_BankTraceInfo = new();
            if (primaryBankTraceInfo is not null)
            {
                primary_BankTraceInfo.ShortTimeIDChecked = primaryBankTraceInfo.ShortTimeID_Flag;
                primary_BankTraceInfo.InternalEmailSame_Flag = primaryBankTraceInfo.InternalEmailSame_Flag;
                primary_BankTraceInfo.InternalMobileSame_Flag = primaryBankTraceInfo.InternalMobileSame_Flag;
            }

            // 正卡人活動資料
            Primary_ActivityInfo primary_ActivityInfo = mapper.Map<Primary_ActivityInfo>(main);

            var annyalFeePaymentTypeDic = applyCreditCardInfoParams.AnnualFeeCollectionMethod.ToDictionary(t => t.Value, t => t.Name);

            primary_ActivityInfo.AnnualFeePaymentType = main.AnnualFeePaymentType;
            primary_ActivityInfo.AnnualFeePaymentTypeName =
                annyalFeePaymentTypeDic.GetValueOrDefault(main.AnnualFeePaymentType ?? String.Empty) ?? String.Empty;

            // 建構附卡人資料
            Supplementary? supplementaryInfo = null;
            if (supplementary != null)
            {
                supplementaryInfo = mapper.Map<Supplementary>(supplementary);

                // 設定附卡人的名稱欄位
                supplementaryInfo.CitizenshipName = !string.IsNullOrEmpty(supplementary.CitizenshipCode)
                    ? citizenshipDic.GetValueOrDefault(supplementary.CitizenshipCode ?? string.Empty)
                    : string.Empty;

                supplementaryInfo.BirthCitizenshipCodeOtherName = !string.IsNullOrEmpty(supplementary.BirthCitizenshipCodeOther)
                    ? citizenshipDic.GetValueOrDefault(supplementary.BirthCitizenshipCodeOther ?? string.Empty)
                    : string.Empty;

                supplementaryInfo.IDCardRenewalLocationName = !string.IsNullOrEmpty(supplementary.IDCardRenewalLocationCode)
                    ? idCardRenewalLocationDic.GetValueOrDefault(supplementary.IDCardRenewalLocationCode)
                    : string.Empty;

                supplementaryInfo.NameCheckedReasonCodeList = string.IsNullOrEmpty(supplementary.NameCheckedReasonCodes)
                    ? new List<NameCheckdReasonDto>()
                    : supplementary
                        .NameCheckedReasonCodes.Split(",")
                        .Select(x => new NameCheckdReasonDto
                        {
                            NameCheckedReasonCode = (NameCheckedReasonCode)Enum.Parse(typeof(NameCheckedReasonCode), x),
                        })
                        .ToList();
            }

            // 建構KYC加強審核資料
            var mainFinanceCheckInfo = financeCheckInfos.FirstOrDefault(x => x.UserType == UserType.正卡人);
            KYCInfo kycInfo = mapper.Map<KYCInfo>(mainFinanceCheckInfo);
            if (mainFinanceCheckInfo != null)
            {
                CardStatus[] canShowKYCStrongReview =
                {
                    CardStatus.申請核卡中,
                    CardStatus.申請退件中,
                    CardStatus.申請補件中,
                    CardStatus.申請撤件中,
                    CardStatus.退件_等待完成本案徵審,
                    CardStatus.補件_等待完成本案徵審,
                    CardStatus.撤件_等待完成本案徵審,
                    CardStatus.核卡_等待完成本案徵審,
                    CardStatus.補回件,
                    CardStatus.退回重審,
                    CardStatus.申請核卡_等待完成本案徵審,
                    CardStatus.申請退件_等待完成本案徵審,
                    CardStatus.申請補件_等待完成本案徵審,
                    CardStatus.申請撤件_等待完成本案徵審,
                    CardStatus.人工徵信中,
                };

                kycInfo.IsShowKYCStrongReview = handles
                    .Where(x => x.CardStep == CardStep.人工徵審)
                    .Any(x => canShowKYCStrongReview.Contains(x.CardStatus))
                    ? "Y"
                    : "N";
            }

            GetApplyCreditCardInfoByApplyNoResponse response = new(
                mainInfo: mainInfo,
                primary_BasicInfo: primary_BasicInfo,
                primary_JobInfo: primary_JobInfo,
                primary_StudentInfo: primary_StudentInfo,
                primary_WebCardInfo: primary_WebCardInfo,
                primary_ActivityInfo: primary_ActivityInfo,
                primary_BankTraceInfo: primary_BankTraceInfo,
                supplementary: supplementaryInfo,
                kycInfo: kycInfo
            );

            // 新增查閱紀錄
            await AddReviewerLog(applyNo);

            return ApiResponseHelper.Success(response);
        }

        private async Task<(
            Reviewer_ApplyCreditCardInfoMain? main,
            List<Reviewer_ApplyCreditCardInfoHandle> handles,
            List<Reviewer_BankTrace> bankTraceInfos,
            List<Reviewer_FinanceCheckInfo> financeCheckInfos,
            Reviewer_ApplyCreditCardInfoSupplementary? supplementary
        )> GetApplyCreditCardInfoByApplyNo(string applyNo)
        {
            Reviewer_ApplyCreditCardInfoMain main;
            List<Reviewer_ApplyCreditCardInfoHandle> handles;
            List<Reviewer_BankTrace> bankTraceInfos;
            List<Reviewer_FinanceCheckInfo> financeCheckInfos;

            Reviewer_ApplyCreditCardInfoSupplementary supplementary;

            using (var conn = scoreSharpDapperContext.CreateScoreSharpConnection())
            {
                SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                    sql: "Usp_GetApplyCreditCardInfoByApplyNo",
                    param: new { ApplyNo = applyNo },
                    commandType: CommandType.StoredProcedure
                );
                main = results.Read<Reviewer_ApplyCreditCardInfoMain>().ToList().SingleOrDefault();
                handles = results.Read<Reviewer_ApplyCreditCardInfoHandle>().ToList();
                bankTraceInfos = results.Read<Reviewer_BankTrace>().ToList();
                financeCheckInfos = results.Read<Reviewer_FinanceCheckInfo>().ToList();
                supplementary = results.Read<Reviewer_ApplyCreditCardInfoSupplementary>().ToList().SingleOrDefault();
            }

            return (main, handles, bankTraceInfos, financeCheckInfos, supplementary);
        }

        private async Task AddReviewerLog(string applyNo)
        {
            var reviewerLog = new Reviewer_ApplyCreditCardInfoQueryLog
            {
                ApplyNo = applyNo,
                UserId = jwtProfilerHelper.UserId,
                QueryTime = DateTime.Now,
            };

            await scoreSharpContext.Reviewer_ApplyCreditCardInfoQueryLog.AddAsync(reviewerLog);
            await scoreSharpContext.SaveChangesAsync();
        }
    }
}
