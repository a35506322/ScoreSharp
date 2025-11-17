using System.Threading.Tasks;
using Bogus;
using hyjiacan.py4n;
using ScoreSharp.Middleware.Modules.Test.Helpers.Models;
using static Bogus.DataSets.Name;
using static hyjiacan.py4n.Pinyin4Net;

namespace ScoreSharp.Middleware.Modules.Test.Helpers;

public class EcardCreateReqHelper(ScoreSharpContext _context, IScoreSharpDapperContext _dapperContext, IConfiguration _configuration)
{
    public string 產生身份(Faker faker, Ecard測試種類? idKind)
    {
        if (idKind is not null && idKind != Ecard測試種類.MYDATA_小白 && idKind != Ecard測試種類.小白)
        {
            return "存戶";
        }
        else
        {
            return "";
        }
    }

    public string 產生民國生日(Faker faker)
    {
        var date = faker.Date.Past(50, DateTime.Now.AddYears(-18));
        var taiwanYear = date.Year - 1911;
        var taiwanDate = $"{taiwanYear:D3}{date:MMdd}";
        return taiwanDate;
    }

    public string 產生身分證發證日(string birthday, int addYear = 18)
    {
        // 取得民國年
        int taiwanYear = int.Parse(birthday.Substring(0, 3));
        // 取得月份
        int month = int.Parse(birthday.Substring(3, 2));
        // 取得日
        int day = int.Parse(birthday.Substring(5, 2));
        // 將民國年轉換為西元年
        int gregorianYear = taiwanYear + 1911;
        // 加上18年
        int newGregorianYear = gregorianYear + addYear;
        // 將新西元年轉回民國年
        int newTaiwanYear = newGregorianYear - 1911;
        // 格式化回民國日期格式
        string taiwanDate = newTaiwanYear.ToString("000") + month.ToString("00") + day.ToString("00");
        return taiwanDate;
    }

    public string 產生申請書編號(Faker faker, string id_type)
    {
        var datePart = DateTime.Now.ToString("yyyyMMdd");
        var fixedChar = string.IsNullOrEmpty(id_type) ? "X" : "B";
        var serialNumber = faker.Random.Int(1, 9999).ToString("D4");
        return $"{datePart}{fixedChar}{serialNumber}";
    }

    public string 取得表單代碼(string id_type) =>
        id_type switch
        {
            "" => "AP00A169",
            "卡友" => "AP00A170",
            "存戶" => "AP00A171",
            _ => throw new ArgumentException($"取得表單代碼 type 錯誤:{id_type}"),
        };

    public string 給予性別產生中文姓名(Faker faker, string sex) =>
        faker.Name.LastName(sex == "1" ? Gender.Male : Gender.Female) + faker.Name.FirstName(sex == "1" ? Gender.Male : Gender.Female);

    public string 給予性別產生英文姓名(string name)
    {
        PinyinFormat format = PinyinFormat.WITHOUT_TONE | PinyinFormat.UPPERCASE | PinyinFormat.WITH_U_AND_COLON;
        var formatName = GetPinyinArray(name, format);
        var cleanPinyin = formatName.Select(item => item.FirstOrDefault() ?? string.Empty); // 取每個拼音項的第一個字
        string finalName = string.Join(" ", cleanPinyin);
        return finalName;
    }

    public string? 產生身分證字號(Ecard測試種類? idKind)
    {
        string? id = null;

        switch (idKind)
        {
            case Ecard測試種類.命中全部關注名單_分行資訊:
                id = "A123493693";
                break;
            case Ecard測試種類.命中929:
                id = "A126327570";
                break;
            case Ecard測試種類.分行資訊:
                id = "A110035356";
                break;
            case Ecard測試種類.MYDATA_存戶:
                id = "K121223932";
                break;
            case Ecard測試種類.命中關注名單_I_聯徵資料_解聘:
                id = "N800212908";
                break;
            case Ecard測試種類.命中關注名單_G_失蹤人口:
                id = "A122911398";
                break;
            case Ecard測試種類.原卡友:
                id = "A127000014";
                break;
        }

        return id;
    }

    public string 格式化完整地址(
        string city,
        string district,
        string road,
        string alley,
        string lane,
        string number,
        string subNumber,
        string floor
    ) => $"{city}{district}{road}{alley}弄{lane}號之{subNumber}號{floor}樓";

    public string 產生專案代號(string id_type) =>
        id_type switch
        {
            "卡友" => "OTN",
            "存戶" => "ETU",
            "" => "ENP",
        };

    public bool 是否成人(int age) => (age > 22);

    public int 計算年齡By民國年(string minguoDate)
    {
        // 確認日期格式為7位數 (民國年MMDD)
        if (minguoDate.Length != 7)
        {
            throw new ArgumentException("日期格式不正確，應該是7位數格式 (民國年MMDD)。");
        }

        // 解析民國年
        int minguoYear = int.Parse(minguoDate.Substring(0, 3));
        int month = int.Parse(minguoDate.Substring(3, 2));
        int day = int.Parse(minguoDate.Substring(5, 2));

        // 將民國年轉為西元年
        int year = minguoYear + 1911;

        // 創建西元日期
        DateTime birthDate = new DateTime(year, month, day);
        DateTime today = DateTime.Today;

        // 計算年齡
        int age = today.Year - birthDate.Year;

        // 如果還沒有過今年的生日，年齡 -1
        if (today < birthDate.AddYears(age))
        {
            age--;
        }

        return age;
    }

