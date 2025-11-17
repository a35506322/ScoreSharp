using ScoreSharp.Common.Extenstions;
using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Helpers;

public static class MapHelper
{
    public static RetryRequest MapRetryRequest(EcardNewCaseRequest request, CaseContext caseContext)
    {
        return new RetryRequest
        {
            ApplyNo = request.ApplyNo,
            ID = request.ID,
            CHName = request.CHName,
            ApplyCardType = request.ApplyCardType,
            Source = request.Source,
            CardOwner = request.CardOwner,
            ApplyDate = caseContext.ApplyDate,
            UserType = UserType.正卡人,
        };
    }

    public static Reviewer_ApplyCreditCardInfoHandle MapToHandle(EcardNewCaseRequest request, CaseContext caseContext)
    {
        var result = new Reviewer_ApplyCreditCardInfoHandle();

        result.SeqNo = Ulid.NewUlid().ToString();
        result.ApplyNo = request.ApplyNo;
        result.ID = request.ID;
        result.UserType = UserType.正卡人;
        result.CardStatus = ConvertHelper.ConvertCardStatus(caseContext);
        result.CreditCheckCode = request.CreditCheckCode;
        result.ApplyCardType = request.ApplyCardType;
        result.ReviewerUserId = null;
        result.ApproveUserId = null;
        result.ApplyCardKind = ConvertHelper.ConvertApplyCardKind(request.ApplyCardKind);
        result.MonthlyIncomeCheckUserId = null;
        result.CaseChangeAction = null;
        result.SupplementReasonCode = null;
        result.OtherSupplementReason = null;
        result.SupplementNote = null;
        result.SupplementSendCardAddr = null;
        result.WithdrawalNote = null;
        result.RejectionReasonCode = null;
        result.OtherSupplementReason = null;
        result.RejectionNote = null;
        result.RejectionSendCardAddr = null;
        result.CardPromotionCode = null;
        result.IsPrintSMSAndPaper = null;
        result.CardLimit = null;
        result.OriginCardholderJCICNotes = null;
        result.CreditLimit_RatingAdvice = null;

        return result;
    }

