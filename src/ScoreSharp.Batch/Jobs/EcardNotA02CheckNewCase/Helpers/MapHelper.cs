using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;

public static class MapHelper
{
    public static Reviewer_ApplyCreditCardInfoProcess MapToProcess(
        string applyNo,
        string action,
        DateTime startTime,
        DateTime endTime,
        string? notes = null
    )
    {
        return new()
        {
            ApplyNo = applyNo,
            ProcessUserId = "SYSTEM",
            Process = action,
            StartTime = startTime,
            EndTime = endTime,
            Notes = notes,
        };
    }

    public static VerifyContext MapToVerifyContext(CheckJobContext context) =>
        new()
        {
            是否檢核IP相同 = context.IsCheckSameIP == CaseCheckStatus.需檢核_未完成 || context.IsCheckSameIP == CaseCheckStatus.需檢核_失敗,

            是否檢核行內IP =
                context.IsCheckEqualInternalIP == CaseCheckStatus.需檢核_未完成 || context.IsCheckEqualInternalIP == CaseCheckStatus.需檢核_失敗,

            是否檢核網路電子郵件 =
                context.IsCheckSameWebCaseEmail == CaseCheckStatus.需檢核_未完成 || context.IsCheckSameWebCaseEmail == CaseCheckStatus.需檢核_失敗,

            是否檢核網路手機號碼 =
                context.IsCheckSameWebCaseMobile == CaseCheckStatus.需檢核_未完成 || context.IsCheckSameWebCaseMobile == CaseCheckStatus.需檢核_失敗,

            是否檢核929 = context.IsCheck929 == CaseCheckStatus.需檢核_未完成 || context.IsCheck929 == CaseCheckStatus.需檢核_失敗,

            是否檢核分行資訊 = context.IsQueryBranchInfo == CaseCheckStatus.需檢核_未完成 || context.IsQueryBranchInfo == CaseCheckStatus.需檢核_失敗,

            是否檢核關注名單 = context.IsCheckFocus == CaseCheckStatus.需檢核_未完成 || context.IsCheckFocus == CaseCheckStatus.需檢核_失敗,

            是否檢核姓名檢查 = context.IsCheckName == CaseCheckStatus.需檢核_未完成 || context.IsCheckName == CaseCheckStatus.需檢核_失敗,

            是否檢查短時間ID相同 =
                context.IsCheckShortTimeID == CaseCheckStatus.需檢核_未完成 || context.IsCheckShortTimeID == CaseCheckStatus.需檢核_失敗,

            是否檢核行內Email =
                context.IsCheckInternalEmail == CaseCheckStatus.需檢核_未完成 || context.IsCheckInternalEmail == CaseCheckStatus.需檢核_失敗,

            是否檢核行內手機 =
                context.IsCheckInternalMobile == CaseCheckStatus.需檢核_未完成 || context.IsCheckInternalMobile == CaseCheckStatus.需檢核_失敗,

            是否檢查重覆進件 =
                context.IsCheckRepeatApply == CaseCheckStatus.需檢核_未完成 || context.IsCheckRepeatApply == CaseCheckStatus.需檢核_失敗,
        };

