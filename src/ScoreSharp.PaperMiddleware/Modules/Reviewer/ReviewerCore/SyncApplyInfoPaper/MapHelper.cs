using ScoreSharp.Common.Extenstions;
using ScoreSharp.Common.Helpers.Address;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoPaper;

public static class MapHelper
{
    /// <summary>
    /// 將 SyncApplyInfoPaperRequest 的全形字元轉換為半形字元
    /// </summary>
    /// <param name="source">原始請求物件</param>
    /// <returns>轉換後的新請求物件</returns>
    public static SyncApplyInfoPaperRequest ToHalfWidthRequest(SyncApplyInfoPaperRequest source)
    {
        return new SyncApplyInfoPaperRequest
        {
            // 基本資訊，不需要轉換
            ApplyNo = source.ApplyNo,
            SyncStatus = source.SyncStatus,
            SyncUserId = source.SyncUserId,
            CardOwner = source.CardOwner,

            // 正卡人基本資料
            M_CHName = source.M_CHName.ToHalfWidth(),
            M_ID = source.M_ID,
            M_IDIssueDate = source.M_IDIssueDate,
            M_IDCardRenewalLocationCode = source.M_IDCardRenewalLocationCode,
            M_IDTakeStatus = source.M_IDTakeStatus,
            M_Sex = source.M_Sex,
            M_MarriageState = source.M_MarriageState,
            M_ChildrenCount = source.M_ChildrenCount,
            M_BirthDay = source.M_BirthDay,
            M_ENName = source.M_ENName.ToHalfWidth(),
            M_BirthCitizenshipCode = source.M_BirthCitizenshipCode,
            M_BirthCitizenshipCodeOther = source.M_BirthCitizenshipCodeOther,
            M_CitizenshipCode = source.M_CitizenshipCode,
            M_Education = source.M_Education,
            M_GraduatedElementarySchool = source.M_GraduatedElementarySchool,

            // 正卡_戶籍地址
            M_Reg_ZipCode = source.M_Reg_ZipCode.ToHalfWidth(),
            M_Reg_City = AddressHelper.將縣市台字轉換為臺字(source.M_Reg_City.ToHalfWidth()),
            M_Reg_District = source.M_Reg_District.ToHalfWidth(),
            M_Reg_Road = source.M_Reg_Road.ToHalfWidth(),
            M_Reg_Lane = source.M_Reg_Lane.ToHalfWidth(),
            M_Reg_Alley = source.M_Reg_Alley.ToHalfWidth(),
            M_Reg_Number = source.M_Reg_Number.ToHalfWidth(),
            M_Reg_SubNumber = source.M_Reg_SubNumber.ToHalfWidth(),
            M_Reg_Floor = source.M_Reg_Floor.ToHalfWidth(),
            M_Reg_Other = source.M_Reg_Other.ToHalfWidth(),

            // 正卡_居住地址
            M_Live_AddressType = source.M_Live_AddressType,
            M_Live_ZipCode = source.M_Live_ZipCode.ToHalfWidth(),
            M_Live_City = AddressHelper.將縣市台字轉換為臺字(source.M_Live_City.ToHalfWidth()),
            M_Live_District = source.M_Live_District.ToHalfWidth(),
            M_Live_Road = source.M_Live_Road.ToHalfWidth(),
            M_Live_Lane = source.M_Live_Lane.ToHalfWidth(),
            M_Live_Alley = source.M_Live_Alley.ToHalfWidth(),
            M_Live_Number = source.M_Live_Number.ToHalfWidth(),
            M_Live_SubNumber = source.M_Live_SubNumber.ToHalfWidth(),
            M_Live_Floor = source.M_Live_Floor.ToHalfWidth(),
            M_Live_Other = source.M_Live_Other.ToHalfWidth(),

            // 正卡_帳單地址
            M_Bill_AddressType = source.M_Bill_AddressType,
            M_Bill_ZipCode = source.M_Bill_ZipCode.ToHalfWidth(),
            M_Bill_City = AddressHelper.將縣市台字轉換為臺字(source.M_Bill_City.ToHalfWidth()),
            M_Bill_District = source.M_Bill_District.ToHalfWidth(),
            M_Bill_Road = source.M_Bill_Road.ToHalfWidth(),
            M_Bill_Lane = source.M_Bill_Lane.ToHalfWidth(),
            M_Bill_Alley = source.M_Bill_Alley.ToHalfWidth(),
            M_Bill_Number = source.M_Bill_Number.ToHalfWidth(),
            M_Bill_SubNumber = source.M_Bill_SubNumber.ToHalfWidth(),
            M_Bill_Floor = source.M_Bill_Floor.ToHalfWidth(),
            M_Bill_Other = source.M_Bill_Other.ToHalfWidth(),

            // 正卡_寄卡地址
            M_SendCard_AddressType = source.M_SendCard_AddressType,
            M_SendCard_ZipCode = source.M_SendCard_ZipCode.ToHalfWidth(),
            M_SendCard_City = AddressHelper.將縣市台字轉換為臺字(source.M_SendCard_City.ToHalfWidth()),
            M_SendCard_District = source.M_SendCard_District.ToHalfWidth(),
            M_SendCard_Road = source.M_SendCard_Road.ToHalfWidth(),
            M_SendCard_Lane = source.M_SendCard_Lane.ToHalfWidth(),
            M_SendCard_Alley = source.M_SendCard_Alley.ToHalfWidth(),
            M_SendCard_Number = source.M_SendCard_Number.ToHalfWidth(),
            M_SendCard_SubNumber = source.M_SendCard_SubNumber.ToHalfWidth(),
            M_SendCard_Floor = source.M_SendCard_Floor.ToHalfWidth(),
            M_SendCard_Other = source.M_SendCard_Other.ToHalfWidth(),

            // 正卡_聯絡資訊
            M_HouseRegPhone = source.M_HouseRegPhone.ToHalfWidth(),
            M_LivePhone = source.M_LivePhone.ToHalfWidth(),
            M_Mobile = source.M_Mobile.ToHalfWidth(),
            M_LiveOwner = source.M_LiveOwner,
            M_LiveYear = source.M_LiveYear,
            M_EMail = source.M_EMail,

            // 正卡_公司資訊
            M_CompName = source.M_CompName.ToHalfWidth(),
            M_CompTrade = source.M_CompTrade,
            M_AMLProfessionCode = source.M_AMLProfessionCode,
            M_AMLProfessionOther = source.M_AMLProfessionOther,
            M_AMLJobLevelCode = source.M_AMLJobLevelCode,
            M_CompJobLevel = source.M_CompJobLevel,

            // 正卡_公司地址
            M_Comp_ZipCode = source.M_Comp_ZipCode.ToHalfWidth(),
            M_Comp_City = AddressHelper.將縣市台字轉換為臺字(source.M_Comp_City.ToHalfWidth()),
            M_Comp_District = source.M_Comp_District.ToHalfWidth(),
            M_Comp_Road = source.M_Comp_Road.ToHalfWidth(),
            M_Comp_Lane = source.M_Comp_Lane.ToHalfWidth(),
            M_Comp_Alley = source.M_Comp_Alley.ToHalfWidth(),
            M_Comp_Number = source.M_Comp_Number.ToHalfWidth(),
            M_Comp_SubNumber = source.M_Comp_SubNumber.ToHalfWidth(),
            M_Comp_Floor = source.M_Comp_Floor.ToHalfWidth(),
            M_Comp_Other = source.M_Comp_Other.ToHalfWidth(),

            // 正卡_工作資訊
            M_CompPhone = source.M_CompPhone.ToHalfWidth(),
            M_CompID = source.M_CompID.ToHalfWidth(),
            M_CompJobTitle = source.M_CompJobTitle,
            M_DepartmentName = source.M_DepartmentName,
            M_CurrentMonthIncome = source.M_CurrentMonthIncome,
            M_EmploymentDate = source.M_EmploymentDate,
            M_CompSeniority = source.M_CompSeniority,

            // 正卡_其他資訊
            M_ReissuedCardType = source.M_ReissuedCardType,
            M_IsAgreeDataOpen = source.M_IsAgreeDataOpen,
            M_MainIncomeAndFundCodes = source.M_MainIncomeAndFundCodes,
            M_MainIncomeAndFundOther = source.M_MainIncomeAndFundOther,
            M_IsAcceptEasyCardDefaultBonus = source.M_IsAcceptEasyCardDefaultBonus,

            // 申請書資訊
            ElecCodeId = source.ElecCodeId,
            M_IsHoldingBankCard = source.M_IsHoldingBankCard,
            FirstBrushingGiftCode = source.FirstBrushingGiftCode,
            AnnualFeePaymentType = source.AnnualFeePaymentType,
            IsAgreeMarketing = source.IsAgreeMarketing,
            BillType = source.BillType,
            ProjectCode = source.ProjectCode,
            PromotionUnit = source.PromotionUnit,
            PromotionUser = source.PromotionUser,
            AcceptType = source.AcceptType,
            AnliNo = source.AnliNo,
            CaseType = source.CaseType,

            // 附卡人資料
            S1_ApplicantRelationship = source.S1_ApplicantRelationship,
            S1_CHName = source.S1_CHName.ToHalfWidth(),
            S1_ID = source.S1_ID,
            S1_IDIssueDate = source.S1_IDIssueDate,
            S1_IDCardRenewalLocationCode = source.S1_IDCardRenewalLocationCode,
            S1_IDTakeStatus = source.S1_IDTakeStatus,
            S1_Sex = source.S1_Sex,
            S1_MarriageState = source.S1_MarriageState,
            S1_BirthDay = source.S1_BirthDay,
            S1_ENName = source.S1_ENName.ToHalfWidth(),
            S1_BirthCitizenshipCode = source.S1_BirthCitizenshipCode,
            S1_BirthCitizenshipCodeOther = source.S1_BirthCitizenshipCodeOther,
            S1_CitizenshipCode = source.S1_CitizenshipCode,

            // 附卡1_居住地址
            S1_Live_AddressType = source.S1_Live_AddressType,
            S1_Live_ZipCode = source.S1_Live_ZipCode.ToHalfWidth(),
            S1_Live_City = AddressHelper.將縣市台字轉換為臺字(source.S1_Live_City.ToHalfWidth()),
            S1_Live_District = source.S1_Live_District.ToHalfWidth(),
            S1_Live_Road = source.S1_Live_Road.ToHalfWidth(),
            S1_Live_Lane = source.S1_Live_Lane.ToHalfWidth(),
            S1_Live_Alley = source.S1_Live_Alley.ToHalfWidth(),
            S1_Live_Number = source.S1_Live_Number.ToHalfWidth(),
            S1_Live_SubNumber = source.S1_Live_SubNumber.ToHalfWidth(),
            S1_Live_Floor = source.S1_Live_Floor.ToHalfWidth(),
            S1_Live_Other = source.S1_Live_Other.ToHalfWidth(),

            // 附卡1_寄卡地址
            S1_SendCard_AddressType = source.S1_SendCard_AddressType,
            S1_SendCard_ZipCode = source.S1_SendCard_ZipCode.ToHalfWidth(),
            S1_SendCard_City = AddressHelper.將縣市台字轉換為臺字(source.S1_SendCard_City.ToHalfWidth()),
            S1_SendCard_District = source.S1_SendCard_District.ToHalfWidth(),
            S1_SendCard_Road = source.S1_SendCard_Road.ToHalfWidth(),
            S1_SendCard_Lane = source.S1_SendCard_Lane.ToHalfWidth(),
            S1_SendCard_Alley = source.S1_SendCard_Alley.ToHalfWidth(),
            S1_SendCard_Number = source.S1_SendCard_Number.ToHalfWidth(),
            S1_SendCard_SubNumber = source.S1_SendCard_SubNumber.ToHalfWidth(),
            S1_SendCard_Floor = source.S1_SendCard_Floor.ToHalfWidth(),
            S1_SendCard_Other = source.S1_SendCard_Other.ToHalfWidth(),

            // 附卡1_聯絡資訊
            S1_LivePhone = source.S1_LivePhone.ToHalfWidth(),
            S1_Mobile = source.S1_Mobile.ToHalfWidth(),
            S1_CompPhone = source.S1_CompPhone.ToHalfWidth(),
            S1_CompName = source.S1_CompName.ToHalfWidth(),
            S1_CompJobTitle = source.S1_CompJobTitle,

            // 卡片資訊和申請歷程 (直接複製，不需要轉換)
            CardInfo = source.CardInfo,
            ApplyProcess = source.ApplyProcess,
        };
    }