    public static Reviewer_ApplyCreditCardInfoMain MapToMain(EcardNewCaseRequest request, CaseContext caseContext, string currentVersion)
    {
        var result = new Reviewer_ApplyCreditCardInfoMain();

        result.ApplyNo = request.ApplyNo;
        result.ID = request.ID!;
        result.UserType = UserType.正卡人;
        result.CHName = request.CHName;
        result.Sex = ConvertHelper.ConvertSex(request.Sex!);
        result.BirthDay = request.Birthday;
        result.ENName = request.ENName;
        result.IDIssueDate = request.IDIssueDate;
        result.IDCardRenewalLocationCode = null; //  在這邊map後出去會用request.IDCardRenewalLocationName檢查並轉換
        result.IDTakeStatus = ConvertHelper.ConvertIDTakeStatus(request.IDTakeStatus);
        result.CitizenshipCode = request.CitizenshipCode;
        result.BirthCitizenshipCode = null; // 在這邊map後出去會檢查並轉換
        result.BirthCitizenshipCodeOther = null; // 在這邊map後出去會檢查並轉換
        result.MarriageState = null;
        result.Education = ConvertHelper.ConvertEducation(request.Education);
        result.GraduatedElementarySchool = request.GraduatedElementarySchool;
        result.Mobile = request.Mobile;
        result.EMail = request.EMail;
        result.LiveAddressType = null;
        result.BillAddressType = ConvertHelper.ConvertBillAddressType(request.BillAddress);
        result.SendCardAddressType = ConvertHelper.ConvertSendCardAddressType(request.SendCardAddress);
        result.HouseRegPhone = request.HouseRegPhone;
        result.LivePhone = request.LivePhone;
        result.LiveOwner = ConvertHelper.ConvertLiveOwner(request.LiveOwner);
        result.LiveYear = null;
        result.ResidencePermitIssueDate = null;
        result.PassportNo = null;
        result.ResidencePermitIssueDate = null;
        result.ExpatValidityPeriod = null;
        result.IsFATCAIdentity = null; // 在這邊map後出去會檢查並轉換
        result.SocialSecurityCode = null;
        result.NameCheckedReasonCodes = null;
        result.ISRCAForCurrentPEP = null;
        result.ResignPEPKind = null;
        result.PEPRange = null;
        result.IsCurrentPositionRelatedPEPPosition = null;
        result.CompName = request.CompName;
        result.CompID = request.CompID;
        result.CompJobTitle = request.CompJobTitle;
        result.CompSeniority = int.TryParse(request.CompSeniority, out int seniority) ? seniority : null;
        result.CompPhone = request.CompPhone;
        result.AMLProfessionCode = request.AMLProfessionCode;
        result.AMLProfessionCode_Version = null;
        result.AMLProfessionOther = request.AMLProfessionOther;
        result.AMLJobLevelCode = request.AMLJobLevelCode;
        result.CompTrade = null;
        result.CompJobLevel = null;
        result.CurrentMonthIncome = int.TryParse(request.CurrentMonthIncome, out int monthIncome) ? monthIncome : null;
        result.MainIncomeAndFundCodes = request.MainIncomeAndFundCodes;
        result.MainIncomeAndFundOther = request.MainIncomeAndFundOther;
        result.IsStudent = request.IsStudent!;
        result.StudSchool = null;
        result.StudScheduledGraduationDate = null;
        result.ParentName = request.ParentName;
        result.ParentPhone = request.ParentPhone;
        result.ParentLiveAddressType = null;
        result.StudentApplicantRelationship = null;
        result.B68UnsecuredCreditAmount = null;
        result.IsAgreeDataOpen = ConvertHelper.ConvertYN(request.IsAgreeDataOpen!);
        result.IsPayNoticeBind = request.IsPayNoticeBind!;
        result.IsAgreeMarketing = ConvertHelper.ConvertYN(request.IsAgreeMarketing!);
        result.IsAcceptEasyCardDefaultBonus = ConvertHelper.ConvertYN(request.IsAcceptEasyCardDefaultBonus!);
        result.BillType = ConvertHelper.ConvertBillType(request.BillType!);
        result.IsApplyAutoDeduction = ConvertHelper.ConvertYN(request.IsApplyAutoDeduction!);
        result.AutoDeductionBankAccount = request.AutoDeductionBankAccount;
        result.ResidencePermitBackendNum = null;
        result.IsForeverResidencePermit = null;
        result.IsForeverResidencePermit = null;
        result.ResidencePermitDeadline = null;
        result.IsDunyangBlackList = null;
        result.OldCertificateVerified = null;
        result.AcceptType = null;
        result.CustomerServiceNotes = null;
        result.CardOwner = ConvertHelper.ConvertCardOwner(request.CardOwner).Value;
        result.CustomerSpecialNotes = null;
        result.PromotionUnit = request.PromotionUnit;
        result.PromotionUser = request.PromotionUser;
        result.AnliNo = request.AnliNo;
        result.ProjectCode = request.ProjectCode;
        result.LastUpdateTime = null;
        result.LastUpdateUserId = null;
        result.Source = ConvertHelper.ConvertSource(request.Source).Value;
        result.ApplyDate = caseContext.ApplyDate;
        result.CurrentHandleUserId = null;
        result.UserSourceIP = request.UserSourceIP;
        result.OTPTime = DateTime.Parse(request.OTPTime!);
        result.OTPMobile = request.OTPMobile;
        result.LineBankUUID = request.LineBankUUID;
        result.IsKYCChange = request.IsKYCChange;
        result.EcardAttachmentNotes = ConvertHelper.ConvertAttachmentNotes(request.EcardAttachmentNotes);
        result.MyDataCaseNo = request.MyDataCaseNo;
        result.IDType = ConvertHelper.ConvertIDType(request.IDType);
        result.FirstBrushingGiftCode = request.FirstBrushingGiftCode;
        result.CaseType = CaseType.一般件;
        result.CardAppId = request.CardAppId;
        result.AMLRiskLevel = null;
        result.IsApplyDigtalCard = request.IsApplyDigtalCard;
        result.LongTerm = null;
        result.BlackListNote = null;
        result.IsRepeatApply = null;
        result.IsOriginalCardholder = null;
        result.IsConvertCard = null;

        result.ECard_AppendixIsException = caseContext.ResultCode == 回覆代碼.附件異常 ? "Y" : "N";

        // 2025.07.22  以SysParamManage_SysParam的AMLProfessionCode_Version為當前版本
        result.AMLProfessionCode_Version = currentVersion;

        if (caseContext.CaseType == 進件類型.原卡友 && !string.IsNullOrEmpty(request.Reg_Other))
        {
            result.Reg_ZipCode = request.Reg_Zip;
            result.Reg_FullAddr = AddressHelper.將縣市台字轉換為臺字(request.Reg_Other.Trim().Replace(" ", "").Replace("　", "").ToHalfWidth());
        }
        else
        {
            result.Reg_ZipCode = request.Reg_Zip;
            result.Reg_City = request.Reg_City;
            result.Reg_District = request.Reg_District;
            result.Reg_Road = request.Reg_Road;
            result.Reg_Lane = request.Reg_Lane;
            result.Reg_Alley = request.Reg_Alley;
            result.Reg_Number = request.Reg_Number;
            result.Reg_SubNumber = request.Reg_SubNumber;
            result.Reg_Floor = request.Reg_Floor;
            result.Reg_Other = request.Reg_Other;
            result.Reg_FullAddr = null;
        }

        result.Live_ZipCode = request.Home_Zip;
        result.Live_City = request.Home_City;
        result.Live_District = request.Home_District;
        result.Live_Road = request.Home_Road;
        result.Live_Lane = request.Home_Lane;
        result.Live_Alley = request.Home_Alley;
        result.Live_Number = request.Home_Number;
        result.Live_SubNumber = request.Home_SubNumber;
        result.Live_Floor = request.Home_Floor;
        result.Live_Other = request.Home_Other;
        result.Live_FullAddr = null;

        result.Comp_ZipCode = request.Comp_Zip;
        result.Comp_City = request.Comp_City;
        result.Comp_District = request.Comp_District;
        result.Comp_Road = request.Comp_Road;
        result.Comp_Lane = request.Comp_Lane;
        result.Comp_Alley = request.Comp_Alley;
        result.Comp_Number = request.Comp_Number;
        result.Comp_SubNumber = request.Comp_SubNumber;
        result.Comp_Floor = request.Comp_Floor;
        result.Comp_Other = request.Comp_Other;
        result.Comp_FullAddr = null;

        var sendCardAddress = ConvertHelper.ConvertSendCardAddressType(request.SendCardAddress);
        // 寄卡地址
        if (!String.IsNullOrWhiteSpace(request.SendCardAddress))
        {
            if (sendCardAddress == SendCardAddressType.同戶籍地址)
            {
                result.SendCard_ZipCode = request.Reg_Zip;
                result.SendCard_City = request.Reg_City;
                result.SendCard_District = request.Reg_District;
                result.SendCard_Road = request.Reg_Road;
                result.SendCard_Lane = request.Reg_Lane;
                result.SendCard_Alley = request.Reg_Alley;
                result.SendCard_Number = request.Reg_Number;
                result.SendCard_SubNumber = request.Reg_SubNumber;
                result.SendCard_Floor = request.Reg_Floor;
                result.SendCard_Other = request.Reg_Other;
                result.SendCard_FullAddr = null;
            }
            else if (sendCardAddress == SendCardAddressType.同居住地址)
            {
                result.SendCard_ZipCode = request.Home_Zip;
                result.SendCard_City = request.Home_City;
                result.SendCard_District = request.Home_District;
                result.SendCard_Road = request.Home_Road;
                result.SendCard_Lane = request.Home_Lane;
                result.SendCard_Alley = request.Home_Alley;
                result.SendCard_Number = request.Home_Number;
                result.SendCard_SubNumber = request.Home_SubNumber;
                result.SendCard_Floor = request.Home_Floor;
                result.SendCard_Other = request.Home_Other;
                result.SendCard_FullAddr = null;
            }
            else if (sendCardAddress == SendCardAddressType.同公司地址)
            {
                result.SendCard_ZipCode = request.Comp_Zip;
                result.SendCard_City = request.Comp_City;
                result.SendCard_District = request.Comp_District;
                result.SendCard_Road = request.Comp_Road;
                result.SendCard_Lane = request.Comp_Lane;
                result.SendCard_Alley = request.Comp_Alley;
                result.SendCard_Number = request.Comp_Number;
                result.SendCard_SubNumber = request.Comp_SubNumber;
                result.SendCard_Floor = request.Comp_Floor;
                result.SendCard_Other = request.Comp_Other;
                result.SendCard_FullAddr = null;
            }
        }
        else if (caseContext.CaseType == 進件類型.原卡友 && !string.IsNullOrEmpty(request.CardAddr))
        {
            result.SendCard_FullAddr = AddressHelper.將縣市台字轉換為臺字(request.CardAddr.Trim().Replace(" ", "").Replace("　", "").ToHalfWidth());
        }

        // 帳單地址
        var billAddressType = ConvertHelper.ConvertBillAddressType(request.BillAddress);
        if (!String.IsNullOrWhiteSpace(request.BillAddress))
        {
            if (billAddressType == BillAddressType.同戶籍地址)
            {
                result.Bill_ZipCode = request.Reg_Zip;
                result.Bill_City = request.Reg_City;
                result.Bill_District = request.Reg_District;
                result.Bill_Road = request.Reg_Road;
                result.Bill_Lane = request.Reg_Lane;
                result.Bill_Alley = request.Reg_Alley;
                result.Bill_Number = request.Reg_Number;
                result.Bill_SubNumber = request.Reg_SubNumber;
                result.Bill_Floor = request.Reg_Floor;
                result.Bill_Other = request.Reg_Other;
                result.Bill_FullAddr = null;
            }
            else if (billAddressType == BillAddressType.同居住地址)
            {
                result.Bill_ZipCode = request.Home_Zip;
                result.Bill_City = request.Home_City;
                result.Bill_District = request.Home_District;
                result.Bill_Road = request.Home_Road;
                result.Bill_Lane = request.Home_Lane;
                result.Bill_Alley = request.Home_Alley;
                result.Bill_Number = request.Home_Number;
                result.Bill_SubNumber = request.Home_SubNumber;
                result.Bill_Floor = request.Home_Floor;
                result.Bill_Other = request.Home_Other;
                result.Bill_FullAddr = null;
            }
            else if (billAddressType == BillAddressType.同公司地址)
            {
                result.Bill_ZipCode = request.Comp_Zip;
                result.Bill_City = request.Comp_City;
                result.Bill_District = request.Comp_District;
                result.Bill_Road = request.Comp_Road;
                result.Bill_Lane = request.Comp_Lane;
                result.Bill_Alley = request.Comp_Alley;
                result.Bill_Number = request.Comp_Number;
                result.Bill_SubNumber = request.Comp_SubNumber;
                result.Bill_Floor = request.Comp_Floor;
                result.Bill_Other = request.Comp_Other;
                result.Bill_FullAddr = null;
            }
        }

        // 是學生情況下 家長地址為必填
        if (request.IsStudent == "Y")
        {
            result.ParentLive_ZipCode = request.ParentLive_ZipCode;
            result.ParentLive_City = request.ParentLive_City;
            result.ParentLive_District = request.ParentLive_District;
            result.ParentLive_Road = request.ParentLive_Road;
            result.ParentLive_Lane = request.ParentLive_Lane;
            result.ParentLive_Alley = request.ParentLive_Alley;
            result.ParentLive_Number = request.ParentLive_Number;
            result.ParentLive_SubNumber = request.ParentLive_SubNumber;
            result.ParentLive_Floor = request.ParentLive_Floor;
            result.ParentLive_Other = request.ParentLive_Other;
            result.ParentLive_FullAddr = null;
        }

        // SendCardAddressType.同帳單地址 && BillAddressType.同寄卡地址 處理
        if (sendCardAddress == SendCardAddressType.同帳單地址 && billAddressType == BillAddressType.同寄卡地址)
        {
            result.SendCard_ZipCode = result.Bill_ZipCode;
            result.SendCard_City = result.Bill_City;
            result.SendCard_District = result.Bill_District;
            result.SendCard_Road = result.Bill_Road;
            result.SendCard_Lane = result.Bill_Lane;
            result.SendCard_Alley = result.Bill_Alley;
            result.SendCard_Number = result.Bill_Number;
            result.SendCard_SubNumber = result.Bill_SubNumber;
            result.SendCard_Floor = result.Bill_Floor;
            result.SendCard_Other = result.Bill_Other;
            result.SendCard_FullAddr = null;
        }
        else
        {
            if (sendCardAddress == SendCardAddressType.同帳單地址)
            {
                result.SendCard_ZipCode = result.Bill_ZipCode;
                result.SendCard_City = result.Bill_City;
                result.SendCard_District = result.Bill_District;
                result.SendCard_Road = result.Bill_Road;
                result.SendCard_Lane = result.Bill_Lane;
                result.SendCard_Alley = result.Bill_Alley;
                result.SendCard_Number = result.Bill_Number;
                result.SendCard_SubNumber = result.Bill_SubNumber;
                result.SendCard_Floor = result.Bill_Floor;
                result.SendCard_Other = result.Bill_Other;
                result.SendCard_FullAddr = null;
            }
            if (billAddressType == BillAddressType.同寄卡地址)
            {
                result.Bill_ZipCode = result.SendCard_ZipCode;
                result.Bill_City = result.SendCard_City;
                result.Bill_District = result.SendCard_District;
                result.Bill_Road = result.SendCard_Road;
                result.Bill_Lane = result.SendCard_Lane;
                result.Bill_Alley = result.SendCard_Alley;
                result.Bill_Number = result.SendCard_Number;
                result.Bill_SubNumber = result.SendCard_SubNumber;
                result.Bill_Floor = result.SendCard_Floor;
                result.Bill_Other = result.SendCard_Other;
                result.Bill_FullAddr = null;
            }
        }
        result.LastUpdateUserId = UserIdConst.SYSTEM;
        result.LastUpdateTime = caseContext.ApplyDate;

        return result;
    }