    public static VerifyResultContext MapToVerifyResultContext(VerifyContext verifyContext, CreditCardValidateResult validateResult) =>
        new()
        {
            檢核929成功 = verifyContext.是否檢核929 && validateResult.Check929Res.IsSuccess,
            檢核分行資訊成功 = verifyContext.是否檢核分行資訊 && validateResult.QueryBranchInfoRes.IsSuccess,
            檢核關注名單成功 = verifyContext.是否檢核關注名單 && validateResult.CheckFocusRes.IsSuccess,
            檢核IP相同成功 = verifyContext.是否檢核IP相同 && validateResult.SameIPCheckRes.IsSuccess,
            檢核行內IP成功 = verifyContext.是否檢核行內IP && validateResult.InternalIPCheckRes.IsSuccess,
            檢核電子郵件成功 = verifyContext.是否檢核網路電子郵件 && validateResult.SameEmailCheckRes.IsSuccess,
            檢核手機號碼成功 = verifyContext.是否檢核網路手機號碼 && validateResult.SameMobileCheckRes.IsSuccess,
            檢核短時間ID相同成功 = verifyContext.是否檢查短時間ID相同 && validateResult.ShortTimeIDCheckRes.IsSuccess,

            檢核姓名檢核成功 = verifyContext.是否檢核姓名檢查 && validateResult.CheckNameRes.IsSuccess,

            命中929 =
                verifyContext.是否檢核929 && validateResult.Check929Res.IsSuccess && validateResult.Check929Res.SuccessData?.Data?.是否命中 == "Y",
            命中分行資訊 =
                verifyContext.是否檢核分行資訊
                && validateResult.QueryBranchInfoRes.IsSuccess
                && validateResult.QueryBranchInfoRes.SuccessData?.Data?.是否命中 == "Y",
            命中關注名單1 =
                verifyContext.是否檢核關注名單
                && validateResult.CheckFocusRes.IsSuccess
                && validateResult.CheckFocusRes.SuccessData?.Data?.Focus1Checked == "Y",
            命中關注名單2 =
                verifyContext.是否檢核關注名單
                && validateResult.CheckFocusRes.IsSuccess
                && validateResult.CheckFocusRes.SuccessData?.Data?.Focus2Checked == "Y",
            命中IP相同 =
                verifyContext.是否檢核IP相同
                && validateResult.SameIPCheckRes.IsSuccess
                && validateResult.SameIPCheckRes.SuccessData?.Data?.是否命中 == "Y",
            命中行內IP =
                verifyContext.是否檢核行內IP
                && validateResult.InternalIPCheckRes.IsSuccess
                && validateResult.InternalIPCheckRes.SuccessData?.Data == true,
            命中電子郵件 =
                verifyContext.是否檢核網路電子郵件
                && validateResult.SameEmailCheckRes.IsSuccess
                && validateResult.SameEmailCheckRes.SuccessData?.Data?.是否命中 == "Y",
            命中手機號碼 =
                verifyContext.是否檢核網路手機號碼
                && validateResult.SameMobileCheckRes.IsSuccess
                && validateResult.SameMobileCheckRes.SuccessData?.Data?.是否命中 == "Y",
            命中短時間ID相同 =
                verifyContext.是否檢查短時間ID相同
                && validateResult.ShortTimeIDCheckRes.IsSuccess
                && validateResult.ShortTimeIDCheckRes.SuccessData?.Data?.是否命中 == "Y",
            命中姓名 =
                verifyContext.是否檢核姓名檢查
                && validateResult.CheckNameRes.IsSuccess
                && validateResult.CheckNameRes.SuccessData.Data.是否命中 == "Y",

            是否檢核行內Email成功 = verifyContext.是否檢核行內Email && validateResult.CheckInternalEmailSameRes.IsSuccess,
            是否檢核行內手機成功 = verifyContext.是否檢核行內手機 && validateResult.CheckInternalMobileSameRes.IsSuccess,
            命中行內Email =
                verifyContext.是否檢核行內Email
                && validateResult.CheckInternalEmailSameRes.IsSuccess
                && validateResult.CheckInternalEmailSameRes.SuccessData?.Data?.是否命中 == "Y",
            命中行內手機 =
                verifyContext.是否檢核行內手機
                && validateResult.CheckInternalMobileSameRes.IsSuccess
                && validateResult.CheckInternalMobileSameRes.SuccessData?.Data?.是否命中 == "Y",

            檢核重複進件成功 = verifyContext.是否檢查重覆進件 && validateResult.CheckRepeatApplyRes.IsSuccess,
            命中重複進件 =
                verifyContext.是否檢查重覆進件
                && validateResult.CheckRepeatApplyRes.IsSuccess
                && validateResult.CheckRepeatApplyRes.SuccessData?.Data == true,
        };

    public static AddressContext MapToRegAddress(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ZipCode = main.Reg_ZipCode,
            City = main.Reg_City,
            District = main.Reg_District,
            Road = main.Reg_Road,
            Alley = main.Reg_Alley,
            Number = main.Reg_Number,
            SubNumber = main.Reg_SubNumber,
            Lane = main.Reg_Lane,
            Floor = main.Reg_Floor,
            Other = main.Reg_Other,
        };