    public static List<Reviewer_OutsideBankInfo> MapToOutsideBankInfo(SyncApplyInfoPaperRequest request)
    {
        List<Reviewer_OutsideBankInfo> outsideBankInfos = new()
        {
            new Reviewer_OutsideBankInfo
            {
                ApplyNo = request.ApplyNo,
                ID = request.M_ID,
                UserType = UserType.正卡人,
            },
        };

        if (request.CardOwner != CardOwner.正卡)
        {
            outsideBankInfos.Add(
                new Reviewer_OutsideBankInfo
                {
                    ApplyNo = request.ApplyNo,
                    ID = request.S1_ID,
                    UserType = UserType.附卡人,
                }
            );
        }

        return outsideBankInfos;
    }

    public static List<Reviewer_ApplyNote> MapToNote(SyncApplyInfoPaperRequest request)
    {
        List<Reviewer_ApplyNote> applyNotes = new()
        {
            new Reviewer_ApplyNote
            {
                ApplyNo = request.ApplyNo,
                ID = request.M_ID,
                UserType = UserType.正卡人,
                Note = "",
            },
        };

        if (request.CardOwner != CardOwner.正卡)
        {
            applyNotes.Add(
                new Reviewer_ApplyNote
                {
                    ApplyNo = request.ApplyNo,
                    ID = request.S1_ID,
                    UserType = UserType.附卡人,
                    Note = "",
                }
            );
        }
        return applyNotes;
    }