    public static (List<Reviewer_ApplyFile>, List<Reviewer_ApplyCreditCardInfoFile>) MapToApplyFileAndCreditCardInfoFile(
        ProcessApplyFileResult processApplyFileResult,
        CaseContext caseContext
    )
    {
        var reviewerApplyFiles = new List<Reviewer_ApplyFile>();
        var reviewerApplyCreditCardInfoFiles = new List<Reviewer_ApplyCreditCardInfoFile>();
        var index = 2;
        foreach (var file in processApplyFileResult.ApplyFiles)
        {
            var fileId = Guid.NewGuid();
            string contentType = file.Key == "uploadPDF" ? "pdf" : "jpg";
            reviewerApplyFiles.Add(
                new Reviewer_ApplyFile
                {
                    ApplyNo = caseContext.ApplyNo,
                    FileId = fileId,
                    FileName = $"{caseContext.ApplyNo}_{file.Key}_{fileId}.{contentType}",
                    FileContent = file.Value,
                    FileType = FileType.申請書相關,
                }
            );

            reviewerApplyCreditCardInfoFiles.Add(
                new Reviewer_ApplyCreditCardInfoFile
                {
                    ApplyNo = caseContext.ApplyNo,
                    AddTime = caseContext.ApplyDate,
                    FileId = fileId,
                    Page = file.Key == "uploadPDF" ? 1 : index++,
                    Process = ConvertHelper.ConvertCardStatus(caseContext).ToString(),
                    AddUserId = UserIdConst.SYSTEM,
                    IsHistory = "N",
                    DBName = "ScoreSharp_File",
                }
            );
        }
        return (reviewerApplyFiles, reviewerApplyCreditCardInfoFiles);
    }

