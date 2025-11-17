using ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;
using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Helpers;

public static class MapHelper
{
    /// <summary>
    /// 將 PendingPaperCase 對應到 PaperCheckJobContext
    /// </summary>
    /// <param name="pendingCase">待處理案件</param>
    /// <param name="caseDetail">案件詳細資料</param>
    /// <returns>檢核上下文</returns>
    public static PaperCheckJobContext MapToPaperCheckJobContext(ReviewerPedding_PaperApplyCardCheckJob pendingCase, ApplyCaseDetail caseDetail)
    {
        var context = new PaperCheckJobContext
        {
            ApplyNo = caseDetail.ApplyNo,
            CardOwner = caseDetail.CardOwner,
            IsChecked = pendingCase.IsChecked,
            ErrorCount = pendingCase.ErrorCount,
        };

        // 建立檢核配置
        var caseConfig = CreateCheckConfiguration(pendingCase);

        // 建立正卡人檢核結果
        // 正卡人直接看案件狀態即可,因為它全部都要檢核
        context.UserCheckResults.Add(
            UserCheckResult.Create(
                id: caseDetail.MainID,
                name: caseDetail.MainName,
                email: caseDetail.MainEmail,
                mobile: caseDetail.MainMobile,
                userType: UserType.正卡人,
                config: caseConfig,
                isOriginalCardholder: caseDetail.MainIsOriginalCardholder ?? string.Empty
            )
        );

        // 如果有附卡人，建立附卡人檢核結果
        if (caseDetail.HasSupplementary)
        {
            var suppCheckConfig = caseConfig.ForSupplementary();
            context.UserCheckResults.Add(
                UserCheckResult.Create(
                    id: caseDetail.SupplementaryID!,
                    name: caseDetail.SupplementaryName!,
                    email: string.Empty,
                    mobile: string.Empty,
                    userType: UserType.附卡人,
                    config: suppCheckConfig,
                    isOriginalCardholder: caseDetail.SupplementaryIsOriginalCardholder ?? string.Empty
                )
            );
        }

        return context;
    }

    /// <summary>
    /// 根據待處理案件建立檢核配置
    /// </summary>
    private static CheckConfiguration CreateCheckConfiguration(ReviewerPedding_PaperApplyCardCheckJob pendingCase)
    {
        return new CheckConfiguration
        {
            是否檢核原持卡人 = ShouldCheck(pendingCase.IsQueryOriginalCardholderData),
            是否檢核行內Email = ShouldCheck(pendingCase.IsCheckInternalEmail),
            是否檢核行內Mobile = ShouldCheck(pendingCase.IsCheckInternalMobile),
            是否檢核姓名檢核 = ShouldCheck(pendingCase.IsCheckName),
            是否檢核929 = ShouldCheck(pendingCase.IsCheck929),
            是否檢核分行資訊 = ShouldCheck(pendingCase.IsQueryBranchInfo),
            是否檢核關注名單 = ShouldCheck(pendingCase.IsCheckFocus),
            是否檢核頻繁ID = ShouldCheck(pendingCase.IsCheckShortTimeID),
            是否檢查重覆進件 = ShouldCheck(pendingCase.IsCheckRepeatApply),
        };
    }

    /// <summary>
    /// 判斷是否需要檢核
    /// </summary>
    private static bool ShouldCheck(CaseCheckStatus status) => status == CaseCheckStatus.需檢核_未完成 || status == CaseCheckStatus.需檢核_失敗;

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

    public static AddressContext MapToLiveAddressForSupplementary(Reviewer_ApplyCreditCardInfoSupplementary supplementary) =>
        new()
        {
            ZipCode = supplementary.Live_ZipCode,
            City = supplementary.Live_City,
            District = supplementary.Live_District,
            Road = supplementary.Live_Road,
            Number = supplementary.Live_Number,
            SubNumber = supplementary.Live_SubNumber,
            Lane = supplementary.Live_Lane,
            Alley = supplementary.Live_Alley,
            Floor = supplementary.Live_Floor,
            Other = supplementary.Live_Other,
        };

    public static AddressContext MapToSendCardAddressForSupplementary(Reviewer_ApplyCreditCardInfoSupplementary supplementary) =>
        new()
        {
            ZipCode = supplementary.SendCard_ZipCode,
            City = supplementary.SendCard_City,
            District = supplementary.SendCard_District,
            Road = supplementary.SendCard_Road,
            Number = supplementary.SendCard_Number,
            SubNumber = supplementary.SendCard_SubNumber,
            Lane = supplementary.SendCard_Lane,
            Alley = supplementary.SendCard_Alley,
            Floor = supplementary.SendCard_Floor,
            Other = supplementary.SendCard_Other,
        };
}
