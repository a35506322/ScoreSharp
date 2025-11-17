using System.Reflection;
using Bogus;
using Newtonsoft.Json;
using ScoreSharp.Middleware.Modules.Test.GetEcardTestDataForNotA02;
using ScoreSharp.Middleware.Modules.Test.Helpers.Models;

namespace ScoreSharp.Middleware.Modules.Test
{
    public partial class TestController
    {
        /// <summary>
        /// 產生非卡友網路進件測試資料
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardNewCaseRequest))]
        [OpenApiOperation("GetEcardTestDataForNotA02")]
        [EndpointSpecificExample(typeof(產出非卡友測試資料_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IResult> GetEcardTestDataForNotA02([FromQuery] GetEcardTestDataForNotA02Request request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Test.GetEcardTestDataForNotA02
{
    public record Query(GetEcardTestDataForNotA02Request request) : IRequest<EcardNewCaseRequest>;

    public class Handler(ScoreSharpContext _context, EcardCreateReqHelper _helper) : IRequestHandler<Query, EcardNewCaseRequest>
    {
        public async Task<EcardNewCaseRequest> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryString = request.request;
            // 解構 Helper
            // 使用解構來獲取所有方法
            var (
                產生身份,
                產生民國生日,
                產生身分證發證日,
                產生申請書編號,
                取得表單代碼,
                給予性別產生中文姓名,
                給予性別產生英文姓名,
                產生身分證字號,
                格式化完整地址,
                產生專案代號,
                是否成人,
                計算年齡,
                取得相關應用資料
            ) = _helper;

            var (
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
            ) = 取得相關應用資料();

            var 非卡友假資料 = new Faker<EcardNewCaseRequest>("zh_TW");
            // 初始化為空字符串
            var properties = typeof(EcardNewCaseRequest)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string));
            foreach (var property in properties)
            {
                非卡友假資料.RuleFor(property.Name, f => "");
            }

            非卡友假資料
                // 卡片資訊
                .RuleFor(
                    o => o.ID_TYPE,
                    (f) => _helper.產生身份(f, (queryString.TestType is not null ? 轉換測試種類(queryString.TestType.Value) : null))
                )
                .RuleFor(o => o.APPLY_NO, string.Empty)
                .RuleFor(o => o.CARD_OWNER, f => "1") // 網路進件只有 1. 正卡
                .RuleFor(o => o.MCARD_SER, f => f.PickRandom(多筆卡別))
                .RuleFor(o => o.FORM_ID, (f, s) => 取得表單代碼(s.ID_TYPE))
                // 正卡人資訊
                .RuleFor(o => o.SEX, f => f.PickRandom(new[] { "1", "2" })) // 1.男 2.女
                .RuleFor(o => o.CH_NAME, (f, s) => 給予性別產生中文姓名(f, s.SEX))
                .RuleFor(o => o.BIRTHDAY, f => 產生民國生日(f))
                .RuleFor(o => o.ENG_NAME, (f, s) => 給予性別產生英文姓名(s.CH_NAME))
                .RuleFor(o => o.BIRTH_PLACE, f => f.PickRandom(多筆縣市)) // 這邊目前很怪，徵審系統明明出生地設定就是 1. 中華民國 2.其他 ，但ECARD過來的是縣市地區
                .RuleFor(o => o.NATIONALITY, f => f.PickRandom(new[] { "TW" })) // 讀取國籍設定，目前先寫死台灣
                .RuleFor(
                    o => o.P_ID,
                    (f, s) =>
                    {
                        var testType = queryString.TestType.Value;
                        var value = testType != null ? 轉換測試種類(testType) : null;

                        return 產生身分證字號(value);
                    }
                )
                .RuleFor(o => o.P_ID_GETDATE, (f, s) => 產生身分證發證日(s.BIRTHDAY, 18))
                .RuleFor(o => o.P_ID_GETADDRNAME, f => f.PickRandom(身分證換發地點多筆代碼)) // 讀取身分證領證地點
                .RuleFor(o => o.P_ID_STATUS, f => f.PickRandom(new[] { "1", "2", "3" })) // 1. 初發、2. 補發、3. 換發
                // 戶籍地址

                .RuleFor(o => o.REG_ADDR_CITY, f => f.PickRandom(多筆縣市))
                .RuleFor(o => o.REG_ADDR_DIST, (f, s) => f.PickRandom(縣市_區域[s.REG_ADDR_CITY]))
                .RuleFor(o => o.REG_ADDR_RD, (f, s) => f.PickRandom(縣市_區域_街道[s.REG_ADDR_CITY + s.REG_ADDR_DIST]))
                .RuleFor(o => o.REG_ADDR_LN, (f, s) => f.Address.BuildingNumber())
                .RuleFor(o => o.REG_ADDR_ALY, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.REG_ADDR_NO, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.REG_ADDR_SUBNO, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.REG_ADDR_F, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.REG_ADDR_OTHER, (f, s) => "")
                // 住家地址

                .RuleFor(o => o.HOME_ADDR_CITY, f => f.PickRandom(多筆縣市))
                .RuleFor(o => o.HOME_ADDR_DIST, (f, s) => f.PickRandom(縣市_區域[s.HOME_ADDR_CITY]))
                .RuleFor(o => o.HOME_ADDR_RD, (f, s) => f.PickRandom(縣市_區域_街道[s.HOME_ADDR_CITY + s.HOME_ADDR_DIST]))
                .RuleFor(o => o.HOME_ADDR_LN, (f, s) => f.Address.BuildingNumber())
                .RuleFor(o => o.HOME_ADDR_ALY, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.HOME_ADDR_NO, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.HOME_ADDR_SUBNO, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.HOME_ADDR_F, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.BILL_TO_ADDR, f => f.PickRandom(new[] { "1", "2", "3" })) //１同戶籍２同居住３同公司
                .RuleFor(o => o.CARD_TO_ADDR, f => f.PickRandom(new[] { "1", "2", "3" })) // １同戶籍２同居住３同公司
                .RuleFor(o => o.CELL_PHONE_NO, f => f.Phone.PhoneNumber("09########"))
                .RuleFor(
                    o => o.EMAIL,
                    f =>
                    {
                        string email;
                        do
                        {
                            email = f.Internet.Email();
                        } while (!char.IsLetterOrDigit(email[0])); // 如果開頭不是字母或數字就重生

                        return email;
                    }
                )
                .RuleFor(o => o.JOB_TYPE, f => f.PickRandom(AML職業別多筆代碼)) // 讀取AML職業類別設定
                .RuleFor(o => o.JOB_LV, f => f.PickRandom(AML職級別多筆代碼)) // 讀取AML職級別設定
                .RuleFor(o => o.COMP_NAME, f => "聯邦測試公司")
                .RuleFor(o => o.COMP_TEL_NO, f => $"{f.Phone.PhoneNumber("0#-########")}#{f.Random.Number(1000, 9999)}")
                .RuleFor(o => o.COMP_ID, (f, s) => !String.IsNullOrEmpty(s.COMP_NAME) ? f.Random.String2(8, "0123456789") : "")
                .RuleFor(
                    o => o.JOB_TITLE,
                    f =>
                    {
                        var jobTitle = f.Name.JobTitle();
                        return jobTitle.Length <= 20 ? jobTitle : jobTitle.Substring(0, 20); // 根據長度進行處理
                    }
                )
                .RuleFor(
                    o => o.JOB_YEAR,
                    (f, s) => 是否成人(計算年齡(s.BIRTHDAY)) ? f.Random.Number(0, 40).ToString() : f.Random.Number(0, 4).ToString()
                )
                // 公司地址

                .RuleFor(o => o.COMP_ADDR_CITY, f => f.PickRandom(多筆縣市))
                .RuleFor(o => o.COMP_ADDR_DIST, (f, s) => f.PickRandom(縣市_區域[s.COMP_ADDR_CITY]))
                .RuleFor(o => o.COMP_ADDR_RD, (f, s) => f.PickRandom(縣市_區域_街道[s.COMP_ADDR_CITY + s.COMP_ADDR_DIST]))
                .RuleFor(o => o.COMP_ADDR_LN, (f, s) => f.Address.BuildingNumber())
                .RuleFor(o => o.COMP_ADDR_ALY, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.COMP_ADDR_NO, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.COMP_ADDR_SUBNO, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.COMP_ADDR_F, (f, s) => f.Random.String2(2, "123456789"))
                .RuleFor(o => o.SALARY, f => Convert.ToInt32(f.Finance.Amount(30000, 100000, 0)).ToString())
                .RuleFor(o => o.MAIN_INCOME, f => String.Join(",", f.PickRandom(主要所得及資金來源多筆代碼, 2))) // 讀取主要所得及資金來源設定
                .RuleFor(o => o.ACCEPT_DM_FLG, f => f.PickRandom(new[] { "0", "1" })) // 0. 否 1.是
                .RuleFor(o => o.M_PROJECT_CODE_ID, (f, s) => 產生專案代號(s.ID_TYPE))
                .RuleFor(o => o.PROM_UNIT_SER, f => "100")
                .RuleFor(o => o.PROM_USER_NAME, f => "1001234")
                .RuleFor(o => o.AGREE_MARKETING_FLG, f => "1")
                .RuleFor(o => o.NOT_ACCEPT_EPAPAER_FLG, f => "1")
                .RuleFor(o => o.FAMILY6_AGE, f => "1") // 首刷禮代碼?
                .RuleFor(o => o.BILL_TYPE, f => f.PickRandom(new[] { "1", "2", "3", "4" })) // 1. 電子帳單、2. 簡訊帳單、3. 紙本帳單、4. LINE帳單
                .RuleFor(o => o.SOURCE_TYPE, f => f.PickRandom(new[] { "ECARD" }))
                .RuleFor(o => o.SOURCE_IP, f => f.Internet.IpAddress().ToString())
                .RuleFor(o => o.OTP_MOBILE_PHONE, f => f.Phone.PhoneNumber("09########"))
                .RuleFor(o => o.OTP_TIME, f => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                .RuleFor(o => o.DIGI_CARD_FLG, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(o => o.CARD_KIND, f => f.PickRandom(new[] { "1", "2", "3" })) //  1.實體 2.數位 3.實體 + 數位
                .RuleFor(o => o.APPLICATION_FILE_NAME, f => f.PickRandom(申請書資訊))
                .RuleFor(
                    o => o.APPENDIX_FLG,
                    f =>
                        queryString.TestType is not null
                            ? (queryString.TestType == 測試種類.MYDATA_小白 || queryString.TestType == 測試種類.MYDATA_存戶 ? "2" : "")
                            : ""
                )
                .RuleFor(o => o.MYDATA_NO, (f, s) => s.APPENDIX_FLG == "2" ? f.Random.Uuid().ToString() : "")
                .RuleFor(o => o.CARD_ADDR, (f, s) => "")
                .RuleFor(o => o.KYC_CHG_FLG, (f, s) => s.ID_TYPE == "卡友" ? f.PickRandom(new[] { "Y", "N" }) : "")
                .RuleFor(o => o.CONSUM_NOTICE_FLG, f => "Y")
                .RuleFor(o => o.UUID, f => "")
                .RuleFor(o => o.DEMAND_CURR_BAL, f => Convert.ToInt32(f.Finance.Amount(10000, 1000000, 0)).ToString())
                .RuleFor(o => o.TIME_CURR_BAL, f => Convert.ToInt32(f.Finance.Amount(10000, 1000000, 0)).ToString())
                .RuleFor(o => o.DEMAND_90DAY_BAL, f => Convert.ToInt32(f.Finance.Amount(10000, 1000000, 0)).ToString())
                .RuleFor(o => o.TIME_90DAY_BAL, f => Convert.ToInt32(f.Finance.Amount(10000, 1000000, 0)).ToString())
                .RuleFor(o => o.BAL_UPD_DATE, f => f.Date.Past().ToString("yyyy/MM/dd HH:mm:ss"))
                // 是否為學生身份
                .RuleFor(o => o.STUDENT_FLG, (f, s) => 是否成人(計算年齡(s.BIRTHDAY)) ? "N" : "Y")
                .RuleFor(o => o.STUDENT_FLG, (f, s) => "N")
                .RuleFor(o => o.PARENT_NAME, (f, s) => s.STUDENT_FLG == "Y" ? f.Name.FullName() : "")
                .RuleFor(o => o.PARENT_HOME_TEL_NO, (f, s) => s.STUDENT_FLG == "Y" ? f.Phone.PhoneNumber("########") : "")
                .RuleFor(o => o.PARENT_HOME_ZIP, (f, s) => s.STUDENT_FLG == "Y" ? f.Address.ZipCode() : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_CITY, (f, s) => s.STUDENT_FLG == "Y" ? f.PickRandom(多筆縣市) : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_DIST, (f, s) => s.STUDENT_FLG == "Y" ? f.PickRandom(縣市_區域[s.PARENT_HOME_ADDR_CITY]) : "")
                .RuleFor(
                    o => o.PARENT_HOME_ADDR_RD,
                    (f, s) => s.STUDENT_FLG == "Y" ? f.PickRandom(縣市_區域_街道[s.PARENT_HOME_ADDR_CITY + s.PARENT_HOME_ADDR_DIST]) : ""
                )
                .RuleFor(o => o.PARENT_HOME_ADDR_LN, (f, s) => s.STUDENT_FLG == "Y" ? f.Address.BuildingNumber() : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_ALY, (f, s) => s.STUDENT_FLG == "Y" ? f.Address.StreetSuffix() : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_NO, (f, s) => s.STUDENT_FLG == "Y" ? f.Random.String2(2, "123456789") : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_SUBNO, (f, s) => s.STUDENT_FLG == "Y" ? f.Random.String2(2, "123456789") : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_F, (f, s) => s.STUDENT_FLG == "Y" ? f.Random.String2(2, "123456789") : "")
                .RuleFor(o => o.PARENT_HOME_ADDR_OTHER, (f) => "")
                .RuleFor(o => o.EDUCATION, (f, s) => 計算年齡(s.BIRTHDAY) > 22 ? f.PickRandom(new[] { "1", "2", "3", "4", "5", "6" }) : "5") // 1：博士、2：碩士、3：大學、4：專科、5：高中高職、6：其他
                .RuleFor(o => o.REG_TEL_NO, f => f.Phone.PhoneNumber("0#-########"))
                .RuleFor(o => o.HOME_TEL_NO, f => f.Phone.PhoneNumber("0#-########"))
                .RuleFor(o => o.HOME_ADDR_COND, f => f.PickRandom(new[] { "1", "2", "3", "4", "5", "6", "7" })) // 1：本人、2：配偶、3：父母親、4：親屬、5：宿舍、6：租貸、7：其他
                .RuleFor(o => o.PRIMARY_SCHOOL, f => "聯邦國小")
                .FinishWith((f, s) => s.APPLY_NO = 產生申請書編號(f, s.ID_TYPE));

            var 非卡友fakerInfo = 非卡友假資料.Generate();

            JsonConvert.SerializeObject(非卡友fakerInfo);

            return 非卡友fakerInfo;
        }

        private Ecard測試種類? 轉換測試種類(測試種類 測試種類) =>
            測試種類 switch
            {
                測試種類.小白 => Ecard測試種類.小白,
                測試種類.存戶 => Ecard測試種類.存戶,
                測試種類.MYDATA_小白 => Ecard測試種類.MYDATA_小白,
                測試種類.MYDATA_存戶 => Ecard測試種類.MYDATA_存戶,
                測試種類.分行資訊 => Ecard測試種類.分行資訊,
                測試種類.命中關注名單_I_聯徵資料_解聘 => Ecard測試種類.命中關注名單_I_聯徵資料_解聘,
                測試種類.命中關注名單_G_失蹤人口 => Ecard測試種類.命中關注名單_G_失蹤人口,
                _ => null,
            };
    }
}
