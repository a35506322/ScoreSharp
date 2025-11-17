using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.Helpers;

public static class ReviewerMapHelper
{
    public static CaseInfoContext MapToCaseInfoContext(Reviewer_ApplyCreditCardInfoMain main)
    {
        return new CaseInfoContext
        {
            CardOwner = main.CardOwner.Value!,
            Source = main.Source,
            M_BillType = main.BillType,
        };
    }

    public static HandleInfoContext MapToHandleInfoContext(
        Reviewer_ApplyCreditCardInfoHandle handle,
        ReviewAction recentAction,
        Dictionary<string, string> cardDict
    )
    {
        return new HandleInfoContext
        {
            HandleSeqNo = handle.SeqNo,
            UserType = handle.UserType,
            RecentAction = recentAction,
            CardStep = handle.CardStep,
            ApplyCardType = handle.ApplyCardType,
            IsCITSCard = cardDict.TryGetValue(handle.ApplyCardType, out var isCITSCard) ? isCITSCard : "N",
        };
    }

    public static ReviewerMainDataContext MapToReviewerMainDataContext(
        Reviewer_ApplyCreditCardInfoMain main,
        IReadOnlyCollection<HandleInfoContext> handles
    )
    {
        return new ReviewerMainDataContext
        {
            ID = main.ID,
            CHName = main.CHName,
            Email = main.EMail,
            Mobile = main.Mobile,
            BirthDay = main.BirthDay,
            IDIssueDate = main.IDIssueDate,
            CurrentMonthIncome = main.CurrentMonthIncome,
            ResidencePermitIssueDate = main.ResidencePermitIssueDate,
            ResidencePermitDeadline = main.ResidencePermitDeadline,
            ExpatValidityPeriod = main.ExpatValidityPeriod,
            IsForeverResidencePermit = main.IsForeverResidencePermit,
            ResidencePermitBackendNum = main.ResidencePermitBackendNum,
            OldCertificateVerified = main.OldCertificateVerified,
            Handles = handles,
        };
    }

    public static ReviewerSupplementaryDataContext MapToReviewerSupplementaryDataContext(
        Reviewer_ApplyCreditCardInfoSupplementary supplementary,
        IReadOnlyCollection<HandleInfoContext> handles
    )
    {
        return new ReviewerSupplementaryDataContext
        {
            ID = supplementary.ID,
            CHName = supplementary.CHName,
            BirthDay = supplementary.BirthDay,
            IDIssueDate = supplementary.IDIssueDate,
            ResidencePermitIssueDate = supplementary.ResidencePermitIssueDate,
            ResidencePermitDeadline = supplementary.ResidencePermitDeadline,
            ExpatValidityPeriod = supplementary.ExpatValidityPeriod,
            IsForeverResidencePermit = supplementary.IsForeverResidencePermit,
            ResidencePermitBackendNum = supplementary.ResidencePermitBackendNum,
            OldCertificateVerified = supplementary.OldCertificateVerified,
            Handles = handles,
        };
    }