    public static Reviewer_InternalCommunicate MapToCommunicate(SyncApplyInfoPaperRequest request)
    {
        return new() { ApplyNo = request.ApplyNo };
    }

    public static List<Reviewer_BankTrace> MarpToBankTrace(SyncApplyInfoPaperRequest request)
    {
        List<Reviewer_BankTrace> bankTraces = new()
        {
            new Reviewer_BankTrace
            {
                ApplyNo = request.ApplyNo,
                ID = request.M_ID,
                UserType = UserType.正卡人,
            },
        };

        return bankTraces;
    }

    public static List<Reviewer_FinanceCheckInfo> MapToFinance(SyncApplyInfoPaperRequest request, string kycStrongReVersion)
    {
        List<Reviewer_FinanceCheckInfo> financeCheckInfos = new()
        {
            new Reviewer_FinanceCheckInfo
            {
                ApplyNo = request.ApplyNo,
                ID = request.M_ID,
                UserType = UserType.正卡人,
                KYC_StrongReVersion = kycStrongReVersion,
            },
        };

        if (request.CardOwner != CardOwner.正卡)
        {
            financeCheckInfos.Add(
                new Reviewer_FinanceCheckInfo
                {
                    ApplyNo = request.ApplyNo,
                    ID = request.S1_ID,
                    UserType = UserType.附卡人,
                }
            );
        }
        return financeCheckInfos;
    }

    public static ReviewerPedding_PaperApplyCardCheckJob MapToCheckJob(SyncApplyInfoPaperRequest request)
    {
        return new ReviewerPedding_PaperApplyCardCheckJob()
        {
            ApplyNo = request.ApplyNo,
            AddTime = DateTime.Now,
            IsChecked = CaseCheckedStatus.未完成,
            IsCheck929 = CaseCheckStatus.需檢核_未完成,
            IsCheckFocus = CaseCheckStatus.需檢核_未完成,
            IsCheckInternalEmail =
                !string.IsNullOrEmpty(request.M_EMail) && request.BillType == BillType.電子帳單
                    ? CaseCheckStatus.需檢核_未完成
                    : CaseCheckStatus.不需檢核,
            IsCheckInternalMobile = string.IsNullOrEmpty(request.M_Mobile) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckName = CaseCheckStatus.需檢核_未完成,
            IsCheckShortTimeID = CaseCheckStatus.需檢核_未完成,
            IsQueryBranchInfo = CaseCheckStatus.需檢核_未完成,
            IsQueryOriginalCardholderData = CaseCheckStatus.需檢核_未完成,
            IsCheckRepeatApply = CaseCheckStatus.需檢核_未完成,
        };
    }
}
