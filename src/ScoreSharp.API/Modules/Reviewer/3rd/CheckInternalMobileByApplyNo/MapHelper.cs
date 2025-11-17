using ScoreSharp.Common.Adapters.MW3.Models;
using ScoreSharp.Common.Extenstions;

namespace ScoreSharp.API.Modules.Reviewer3rd.CheckInternalMobileByApplyNo;

public class MapHelper
{
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