    public static ReviewerMainBankTraceContext MapToReviewerMainBankTraceContext(Reviewer_BankTrace bankTarce, string? chName = null)
    {
        return new ReviewerMainBankTraceContext
        {
            ID = bankTarce.ID,
            CHName = chName,
            EqualInternalIP_Flag = bankTarce.EqualInternalIP_Flag,
            EqualInternalIP_CheckRecord = bankTarce.EqualInternalIP_CheckRecord,
            EqualInternalIP_IsError = bankTarce.EqualInternalIP_IsError,
            SameIP_Flag = bankTarce.SameIP_Flag,
            SameIP_CheckRecord = bankTarce.SameIP_CheckRecord,
            SameIP_IsError = bankTarce.SameIP_IsError,
            SameEmail_Flag = bankTarce.SameEmail_Flag,
            SameEmail_CheckRecord = bankTarce.SameEmail_CheckRecord,
            SameEmail_IsError = bankTarce.SameEmail_IsError,
            SameMobile_Flag = bankTarce.SameMobile_Flag,
            SameMobile_CheckRecord = bankTarce.SameMobile_CheckRecord,
            SameMobile_IsError = bankTarce.SameMobile_IsError,
            ShortTimeID_Flag = bankTarce.ShortTimeID_Flag,
            ShortTimeID_CheckRecord = bankTarce.ShortTimeID_CheckRecord,
            ShortTimeID_IsError = bankTarce.ShortTimeID_IsError,
            InternalEmailSame_Flag = bankTarce.InternalEmailSame_Flag,
            InternalEmailSame_CheckRecord = bankTarce.InternalEmailSame_CheckRecord,
            InternalEmailSame_IsError = bankTarce.InternalEmailSame_IsError,
            InternalMobileSame_Flag = bankTarce.InternalMobileSame_Flag,
            InternalMobileSame_CheckRecord = bankTarce.InternalMobileSame_CheckRecord,
            InternalMobileSame_IsError = bankTarce.InternalMobileSame_IsError,
        };
    }

    public static ReviewerFinanceCheckMainContext MapToReviewerFinanceCheckMainContext(
        Reviewer_FinanceCheckInfo financeCheck,
        string mainNameChecked,
        string? chName = null
    )
    {
        return new ReviewerFinanceCheckMainContext
        {
            ID = financeCheck.ID,
            UserType = financeCheck.UserType,
            CHName = chName,
            Checked929 = financeCheck.Checked929,
            Q929_QueryTime = financeCheck.Q929_QueryTime,
            NameChecked = mainNameChecked,
            IsBranchCustomer = financeCheck.IsBranchCustomer,
            Focus1Check = financeCheck.Focus1Check,
            Focus1Hit = financeCheck.Focus1Hit,
            Focus1_QueryTime = financeCheck.Focus1_QueryTime,
            Focus2Check = financeCheck.Focus2Check,
            Focus2Hit = financeCheck.Focus2Hit,
            Focus2_QueryTime = financeCheck.Focus2_QueryTime,
            AMLRiskLevel = financeCheck.AMLRiskLevel,
            KYC_StrongReStatus = financeCheck.KYC_StrongReStatus,
        };
    }

    public static ReviewerFinanceCheckSupplementaryContext MapToReviewerFinanceCheckSupplementaryContext(
        Reviewer_FinanceCheckInfo financeCheck,
        string supplementaryNameChecked,
        string? chName = null
    )
    {
        return new ReviewerFinanceCheckSupplementaryContext
        {
            ID = financeCheck.ID,
            UserType = financeCheck.UserType,
            CHName = chName,
            Checked929 = financeCheck.Checked929,
            Q929_QueryTime = financeCheck.Q929_QueryTime,
            NameChecked = supplementaryNameChecked,
            Focus1Check = financeCheck.Focus1Check,
            Focus1Hit = financeCheck.Focus1Hit,
            Focus1_QueryTime = financeCheck.Focus1_QueryTime,
            Focus2Check = financeCheck.Focus2Check,
            Focus2Hit = financeCheck.Focus2Hit,
            Focus2_QueryTime = financeCheck.Focus2_QueryTime,
        };
    }

