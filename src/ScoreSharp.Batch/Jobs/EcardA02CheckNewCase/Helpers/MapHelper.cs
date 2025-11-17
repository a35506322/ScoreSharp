using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Helpers;

public class MapHelper
{
    public static VerifyContext MapToVerifyContext(CheckA02JobContext context) =>
        new()
        {
            是否查詢原持卡人 =
                context.IsQueryOriginalCardholderData == CaseCheckStatus.需檢核_未完成
                || context.IsQueryOriginalCardholderData == CaseCheckStatus.需檢核_失敗,

            是否檢核929 = context.IsCheck929 == CaseCheckStatus.需檢核_未完成 || context.IsCheck929 == CaseCheckStatus.需檢核_失敗,

            是否檢核行內Email =
                context.IsCheckInternalEmail == CaseCheckStatus.需檢核_未完成 || context.IsCheckInternalEmail == CaseCheckStatus.需檢核_失敗,

            是否檢核行內手機 =
                context.IsCheckInternalMobile == CaseCheckStatus.需檢核_未完成 || context.IsCheckInternalMobile == CaseCheckStatus.需檢核_失敗,

            是否檢核IP相同 = context.IsCheckSameIP == CaseCheckStatus.需檢核_未完成 || context.IsCheckSameIP == CaseCheckStatus.需檢核_失敗,

            是否檢核行內IP =
                context.IsCheckEqualInternalIP == CaseCheckStatus.需檢核_未完成 || context.IsCheckEqualInternalIP == CaseCheckStatus.需檢核_失敗,

            是否檢核網路電子郵件 =
                context.IsCheckSameWebCaseEmail == CaseCheckStatus.需檢核_未完成 || context.IsCheckSameWebCaseEmail == CaseCheckStatus.需檢核_失敗,

            是否檢核網路手機 =
                context.IsCheckSameWebCaseMobile == CaseCheckStatus.需檢核_未完成 || context.IsCheckSameWebCaseMobile == CaseCheckStatus.需檢核_失敗,

            是否檢核關注名單 = context.IsCheckFocus == CaseCheckStatus.需檢核_未完成 || context.IsCheckFocus == CaseCheckStatus.需檢核_失敗,

            是否檢查短時間ID相同 =
                context.IsCheckShortTimeID == CaseCheckStatus.需檢核_未完成 || context.IsCheckShortTimeID == CaseCheckStatus.需檢核_失敗,
            是否檢查黑名單 = context.IsBlackList == CaseCheckStatus.需檢核_未完成 || context.IsBlackList == CaseCheckStatus.需檢核_失敗,

            是否檢查重覆進件 =
                context.IsCheckRepeatApply == CaseCheckStatus.需檢核_未完成 || context.IsCheckRepeatApply == CaseCheckStatus.需檢核_失敗,
        };