    public static AddressContext MapToLiveAddress(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ZipCode = main.Live_ZipCode,
            City = main.Live_City,
            District = main.Live_District,
            Road = main.Live_Road,
            Number = main.Live_Number,
            SubNumber = main.Live_SubNumber,
            Lane = main.Live_Lane,
            Alley = main.Live_Alley,
            Floor = main.Live_Floor,
            Other = main.Live_Other,
        };

    public static AddressContext MapToParentLiveAddress(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ZipCode = main.ParentLive_ZipCode,
            City = main.ParentLive_City,
            District = main.ParentLive_District,
            Road = main.ParentLive_Road,
            Lane = main.ParentLive_Lane,
            Alley = main.ParentLive_Alley,
            Number = main.ParentLive_Number,
            SubNumber = main.ParentLive_SubNumber,
            Floor = main.ParentLive_Floor,
            Other = main.ParentLive_Other,
        };

    public static AddressContext MapToCompAddress(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ZipCode = main.Comp_ZipCode,
            City = main.Comp_City,
            District = main.Comp_District,
            Road = main.Comp_Road,
            Number = main.Comp_Number,
            SubNumber = main.Comp_SubNumber,
            Lane = main.Comp_Lane,
            Alley = main.Comp_Alley,
            Floor = main.Comp_Floor,
            Other = main.Comp_Other,
        };

    public static AddressContext MapToBillAddress(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ZipCode = main.Bill_ZipCode,
            City = main.Bill_City,
            District = main.Bill_District,
            Road = main.Bill_Road,
            Number = main.Bill_Number,
            SubNumber = main.Bill_SubNumber,
            Lane = main.Bill_Lane,
            Alley = main.Bill_Alley,
            Floor = main.Bill_Floor,
            Other = main.Bill_Other,
        };

    public static AddressContext MapToSendCardAddress(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ZipCode = main.SendCard_ZipCode,
            City = main.SendCard_City,
            District = main.SendCard_District,
            Road = main.SendCard_Road,
            Number = main.SendCard_Number,
            SubNumber = main.SendCard_SubNumber,
            Lane = main.SendCard_Lane,
            Alley = main.SendCard_Alley,
            Floor = main.SendCard_Floor,
            Other = main.SendCard_Other,
        };