    public static ReviewerMainAddressContext MapToReviewerMainAddressContext(Reviewer_ApplyCreditCardInfoMain main)
    {
        return new ReviewerMainAddressContext
        {
            ID = main.ID,
            UserType = UserType.正卡人,
            CHName = main.CHName,
            IsOriginalCardholder = main.IsOriginalCardholder,
            IsStudent = main.IsStudent,
            Reg_ZipCode = main.Reg_ZipCode,
            Reg_City = main.Reg_City,
            Reg_District = main.Reg_District,
            Reg_Road = main.Reg_Road,
            Reg_Number = main.Reg_Number,
            Reg_FullAddr = main.Reg_FullAddr,
            Live_ZipCode = main.Live_ZipCode,
            Live_City = main.Live_City,
            Live_District = main.Live_District,
            Live_Road = main.Live_Road,
            Live_Number = main.Live_Number,
            Live_FullAddr = main.Live_FullAddr,
            Bill_ZipCode = main.Bill_ZipCode,
            Bill_City = main.Bill_City,
            Bill_District = main.Bill_District,
            Bill_Road = main.Bill_Road,
            Bill_Number = main.Bill_Number,
            Bill_FullAddr = main.Bill_FullAddr,
            SendCard_ZipCode = main.SendCard_ZipCode,
            SendCard_City = main.SendCard_City,
            SendCard_District = main.SendCard_District,
            SendCard_Road = main.SendCard_Road,
            SendCard_Number = main.SendCard_Number,
            SendCard_FullAddr = main.SendCard_FullAddr,
            ParentLive_ZipCode = main.ParentLive_ZipCode,
            ParentLive_City = main.ParentLive_City,
            ParentLive_District = main.ParentLive_District,
            ParentLive_Road = main.ParentLive_Road,
            ParentLive_Number = main.ParentLive_Number,
            ParentLive_FullAddr = main.ParentLive_FullAddr,
        };
    }

    public static ReviewerSupplementaryAddressContext MapToReviewerSupplementaryAddressContext(
        Reviewer_ApplyCreditCardInfoSupplementary supplementary
    )
    {
        return new ReviewerSupplementaryAddressContext
        {
            ID = supplementary.ID,
            UserType = UserType.附卡人,
            CHName = supplementary.CHName,
            IsOriginalCardholder = supplementary.IsOriginalCardholder,
            SendCard_ZipCode = supplementary.SendCard_ZipCode,
            SendCard_City = supplementary.SendCard_City,
            SendCard_District = supplementary.SendCard_District,
            SendCard_Road = supplementary.SendCard_Road,
            SendCard_Number = supplementary.SendCard_Number,
            SendCard_FullAddr = supplementary.SendCard_FullAddr,
        };
    }

    /// <summary>
    /// 根據欄位名稱取得對應的 API URL
    /// </summary>
    /// <param name="field">欄位名稱</param>
    /// <returns>API URL 名稱，如果沒有對應的 API 則回傳空字串</returns>
    public static string GetAPIUrlByField(string? field)
    {
        if (string.IsNullOrWhiteSpace(field))
            return string.Empty;

        return field switch
        {
            "InternalEmailSame_Flag" => "CheckInternalEmailByApplyNo",
            "InternalMobileSame_Flag" => "CheckInternalMobileByApplyNo",
            "Q929_QueryTime" or "Checked929" => "Get929ByApplyNo",
            "Focus1Check" or "Focus1_QueryTime" => "GetFocus1ByApplyNo",
            "Focus2Check" or "Focus2_QueryTime" => "GetFocus2ByApplyNo",
            "NameChecked" => "GetNameCheck",
            "IsBranchCustomer" => "GetBranchInfoByApplyNo",
            "AMLRiskLevel" => "KYCSync",
            "SameEmail_Flag" => "CheckSameEmailByApplyNo",
            "SameMobile_Flag" => "CheckSameMobileByApplyNo",
            "EqualInternalIP_Flag" => "CheckInternalIPByApplyNo",
            "SameIP_Flag" => "CheckSameIPByApplyNo",
            "ShortTimeID_Flag" => string.Empty, // TODO: 需要實作
            _ => string.Empty,
        };
    }

    public static RetryCheck MapToRetryCheck(ReviewerValidationError error, string MainName, string SupplementaryName)
    {
        return new RetryCheck()
        {
            Message = error.Message,
            Field = error.Field,
            APIUrl = ReviewerMapHelper.GetAPIUrlByField(error.Field),
            ID = error.Id,
            Name = error.Source == ValidateSourceType.正卡人 ? MainName : SupplementaryName,
            UserType = error.Source == ValidateSourceType.正卡人 ? UserType.正卡人 : UserType.附卡人,
        };
    }
}