    public static QueryOriginalCardholderData MapToQueryOriginalCardholderData(QueryOriginalCardholderInfo data) =>
        new()
        {
            ID = data.ID != null ? data.ID.Trim().ToHalfWidth() : string.Empty,
            EnglishName = data.EnglishName != null ? data.EnglishName.Trim().ToHalfWidth() : string.Empty,
            ChineseName = data.ChineseName != null ? data.ChineseName.Trim().ToHalfWidth() : string.Empty,
            BirthDate =
                data.BirthDate != null
                    ? data.BirthDate.Trim().ToHalfWidth().Length == 8
                        ? data.BirthDate.Trim().ToHalfWidth().ToTaiwanDate()
                        : data.BirthDate.Trim().ToHalfWidth()
                    : string.Empty,
            BillZip = data.BillZip != null ? data.BillZip.Trim().ToHalfWidth() : string.Empty,
            HomeTel = data.HomeTel != null ? data.HomeTel.Trim().ToHalfWidth().FormatFixedPhoneNumber() : string.Empty,
            ContactTel = data.ContactTel != null ? data.ContactTel.Trim().ToHalfWidth().FormatFixedPhoneNumber() : string.Empty,
            CompanyTel = data.CompanyTel != null ? data.CompanyTel.Trim().ToHalfWidth().FormatFixedPhoneNumber() : string.Empty,
            CellTel = data.CellTel != null ? data.CellTel.Trim().ToHalfWidth().FormatFixedPhoneNumber() : string.Empty,
            CRCrlimit = data.CRCrlimit ?? 0,
            UniformNumber = data.UniformNumber != null ? data.UniformNumber.Trim().ToHalfWidth() : string.Empty,
            Email = data.Email != null ? data.Email.Trim().ToHalfWidth() : string.Empty,
            Sex = MapToSex(data.Sex != null ? data.Sex.Trim().ToHalfWidth() : string.Empty),
            CycleDD = data.CycleDD != null ? data.CycleDD.Trim().ToHalfWidth() : string.Empty,
            BillAddr = data.BillAddr != null ? data.BillAddr.Trim().ToHalfWidth().Replace(" ", "").Replace("　", "") : string.Empty,
            HomeAddr = data.HomeAddr != null ? data.HomeAddr.Trim().ToHalfWidth().Replace(" ", "").Replace("　", "") : string.Empty,
            ContactName = data.ContactName != null ? data.ContactName.Trim().ToHalfWidth() : string.Empty,
            ContactRel = data.ContactRel != null ? data.ContactRel.Trim().ToHalfWidth() : string.Empty,
            CompanyAddr = data.CompanyAddr != null ? data.CompanyAddr.Trim().ToHalfWidth().Replace(" ", "").Replace("　", "") : string.Empty,
            SendAddr = data.SendAddr != null ? data.SendAddr.Trim().ToHalfWidth().Replace(" ", "").Replace("　", "") : string.Empty,
            CompanyName = data.CompanyName != null ? data.CompanyName.Trim().ToHalfWidth() : string.Empty,
            CompanyTitle = data.CompanyTitle != null ? data.CompanyTitle.Trim().ToHalfWidth() : string.Empty,
            EducateCode = MapToEducate(data.EducateCode != null ? data.EducateCode.Trim().ToHalfWidth() : string.Empty),
            MarriageCode = MapToMarriageState(data.MarriageCode != null ? data.MarriageCode.Trim().ToHalfWidth() : string.Empty),
            ProfessionCode = data.ProfessionCode != null ? data.ProfessionCode.Trim().ToHalfWidth() : string.Empty,
            ProfessionPeriod = int.TryParse(data.ProfessionPeriod != null ? data.ProfessionPeriod.Trim().ToHalfWidth() : string.Empty, out var result)
                ? result
                : 0,
            MonthlySalary = int.TryParse(data.Salary != null ? data.Salary.Trim().ToHalfWidth() : string.Empty, out var result2)
                ? result2 * 10000 / 12
                : 0,
            National = data.National != null ? data.National.Trim().ToHalfWidth() : string.Empty,
            Passport = data.Passport != null ? data.Passport.Trim().ToHalfWidth() : string.Empty,
            PassportDate = data.PassportDate != null ? data.PassportDate.Trim().ToHalfWidth() : string.Empty,
            ForeignerIssueDate = data.ForeignerIssueDate != null ? data.ForeignerIssueDate.Trim().ToHalfWidth() : string.Empty,
            SchoolName = data.SchoolName != null ? data.SchoolName.Trim().ToHalfWidth() : string.Empty,
            ContactAddr = data.ContactAddr != null ? data.ContactAddr.Trim().ToHalfWidth().Replace(" ", "").Replace("　", "") : string.Empty,
            ResideNBR =
                data.ResideNBR != null
                    ? int.TryParse(data.ResideNBR.Trim().ToHalfWidth(), out var result3)
                        ? result3
                        : 0
                    : 0,
            CompTrade = MapToCompTrade(
                data.ProfessionCode != null
                    ? data.ProfessionCode.Trim().ToHalfWidth().Length > 0
                        ? data.ProfessionCode.Trim().ToHalfWidth().Substring(0, 1)
                        : string.Empty
                    : string.Empty
            ),
            CompJobLevel = MapToCompJobLevel(
                data.ProfessionCode != null
                    ? data.ProfessionCode.Trim().ToHalfWidth().Length > 1
                        ? data.ProfessionCode.Trim().ToHalfWidth().Substring(1, 1)
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

    public static VerifyResultContext MapToVerifyResultContext(VerifyContext verifyContext, CreditCardValidateResult validateResult) =>
        new()
        {
            是否查詢原持卡人成功 = verifyContext.是否查詢原持卡人 && validateResult.QueryOriginalCardholderDataRes.IsSuccess,
            是否檢核929成功 = verifyContext.是否檢核929 && validateResult.Check929Res.IsSuccess,
            是否檢核行內Email成功 = verifyContext.是否檢核行內Email && validateResult.CheckInternalEmailSameRes.IsSuccess,
            是否檢核行內手機成功 = verifyContext.是否檢核行內手機 && validateResult.CheckInternalMobileSameRes.IsSuccess,
            是否檢核IP相同成功 = verifyContext.是否檢核IP相同 && validateResult.CheckSameIPRes.IsSuccess,
            是否檢核行內IP成功 = verifyContext.是否檢核行內IP && validateResult.CheckInternalIPRes.IsSuccess,
            是否檢核網路電子郵件成功 = verifyContext.是否檢核網路電子郵件 && validateResult.CheckSameWebCaseEmailRes.IsSuccess,
            是否檢核網路手機成功 = verifyContext.是否檢核網路手機 && validateResult.CheckSameWebMobileRes.IsSuccess,
            是否檢核關注名單成功 = verifyContext.是否檢核關注名單 && validateResult.Check929Res.IsSuccess,
            是否檢查短時間ID成功 = verifyContext.是否檢查短時間ID相同 && validateResult.CheckShortTimeIDRes.IsSuccess,
            是否檢查黑名單成功 = verifyContext.是否檢查黑名單,
            是否檢查重覆進件成功 = verifyContext.是否檢查重覆進件 && validateResult.CheckRepeatApplyRes.IsSuccess,
            命中929 =
                verifyContext.是否檢核929 && validateResult.Check929Res.IsSuccess && validateResult.Check929Res.SuccessData?.Data?.是否命中 == "Y",
            命中行內Email =
                verifyContext.是否檢核行內Email
                && validateResult.CheckInternalEmailSameRes.IsSuccess
                && validateResult.CheckInternalEmailSameRes.SuccessData?.Data?.是否命中 == "Y",
            命中行內手機 =
                verifyContext.是否檢核行內手機
                && validateResult.CheckInternalMobileSameRes.IsSuccess
                && validateResult.CheckInternalMobileSameRes.SuccessData?.Data?.是否命中 == "Y",
            命中行內IP =
                verifyContext.是否檢核行內IP
                && validateResult.CheckInternalIPRes.IsSuccess
                && validateResult.CheckInternalIPRes.SuccessData?.Data == true,
            命中網路電子郵件相同 =
                verifyContext.是否檢核網路電子郵件
                && validateResult.CheckSameWebCaseEmailRes.IsSuccess
                && validateResult.CheckSameWebCaseEmailRes.SuccessData?.Data?.是否命中 == "Y",
            命中網路手機相同 =
                verifyContext.是否檢核網路手機
                && validateResult.CheckSameWebMobileRes.IsSuccess
                && validateResult.CheckSameWebMobileRes.SuccessData?.Data?.是否命中 == "Y",
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
                && validateResult.CheckSameIPRes.IsSuccess
                && validateResult.CheckSameIPRes.SuccessData?.Data?.是否命中 == "Y",
            命中短時間ID相同 =
                verifyContext.是否檢查短時間ID相同
                && validateResult.CheckShortTimeIDRes.IsSuccess
                && validateResult.CheckShortTimeIDRes.SuccessData?.Data?.是否命中 == "Y",
            命中黑名單 = false,
            命中重覆進件 =
                verifyContext.是否檢查重覆進件
                && validateResult.CheckRepeatApplyRes.IsSuccess
                && validateResult.CheckRepeatApplyRes.SuccessData?.Data == true,
            郵遞區號計算成功 = true,
        };

    public static CompareMain MapToCompareMain(QueryOriginalCardholderData originalCardholderData) =>
        new()
        {
            ENName = originalCardholderData.EnglishName,
            CHName = originalCardholderData.ChineseName,
            BirthDay = originalCardholderData.BirthDate,
            Bill_ZipCode = originalCardholderData.BillZip,
            LivePhone = originalCardholderData.HomeTel,
            CompPhone = originalCardholderData.CompanyTel,
            Mobile = originalCardholderData.CellTel,
            CompID = originalCardholderData.UniformNumber,
            EMail = originalCardholderData.Email,
            Sex = originalCardholderData.Sex,
            Bill_FullAddr = originalCardholderData.BillAddr,
            Reg_FullAddr = originalCardholderData.HomeAddr,
            Comp_FullAddr = originalCardholderData.CompanyAddr,
            SendCard_FullAddr = originalCardholderData.SendAddr,
            CompName = originalCardholderData.CompanyName,
            CompJobTitle = originalCardholderData.CompanyTitle,
            Education = originalCardholderData.EducateCode,
            MarriageState = originalCardholderData.MarriageCode,
            CompSeniority = originalCardholderData.ProfessionPeriod,
            CurrentMonthIncome = originalCardholderData.MonthlySalary,
            CitizenshipCode = originalCardholderData.National,
            PassportNo = originalCardholderData.Passport,
            PassportDate = originalCardholderData.PassportDate,
            ResidencePermitIssueDate = originalCardholderData.ForeignerIssueDate,
            GraduatedElementarySchool = originalCardholderData.SchoolName,
            Live_FullAddr = originalCardholderData.ContactAddr,
            LiveYear = originalCardholderData.ResideNBR,
            CompTrade = originalCardholderData.CompTrade,
            CompJobLevel = originalCardholderData.CompJobLevel,
        };

    public static CompareMain MapToCompareMain(Reviewer_ApplyCreditCardInfoMain main) =>
        new()
        {
            ENName = main.ENName,
            CHName = main.CHName,
            BirthDay = main.BirthDay,
            Bill_ZipCode = main.Bill_ZipCode,
            LivePhone = main.LivePhone,
            CompPhone = main.CompPhone,
            Mobile = main.Mobile,
            CompID = main.CompID,
            EMail = main.EMail,
            Sex = main.Sex,
            Bill_City = main.Bill_City,
            Bill_Road = main.Bill_Road,
            Bill_District = main.Bill_District,
            Bill_FullAddr = main.Bill_FullAddr,
            Reg_City = main.Reg_City,
            Reg_Road = main.Reg_Road,
            Reg_District = main.Reg_District,
            Reg_FullAddr = main.Reg_FullAddr,
            Comp_City = main.Comp_City,
            Comp_Road = main.Comp_Road,
            Comp_District = main.Comp_District,
            Comp_FullAddr = main.Comp_FullAddr,
            SendCard_FullAddr = main.SendCard_FullAddr,
            CompName = main.CompName,
            CompJobTitle = main.CompJobTitle,
            Education = main.Education,
            MarriageState = main.MarriageState,
            CompSeniority = main.CompSeniority,
            CurrentMonthIncome = main.CurrentMonthIncome,
            CitizenshipCode = main.CitizenshipCode,
            PassportNo = main.PassportNo,
            PassportDate = main.PassportDate,
            ResidencePermitIssueDate = main.ResidencePermitIssueDate,
            GraduatedElementarySchool = main.GraduatedElementarySchool,
            Live_City = main.Live_City,
            Live_Road = main.Live_Road,
            Live_District = main.Live_District,
            Live_FullAddr = main.Live_FullAddr,
            LiveYear = main.LiveYear,
            CompTrade = main.CompTrade,
            CompJobLevel = main.CompJobLevel,
        };

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
}