    public (
        IEnumerable<string> 身分證換發地點多筆代碼,
        IEnumerable<string> AML職級別多筆代碼,
        IEnumerable<string> AML職業別多筆代碼,
        IEnumerable<string> 主要所得及資金來源多筆代碼,
        IEnumerable<SetUp_AddressInfo> 地址,
        IEnumerable<string> 多筆縣市,
        Dictionary<string, IEnumerable<string>> 縣市_區域,
        Dictionary<string, IEnumerable<string>> 縣市_區域_街道,
        IEnumerable<string> 取得申請書資訊,
        IEnumerable<string> 多筆卡別
    ) 取得相關應用資料()
    {
        var 身分證換發地點多筆代碼 = _context.SetUp_IDCardRenewalLocation.Where(x => x.IsActive == "Y").Select(x => x.IDCardRenewalLocationName);

        var AML職級別多筆代碼 = _context.SetUp_AMLJobLevel.Where(x => x.IsActive == "Y").Select(s => s.AMLJobLevelCode);

        var amlProfessionList = _context.SetUp_AMLProfession.ToList();

        // Step 2：找出當前系統設定之AML職業類別版本
        var currentVersion = _context.SysParamManage_SysParam.FirstOrDefault().AMLProfessionCode_Version;
        string amlProfessionOtherCode = _configuration.GetSection($"ValidationSetting:AMLProfessionOther_{currentVersion}").Value;

        // Step 3：找出符合前系統設定版本的代碼
        var AML職業別多筆代碼 = amlProfessionList
            .Where(x => x.Version == currentVersion && x.IsActive == "Y" && x.AMLProfessionCode != amlProfessionOtherCode)
            .Select(x => x.AMLProfessionCode)
            .ToList();

        var 主要所得及資金來源多筆代碼 = _context
            .SetUp_MainIncomeAndFund.Where(x => x.IsActive == "Y" && x.MainIncomeAndFundCode != "9")
            .Select(s => s.MainIncomeAndFundCode);

        var 地址 = _context.SetUp_AddressInfo.ToList();
        var 多筆縣市 = 地址.Select(i => i.City).Distinct();

        var 縣市_區域 = 地址.GroupBy(x => x.City).ToDictionary(g => g.Key, g => g.Select(a => a.Area).Distinct());

        var 縣市_區域_街道 = 地址.GroupBy(i => new { i.City, i.Area })
            .ToDictionary(g => g.Key.City + g.Key.Area, g => g.Select(a => a.Road).Distinct());

        var 申請書資訊 = 取得申請書資訊();

        var 多筆卡別 = _context.SetUp_Card.Where(x => x.IsActive == "Y").Select(x => x.CardCode).ToList();

        return (
            身分證換發地點多筆代碼,
            AML職級別多筆代碼,
            AML職業別多筆代碼,
            主要所得及資金來源多筆代碼,
            地址,
            多筆縣市,
            縣市_區域,
            縣市_區域_街道,
            申請書資訊,
            多筆卡別
        );
    }

    public void Deconstruct(
        out Func<Faker, Ecard測試種類?, string> 產生身份,
        out Func<Faker, string> 產生民國生日,
        out Func<string, int, string> 產生身分證發證日,
        out Func<Faker, string, string> 產生申請書編號,
        out Func<string, string> 取得表單代碼,
        out Func<Faker, string, string> 給予性別產生中文姓名,
        out Func<string, string> 給予性別產生英文姓名,
        out Func<Ecard測試種類?, string> 產生身分證字號,
        out Func<string, string, string, string, string, string, string, string, string> 格式化完整地址,
        out Func<string, string> 產生專案代號,
        out Func<int, bool> 是否成人,
        out Func<string, int> 計算年齡By民國年,
        out Func<(
            IEnumerable<string> 身分證換發地點多筆代碼,
            IEnumerable<string> AML職級別多筆代碼,
            IEnumerable<string> AML職業別多筆代碼,
            IEnumerable<string> 主要所得及資金來源多筆代碼,
            IEnumerable<SetUp_AddressInfo> 地址,
            IEnumerable<string> 多筆縣市,
            Dictionary<string, IEnumerable<string>> 縣市_區域,
            Dictionary<string, IEnumerable<string>> 縣市_區域_街道,
            IEnumerable<string> 申請書資訊,
            IEnumerable<string> 多筆卡別
        )> 取得相關應用資料
    )
    {
        產生身份 = this.產生身份;
        產生民國生日 = this.產生民國生日;
        產生身分證發證日 = this.產生身分證發證日;
        產生申請書編號 = this.產生申請書編號;
        取得表單代碼 = this.取得表單代碼;
        給予性別產生中文姓名 = this.給予性別產生中文姓名;
        給予性別產生英文姓名 = this.給予性別產生英文姓名;
        產生身分證字號 = this.產生身分證字號;
        格式化完整地址 = this.格式化完整地址;
        產生專案代號 = this.產生專案代號;
        是否成人 = this.是否成人;
        計算年齡By民國年 = this.計算年齡By民國年;
        取得相關應用資料 = this.取得相關應用資料;
    }

    private IEnumerable<string> 取得申請書資訊()
    {
        string sql =
            @"SELECT TOP (3) [cCard_AppId]
                       FROM [eCard_file].[dbo].[ApplyFile]
                       ORDER BY [cCard_AppSid] DESC";
        using var conn = _dapperContext.CreateECardFileConnection();
        var result = conn.Query<string>(sql);
        return result;
    }
}