    public static ReviewerPedding_WebApplyCardCheckJobForA02 MapToCheckA02Job(CaseContext caseContext, EcardNewCaseRequest req)
    {
        return new ReviewerPedding_WebApplyCardCheckJobForA02
        {
            ApplyNo = caseContext.ApplyNo,
            AddTime = caseContext.ApplyDate,
            IsQueryOriginalCardholderData = CaseCheckStatus.需檢核_未完成,
            IsCheck929 = CaseCheckStatus.需檢核_未完成,
            IsCheckInternalEmail =
                !string.IsNullOrEmpty(req.EMail) && caseContext.BillType == BillType.電子帳單
                    ? CaseCheckStatus.需檢核_未完成
                    : CaseCheckStatus.不需檢核,
            IsCheckInternalMobile = string.IsNullOrEmpty(req.Mobile) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckSameIP = CaseCheckStatus.需檢核_未完成,
            IsCheckEqualInternalIP = CaseCheckStatus.需檢核_未完成,
            IsCheckSameWebCaseEmail = string.IsNullOrEmpty(req.EMail) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckSameWebCaseMobile = string.IsNullOrEmpty(req.Mobile) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckFocus = CaseCheckStatus.需檢核_未完成,
            IsCheckShortTimeID = CaseCheckStatus.需檢核_未完成,
            IsBlackList = CaseCheckStatus.需檢核_未完成,
            IsChecked = CaseCheckedStatus.未完成,
            ErrorCount = 0,
            IsCheckRepeatApply = CaseCheckStatus.需檢核_未完成,
        };
    }

    public static ReviewerPedding_WebApplyCardCheckJobForNotA02 MapToCheckNotA02Job(CaseContext caseContext, EcardNewCaseRequest req)
    {
        return new ReviewerPedding_WebApplyCardCheckJobForNotA02
        {
            ApplyNo = caseContext.ApplyNo,
            AddTime = caseContext.ApplyDate,
            IsCheckName = CaseCheckStatus.需檢核_未完成,
            IsQueryBranchInfo = CaseCheckStatus.需檢核_未完成,
            IsCheck929 = CaseCheckStatus.需檢核_未完成,
            IsCheckSameIP = CaseCheckStatus.需檢核_未完成,
            IsCheckEqualInternalIP = CaseCheckStatus.需檢核_未完成,
            IsChecked = CaseCheckedStatus.未完成,
            ErrorCount = 0,
            IsCheckSameWebCaseEmail = string.IsNullOrEmpty(req.EMail) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckSameWebCaseMobile = string.IsNullOrEmpty(req.Mobile) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckFocus = CaseCheckStatus.需檢核_未完成,
            IsCheckShortTimeID = CaseCheckStatus.需檢核_未完成,
            IsCheckInternalEmail =
                !string.IsNullOrEmpty(req.EMail) && caseContext.BillType == BillType.電子帳單
                    ? CaseCheckStatus.需檢核_未完成
                    : CaseCheckStatus.不需檢核,
            IsCheckInternalMobile = string.IsNullOrEmpty(req.Mobile) ? CaseCheckStatus.不需檢核 : CaseCheckStatus.需檢核_未完成,
            IsCheckRepeatApply = CaseCheckStatus.需檢核_未完成,
        };
    }