    public static QueryOriginalCardholderData MapToQueryOriginalCardholderData(QueryOriginalCardholderInfo data) =>
        new()
        {
            ID = data.ID != null ? data.ID.Trim() : string.Empty,
            EnglishName = data.EnglishName != null ? data.EnglishName.Trim() : string.Empty,
            ChineseName = data.ChineseName != null ? data.ChineseName.Trim() : string.Empty,
            BirthDate =
                data.BirthDate != null
                    ? data.BirthDate.Trim().Length == 8
                        ? data.BirthDate.Trim().ToTaiwanDate()
                        : data.BirthDate.Trim()
                    : string.Empty,
            BillZip = data.BillZip != null ? data.BillZip.Trim() : string.Empty,
            HomeTel = data.HomeTel != null ? data.HomeTel.Trim().FormatFixedPhoneNumber() : string.Empty,
            ContactTel = data.ContactTel != null ? data.ContactTel.Trim().FormatFixedPhoneNumber() : string.Empty,
            CompanyTel = data.CompanyTel != null ? data.CompanyTel.Trim().FormatFixedPhoneNumber() : string.Empty,
            CellTel = data.CellTel != null ? data.CellTel.Trim().FormatFixedPhoneNumber() : string.Empty,
            CRCrlimit = data.CRCrlimit ?? 0,
            UniformNumber = data.UniformNumber != null ? data.UniformNumber.Trim() : string.Empty,
            Email = data.Email != null ? data.Email.Trim() : string.Empty,
            Sex = MapToSex(data.Sex != null ? data.Sex.Trim() : string.Empty),
            CycleDD = data.CycleDD != null ? data.CycleDD.Trim() : string.Empty,
            BillAddr = data.BillAddr != null ? data.BillAddr.Trim() : string.Empty,
            HomeAddr = data.HomeAddr != null ? data.HomeAddr.Trim() : string.Empty,
            ContactName = data.ContactName != null ? data.ContactName.Trim() : string.Empty,
            ContactRel = data.ContactRel != null ? data.ContactRel.Trim() : string.Empty,
            CompanyAddr = data.CompanyAddr != null ? data.CompanyAddr.Trim() : string.Empty,
            SendAddr = data.SendAddr != null ? data.SendAddr.Trim() : string.Empty,
            CompanyName = data.CompanyName != null ? data.CompanyName.Trim() : string.Empty,
            CompanyTitle = data.CompanyTitle != null ? data.CompanyTitle.Trim() : string.Empty,
            EducateCode = MapToEducate(data.EducateCode != null ? data.EducateCode.Trim() : string.Empty),
            MarriageCode = MapToMarriageState(data.MarriageCode != null ? data.MarriageCode.Trim() : string.Empty),
            ProfessionCode = data.ProfessionCode != null ? data.ProfessionCode.Trim() : string.Empty,
            ProfessionPeriod = int.TryParse(data.ProfessionPeriod != null ? data.ProfessionPeriod.Trim() : string.Empty, out var result) ? result : 0,
            MonthlySalary = int.TryParse(data.Salary != null ? data.Salary.Trim() : string.Empty, out var result2) ? result2 * 10000 / 12 : 0,
            National = data.National != null ? data.National.Trim() : string.Empty,
            Passport = data.Passport != null ? data.Passport.Trim() : string.Empty,
            PassportDate = data.PassportDate != null ? data.PassportDate.Trim() : string.Empty,
            ForeignerIssueDate = data.ForeignerIssueDate != null ? data.ForeignerIssueDate.Trim() : string.Empty,
            SchoolName = data.SchoolName != null ? data.SchoolName.Trim() : string.Empty,
            ContactAddr = data.ContactAddr != null ? data.ContactAddr.Trim() : string.Empty,
            ResideNBR =
                data.ResideNBR != null
                    ? int.TryParse(data.ResideNBR.Trim(), out var result3)
                        ? result3
                        : 0
                    : 0,
            CompTrade = MapToCompTrade(
                data.ProfessionCode != null
                    ? data.ProfessionCode.Trim().Length > 0
                        ? data.ProfessionCode.Trim().Substring(0, 1)
                        : string.Empty
                    : string.Empty
            ),
            CompJobLevel = MapToCompJobLevel(
                data.ProfessionCode != null
                    ? data.ProfessionCode.Trim().Length > 1
                        ? data.ProfessionCode.Trim().Substring(1, 1)
                        : string.Empty
                    : string.Empty
            ),
        };

    public static Sex? MapToSex(string sex) =>
        sex switch
        {
            "1" => Sex.男,
            "2" => Sex.女,
            _ => null,
        };

    public static MarriageState? MapToMarriageState(string marriageCode) =>
        marriageCode switch
        {
            "1" => MarriageState.已婚,
            "2" => MarriageState.未婚,
            "3" => MarriageState.其他,
            _ => null,
        };

    public static Education? MapToEducate(string educateCode) =>
        educateCode switch
        {
            "1" => Education.博士,
            "2" => Education.碩士,
            "3" => Education.大學,
            "4" => Education.專科,
            "5" => Education.高中高職,
            "6" => Education.其他,
            _ => null,
        };

    public static CompTrade? MapToCompTrade(string professionCode) =>
        professionCode switch
        {
            "1" => CompTrade.金融業,
            "2" => CompTrade.公務機關,
            "3" => CompTrade.營造_製造_運輸業,
            "4" => CompTrade.一般商業,
            "5" => CompTrade.休閒_娛樂_服務業,
            "6" => CompTrade.軍警消防業,
            "7" => CompTrade.非營利團體,
            "8" => CompTrade.學生,
            "9" => CompTrade.自由業_其他,
            _ => null,
        };

    public static CompJobLevel? MapToCompJobLevel(string professionCode) =>
        professionCode switch
        {
            "1" => CompJobLevel.駕駛人員,
            "2" => CompJobLevel.服務生_門市人員,
            "3" => CompJobLevel.專業人員,
            "4" => CompJobLevel.專業技工,
            "5" => CompJobLevel.業務人員,
            "6" => CompJobLevel.一般職員,
            "7" => CompJobLevel.主管階層,
            "8" => CompJobLevel.股東_董事_負責人,
            "9" => CompJobLevel.家管_其他,
            _ => null,
        };
}