    public static Reviewer_OutsideBankInfo MapToOutsideBankInfo(EcardNewCaseRequest request)
    {
        return new Reviewer_OutsideBankInfo
        {
            ApplyNo = request.ApplyNo,
            ID = request.ID,
            HUOCUN_Balance = String.IsNullOrWhiteSpace(request.HUOCUN_Balance) ? null : int.Parse(request.HUOCUN_Balance),
            DINGCUN_Balance = String.IsNullOrWhiteSpace(request.DINGCUN_Balance) ? null : int.Parse(request.DINGCUN_Balance),
            HUOCUN_Balance_90 = String.IsNullOrWhiteSpace(request.HUOCUN_Balance_90) ? null : int.Parse(request.HUOCUN_Balance_90),
            DINGCUN_Balance_90 = String.IsNullOrWhiteSpace(request.DINGCUN_Balance_90) ? null : int.Parse(request.DINGCUN_Balance_90),
            BalanceUpdateDate = String.IsNullOrWhiteSpace(request.BalanceUpdateDate) ? null : DateTime.Parse(request.BalanceUpdateDate),
            UserType = UserType.正卡人,
        };
    }

    public static Reviewer_ApplyCreditCardInfoProcess MapToProcess(CaseContext caseContext)
    {
        return new Reviewer_ApplyCreditCardInfoProcess
        {
            ApplyNo = caseContext.ApplyNo,
            Process = ConvertHelper.ConvertCardStatus(caseContext).ToString(),
            StartTime = caseContext.ApplyDate,
            EndTime = caseContext.ApplyDate,
            ProcessUserId = UserIdConst.SYSTEM,
        };
    }

    public static Reviewer_ApplyNote MapToNote(EcardNewCaseRequest request)
    {
        return new()
        {
            ApplyNo = request.ApplyNo,
            UserType = UserType.正卡人,
            ID = request.ID,
            Note = "",
            UpdateUserId = null,
            UpdateTime = null,
        };
    }

    public static Reviewer_InternalCommunicate MapToCommunicate(EcardNewCaseRequest request)
    {
        return new() { ApplyNo = request.ApplyNo };
    }

    public static Reviewer_BankTrace MarpToBankTrace(EcardNewCaseRequest request)
    {
        return new()
        {
            ApplyNo = request.ApplyNo,
            ID = request.ID,
            UserType = UserType.正卡人,
        };
    }

    public static Reviewer_FinanceCheckInfo MapToFinance(EcardNewCaseRequest request, string kycStrongReVersion)
    {
        var result = new Reviewer_FinanceCheckInfo();

        result.ApplyNo = request.ApplyNo;
        result.ID = request.ID;
        result.UserType = UserType.正卡人;
        result.IDCheckResultChecked = null;
        result.FamilyMessageChecked = null;
        result.Checked929 = null;
        result.IsBranchCustomer = null;
        result.Focus1Check = null;
        result.Focus1Hit = null;
        result.Focus2Check = null;
        result.Focus2Hit = null;
        result.Q929_RtnCode = null;
        result.Q929_RtnMsg = null;
        result.Q929_QueryTime = null;
        result.BranchCus_RtnCode = null;
        result.BranchCus_RtnMsg = null;
        result.BranchCus_QueryTime = null;
        result.Focus1_RtnCode = null;
        result.Focus1_RtnMsg = null;
        result.Focus1_QueryTime = null;
        result.Focus2_RtnCode = null;
        result.Focus2_RtnMsg = null;
        result.Focus2_QueryTime = null;
        result.KYC_StrongReStatus = null;
        result.KYC_Handler = null;
        result.KYC_Handler_SignTime = null;
        result.KYC_Reviewer = null;
        result.KYC_Reviewer_SignTime = null;
        result.KYC_KYCManager = null;
        result.KYC_KYCManager_SignTime = null;
        result.KYC_StrongReVersion = kycStrongReVersion;

        return result;
    }

    public static ErrorNotice MapToErrorNotice(string applyNo, string type, string errorTitle, EcardNewCaseRequest request, string errorDetail = "")
    {
        return new ErrorNotice
        {
            ApplyNo = applyNo,
            Type = type,
            ErrorTitle = errorTitle,
            Request = request,
            ErrorDetail = errorDetail,
        };
    }

    public static System_ErrorLog MapToSystemLog(ErrorNotice errorNotice)
    {
        return new System_ErrorLog()
        {
            ApplyNo = errorNotice.ApplyNo,
            Project = SystemErrorLogProjectConst.MIDDLEWARE,
            Source = "EcardNewCase",
            Type = errorNotice.Type,
            ErrorMessage = errorNotice.ErrorTitle,
            ErrorDetail = errorNotice.ErrorDetail,
            AddTime = DateTime.Now,
            SendStatus = SendStatus.等待,
            Request = JsonHelper.序列化物件(errorNotice.Request),
        };
    }

    public static CaseContext MapToCaseContext(EcardNewCaseRequest request, bool isCITSCard)
    {
        return new CaseContext
        {
            ApplyNo = request.ApplyNo,
            CaseType = ConvertHelper.ConvertCaseType(request.IDType, isCITSCard),
            MyDataCaseNo = request.MyDataCaseNo,
            ResultCode = 回覆代碼.匯入成功,
            IsCITSCard = isCITSCard,
            IDType = ConvertHelper.ConvertIDType(request.IDType),
            BillType = ConvertHelper.ConvertBillType(request.BillType),
        };
    }

    /// <summary>
    /// 將 EcardNewCaseRequest 的全形字元轉換為半形字元
    /// </summary>
    /// <param name="request">原始請求物件</param>
    /// <returns>轉換後的新請求物件</returns>
    public static EcardNewCaseRequest ToHalfWidthRequest(EcardNewCaseRequest request)
    {
        return new EcardNewCaseRequest
        {
            ApplyNo = request.ApplyNo,
            IDType = request.IDType,
            CreditCheckCode = string.IsNullOrWhiteSpace(request.CreditCheckCode) ? string.Empty : request.CreditCheckCode.Trim().ToHalfWidth(),
            CardOwner = request.CardOwner,
            ApplyCardType = string.IsNullOrWhiteSpace(request.ApplyCardType) ? string.Empty : request.ApplyCardType.Trim().ToHalfWidth(),
            FormCode = string.IsNullOrWhiteSpace(request.FormCode) ? string.Empty : request.FormCode.Trim().ToHalfWidth(),
            CHName = string.IsNullOrWhiteSpace(request.CHName) ? string.Empty : request.CHName.Trim().ToHalfWidth(),
            Sex = request.Sex,
            Birthday = string.IsNullOrWhiteSpace(request.Birthday) ? string.Empty : request.Birthday.Trim().ToHalfWidth(),
            ENName = string.IsNullOrWhiteSpace(request.ENName) ? string.Empty : request.ENName.Trim().ToHalfWidth(),
            BirthPlace = request.BirthPlace,
            BirthPlaceOther = string.IsNullOrWhiteSpace(request.BirthPlaceOther) ? string.Empty : request.BirthPlaceOther.Trim().ToHalfWidth(),
            CitizenshipCode = string.IsNullOrWhiteSpace(request.CitizenshipCode) ? string.Empty : request.CitizenshipCode.Trim().ToHalfWidth(),
            ID = string.IsNullOrWhiteSpace(request.ID) ? string.Empty : request.ID.Trim().ToHalfWidth(),
            IDIssueDate = string.IsNullOrWhiteSpace(request.IDIssueDate) ? string.Empty : request.IDIssueDate.Trim().ToHalfWidth(),
            IDCardRenewalLocationName = string.IsNullOrWhiteSpace(request.IDCardRenewalLocationName)
                ? string.Empty
                : request.IDCardRenewalLocationName.Trim().ToHalfWidth(),
            IDTakeStatus = request.IDTakeStatus,
            Reg_Zip = string.IsNullOrWhiteSpace(request.Reg_Zip) ? string.Empty : request.Reg_Zip.Trim().ToHalfWidth(),
            Reg_City = string.IsNullOrWhiteSpace(request.Reg_City)
                ? string.Empty
                : AddressHelper.將縣市台字轉換為臺字(request.Reg_City.Trim().ToHalfWidth()),
            Reg_District = string.IsNullOrWhiteSpace(request.Reg_District) ? string.Empty : request.Reg_District.Trim().ToHalfWidth(),
            Reg_Road = string.IsNullOrWhiteSpace(request.Reg_Road) ? string.Empty : request.Reg_Road.Trim().ToHalfWidth(),
            Reg_Lane = string.IsNullOrWhiteSpace(request.Reg_Lane) ? string.Empty : request.Reg_Lane.Trim().ToHalfWidth(),
            Reg_Alley = string.IsNullOrWhiteSpace(request.Reg_Alley) ? string.Empty : request.Reg_Alley.Trim().ToHalfWidth(),
            Reg_Number = string.IsNullOrWhiteSpace(request.Reg_Number) ? string.Empty : request.Reg_Number.Trim().ToHalfWidth(),
            Reg_SubNumber = string.IsNullOrWhiteSpace(request.Reg_SubNumber) ? string.Empty : request.Reg_SubNumber.Trim().ToHalfWidth(),
            Reg_Floor = string.IsNullOrWhiteSpace(request.Reg_Floor) ? string.Empty : request.Reg_Floor.Trim().ToHalfWidth(),
            Reg_Other = string.IsNullOrWhiteSpace(request.Reg_Other) ? string.Empty : request.Reg_Other.Trim().ToHalfWidth(),
            Home_Zip = string.IsNullOrWhiteSpace(request.Home_Zip) ? string.Empty : request.Home_Zip.Trim().ToHalfWidth(),
            Home_City = string.IsNullOrWhiteSpace(request.Home_City)
                ? string.Empty
                : AddressHelper.將縣市台字轉換為臺字(request.Home_City.Trim().ToHalfWidth()),
            Home_District = string.IsNullOrWhiteSpace(request.Home_District) ? string.Empty : request.Home_District.Trim().ToHalfWidth(),
            Home_Road = string.IsNullOrWhiteSpace(request.Home_Road) ? string.Empty : request.Home_Road.Trim().ToHalfWidth(),
            Home_Lane = string.IsNullOrWhiteSpace(request.Home_Lane) ? string.Empty : request.Home_Lane.Trim().ToHalfWidth(),
            Home_Alley = string.IsNullOrWhiteSpace(request.Home_Alley) ? string.Empty : request.Home_Alley.Trim().ToHalfWidth(),
            Home_Number = string.IsNullOrWhiteSpace(request.Home_Number) ? string.Empty : request.Home_Number.Trim().ToHalfWidth(),
            Home_SubNumber = string.IsNullOrWhiteSpace(request.Home_SubNumber) ? string.Empty : request.Home_SubNumber.Trim().ToHalfWidth(),
            Home_Floor = string.IsNullOrWhiteSpace(request.Home_Floor) ? string.Empty : request.Home_Floor.Trim().ToHalfWidth(),
            Home_Other = string.IsNullOrWhiteSpace(request.Home_Other) ? string.Empty : request.Home_Other.Trim().ToHalfWidth(),
            BillAddress = request.BillAddress,
            SendCardAddress = request.SendCardAddress,
            Mobile = string.IsNullOrWhiteSpace(request.Mobile) ? string.Empty : request.Mobile.Trim().ToHalfWidth(),
            EMail = request.EMail,
            AMLProfessionCode = request.AMLProfessionCode,
            AMLProfessionOther = string.IsNullOrWhiteSpace(request.AMLProfessionOther)
                ? string.Empty
                : request.AMLProfessionOther.Trim().ToHalfWidth(),
            AMLJobLevelCode = request.AMLJobLevelCode,
            CompName = string.IsNullOrWhiteSpace(request.CompName) ? string.Empty : request.CompName.Trim().ToHalfWidth(),
            CompPhone = string.IsNullOrWhiteSpace(request.CompPhone) ? string.Empty : request.CompPhone.Trim().ToHalfWidth(),
            Comp_Zip = string.IsNullOrWhiteSpace(request.Comp_Zip) ? string.Empty : request.Comp_Zip.Trim().ToHalfWidth(),
            Comp_City = string.IsNullOrWhiteSpace(request.Comp_City)
                ? string.Empty
                : AddressHelper.將縣市台字轉換為臺字(request.Comp_City.Trim().ToHalfWidth()),
            Comp_District = string.IsNullOrWhiteSpace(request.Comp_District) ? string.Empty : request.Comp_District.Trim().ToHalfWidth(),
            Comp_Road = string.IsNullOrWhiteSpace(request.Comp_Road) ? string.Empty : request.Comp_Road.Trim().ToHalfWidth(),
            Comp_Lane = string.IsNullOrWhiteSpace(request.Comp_Lane) ? string.Empty : request.Comp_Lane.Trim().ToHalfWidth(),
            Comp_Alley = string.IsNullOrWhiteSpace(request.Comp_Alley) ? string.Empty : request.Comp_Alley.Trim().ToHalfWidth(),
            Comp_Number = string.IsNullOrWhiteSpace(request.Comp_Number) ? string.Empty : request.Comp_Number.Trim().ToHalfWidth(),
            Comp_SubNumber = string.IsNullOrWhiteSpace(request.Comp_SubNumber) ? string.Empty : request.Comp_SubNumber.Trim().ToHalfWidth(),
            Comp_Floor = string.IsNullOrWhiteSpace(request.Comp_Floor) ? string.Empty : request.Comp_Floor.Trim().ToHalfWidth(),
            Comp_Other = string.IsNullOrWhiteSpace(request.Comp_Other) ? string.Empty : request.Comp_Other.Trim().ToHalfWidth(),
            CurrentMonthIncome = request.CurrentMonthIncome,
            MainIncomeAndFundCodes = request.MainIncomeAndFundCodes,
            MainIncomeAndFundOther = string.IsNullOrWhiteSpace(request.MainIncomeAndFundOther)
                ? string.Empty
                : request.MainIncomeAndFundOther.Trim().ToHalfWidth(),
            IsAgreeDataOpen = string.IsNullOrWhiteSpace(request.IsAgreeDataOpen) ? string.Empty : request.IsAgreeDataOpen.Trim().ToHalfWidth(),
            ProjectCode = string.IsNullOrWhiteSpace(request.ProjectCode) ? string.Empty : request.ProjectCode.Trim().ToHalfWidth(),
            PromotionUnit = string.IsNullOrWhiteSpace(request.PromotionUnit) ? string.Empty : request.PromotionUnit.Trim().ToHalfWidth(),
            PromotionUser = string.IsNullOrWhiteSpace(request.PromotionUser) ? string.Empty : request.PromotionUser.Trim().ToHalfWidth(),
            IsAgreeMarketing = string.IsNullOrWhiteSpace(request.IsAgreeMarketing) ? string.Empty : request.IsAgreeMarketing.Trim().ToHalfWidth(),
            IsAcceptEasyCardDefaultBonus = string.IsNullOrWhiteSpace(request.IsAcceptEasyCardDefaultBonus)
                ? string.Empty
                : request.IsAcceptEasyCardDefaultBonus.Trim().ToHalfWidth(),
            FirstBrushingGiftCode = string.IsNullOrWhiteSpace(request.FirstBrushingGiftCode)
                ? string.Empty
                : request.FirstBrushingGiftCode.Trim().ToHalfWidth(),
            HouseholdRegTransferCardForTaoyuanCitizenCard = string.IsNullOrWhiteSpace(request.HouseholdRegTransferCardForTaoyuanCitizenCard)
                ? string.Empty
                : request.HouseholdRegTransferCardForTaoyuanCitizenCard.Trim().ToHalfWidth(),
            BillType = request.BillType,
            IsApplyAutoDeduction = string.IsNullOrWhiteSpace(request.IsApplyAutoDeduction)
                ? string.Empty
                : request.IsApplyAutoDeduction.Trim().ToHalfWidth(),
            AutoDeductionBankAccount = string.IsNullOrWhiteSpace(request.AutoDeductionBankAccount)
                ? string.Empty
                : request.AutoDeductionBankAccount.Trim().ToHalfWidth(),
            AutoDeductionPayType = request.AutoDeductionPayType,
            Source = request.Source,
            UserSourceIP = string.IsNullOrWhiteSpace(request.UserSourceIP) ? string.Empty : request.UserSourceIP.Trim().ToHalfWidth(),
            OTPMobile = string.IsNullOrWhiteSpace(request.OTPMobile) ? string.Empty : request.OTPMobile.Trim().ToHalfWidth(),
            OTPTime = string.IsNullOrWhiteSpace(request.OTPTime) ? string.Empty : request.OTPTime.Trim().ToHalfWidth(),
            IsApplyDigtalCard = string.IsNullOrWhiteSpace(request.IsApplyDigtalCard) ? string.Empty : request.IsApplyDigtalCard.Trim().ToHalfWidth(),
            ApplyCardKind = request.ApplyCardKind,
            CardAppId = string.IsNullOrWhiteSpace(request.CardAppId) ? string.Empty : request.CardAppId.Trim().ToHalfWidth(),
            EcardAttachmentNotes = request.EcardAttachmentNotes,
            MyDataCaseNo = string.IsNullOrWhiteSpace(request.MyDataCaseNo) ? string.Empty : request.MyDataCaseNo.Trim().ToHalfWidth(),
            CardAddr = string.IsNullOrWhiteSpace(request.CardAddr) ? string.Empty : request.CardAddr.Trim().ToHalfWidth(),
            IsKYCChange = string.IsNullOrWhiteSpace(request.IsKYCChange) ? string.Empty : request.IsKYCChange.Trim().ToHalfWidth(),
            IsPayNoticeBind = string.IsNullOrWhiteSpace(request.IsPayNoticeBind) ? string.Empty : request.IsPayNoticeBind.Trim().ToHalfWidth(),
            LineBankUUID = string.IsNullOrWhiteSpace(request.LineBankUUID) ? string.Empty : request.LineBankUUID.Trim().ToHalfWidth(),
            HUOCUN_Balance = request.HUOCUN_Balance,
            DINGCUN_Balance = request.DINGCUN_Balance,
            HUOCUN_Balance_90 = request.HUOCUN_Balance_90,
            DINGCUN_Balance_90 = request.DINGCUN_Balance_90,
            BalanceUpdateDate = string.IsNullOrWhiteSpace(request.BalanceUpdateDate) ? string.Empty : request.BalanceUpdateDate.Trim().ToHalfWidth(),
            IsStudent = string.IsNullOrWhiteSpace(request.IsStudent) ? string.Empty : request.IsStudent.Trim().ToHalfWidth(),
            ParentName = string.IsNullOrWhiteSpace(request.ParentName) ? string.Empty : request.ParentName.Trim().ToHalfWidth(),
            ParentPhone = string.IsNullOrWhiteSpace(request.ParentPhone) ? string.Empty : request.ParentPhone.Trim().ToHalfWidth(),
            ParentLive_ZipCode = string.IsNullOrWhiteSpace(request.ParentLive_ZipCode)
                ? string.Empty
                : request.ParentLive_ZipCode.Trim().ToHalfWidth(),
            ParentLive_City = string.IsNullOrWhiteSpace(request.ParentLive_City)
                ? string.Empty
                : AddressHelper.將縣市台字轉換為臺字(request.ParentLive_City.Trim().ToHalfWidth()),
            ParentLive_District = string.IsNullOrWhiteSpace(request.ParentLive_District)
                ? string.Empty
                : request.ParentLive_District.Trim().ToHalfWidth(),
            ParentLive_Road = string.IsNullOrWhiteSpace(request.ParentLive_Road) ? string.Empty : request.ParentLive_Road.Trim().ToHalfWidth(),
            ParentLive_Lane = string.IsNullOrWhiteSpace(request.ParentLive_Lane) ? string.Empty : request.ParentLive_Lane.Trim().ToHalfWidth(),
            ParentLive_Alley = string.IsNullOrWhiteSpace(request.ParentLive_Alley) ? string.Empty : request.ParentLive_Alley.Trim().ToHalfWidth(),
            ParentLive_Number = string.IsNullOrWhiteSpace(request.ParentLive_Number) ? string.Empty : request.ParentLive_Number.Trim().ToHalfWidth(),
            ParentLive_SubNumber = string.IsNullOrWhiteSpace(request.ParentLive_SubNumber)
                ? string.Empty
                : request.ParentLive_SubNumber.Trim().ToHalfWidth(),
            ParentLive_Floor = string.IsNullOrWhiteSpace(request.ParentLive_Floor) ? string.Empty : request.ParentLive_Floor.Trim().ToHalfWidth(),
            ParentLive_Other = string.IsNullOrWhiteSpace(request.ParentLive_Other) ? string.Empty : request.ParentLive_Other.Trim().ToHalfWidth(),
            Education = request.Education,
            HouseRegPhone = string.IsNullOrWhiteSpace(request.HouseRegPhone) ? string.Empty : request.HouseRegPhone.Trim().ToHalfWidth(),
            LivePhone = string.IsNullOrWhiteSpace(request.LivePhone) ? string.Empty : request.LivePhone.Trim().ToHalfWidth(),
            LiveOwner = request.LiveOwner,
            GraduatedElementarySchool = string.IsNullOrWhiteSpace(request.GraduatedElementarySchool)
                ? string.Empty
                : request.GraduatedElementarySchool.Trim().ToHalfWidth(),
            CompID = string.IsNullOrWhiteSpace(request.CompID) ? string.Empty : request.CompID.Trim().ToHalfWidth(),
            CompJobTitle = string.IsNullOrWhiteSpace(request.CompJobTitle) ? string.Empty : request.CompJobTitle.Trim().ToHalfWidth(),
            CompSeniority = string.IsNullOrWhiteSpace(request.CompSeniority) ? string.Empty : request.CompSeniority.Trim().ToHalfWidth(),
            AnliNo = string.IsNullOrWhiteSpace(request.AnliNo) ? string.Empty : request.AnliNo.Trim().ToHalfWidth(),
            AppendixFileName_01 = request.AppendixFileName_01,
            AppendixFileName_02 = request.AppendixFileName_02,
            AppendixFileName_03 = request.AppendixFileName_03,
            AppendixFileName_04 = request.AppendixFileName_04,
            AppendixFileName_05 = request.AppendixFileName_05,
            AppendixFileName_06 = request.AppendixFileName_06,
            AppendixFileName_07 = request.AppendixFileName_07,
            AppendixFileName_08 = request.AppendixFileName_08,
        };
    }
}
