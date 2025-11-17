using System.Reflection;
using Bogus;
using Newtonsoft.Json;
using ScoreSharp.Middleware.Modules.Test.GetEcardTestDataForA02;
using ScoreSharp.Middleware.Modules.Test.Helpers.Models;

namespace ScoreSharp.Middleware.Modules.Test
{
    public partial class TestController
    {
        /// <summary>
        /// 產生卡友網路進件測試資料
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(EcardNewCaseRequest))]
        [OpenApiOperation("GetEcardTestDataForA02")]
        [EndpointSpecificExample(typeof(產出卡友測試資料_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<IResult> GetEcardTestDataForA02()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.Middleware.Modules.Test.GetEcardTestDataForA02
{
    public record Query() : IRequest<EcardNewCaseRequest>;

    public class Handler(ScoreSharpContext _context, EcardCreateReqHelper _helper) : IRequestHandler<Query, EcardNewCaseRequest>
    {
        public async Task<EcardNewCaseRequest> Handle(Query request, CancellationToken cancellationToken)
        {
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

            var 卡友假資料 = new Faker<EcardNewCaseRequest>("zh_TW");

            // 初始化為空字符串
            var properties = typeof(EcardNewCaseRequest)
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.PropertyType == typeof(string));
            foreach (var property in properties)
            {
                卡友假資料.RuleFor(property.Name, f => "");
            }

            卡友假資料
                .RuleFor(o => o.CREDIT_ID, f => "A02") // 徵信代碼為A02為卡友
                .RuleFor(o => o.ID_TYPE, f => "卡友")
                .RuleFor(o => o.APPLY_NO, string.Empty)
                .RuleFor(o => o.CARD_OWNER, f => "1") // 網路進件只有 1. 正卡
                .RuleFor(o => o.MCARD_SER, (f, s) => s.ID_TYPE == "存戶" ? "JS59" : "JST59")
                .RuleFor(o => o.FORM_ID, (f, s) => 取得表單代碼(s.ID_TYPE))
                .RuleFor(o => o.SEX, f => f.PickRandom(new[] { "1", "2" })) // 1.男 2.女
                .RuleFor(o => o.CH_NAME, (f, s) => 給予性別產生中文姓名(f, s.SEX))
                .RuleFor(o => o.BIRTHDAY, f => 產生民國生日(f))
                .RuleFor(o => o.ENG_NAME, (f, s) => 給予性別產生英文姓名(s.CH_NAME))
                .RuleFor(o => o.BIRTH_PLACE, f => f.PickRandom(多筆縣市)) // 這邊目前很怪，徵審系統明明出生地設定就是 1. 中華民國 2.其他 ，但ECARD過來的是縣市地區
                .RuleFor(o => o.BIRTH_PLACE_OTHER, f => "") // 目前不確定值有哪些
                .RuleFor(o => o.NATIONALITY, f => f.PickRandom(new[] { "TW" }))
                .RuleFor(o => o.P_ID, (f, s) => 產生身分證字號(轉換測試種類(測試種類.原卡友)))
                .RuleFor(o => o.REG_ZIP, (f, s) => f.Address.ZipCode())
                .RuleFor(
                    o => o.REG_ADDR_OTHER,
                    (f, s) =>
                    {
                        var random = new Random();
                        var randomAddress = 地址.OrderBy(i => random.Next()).First();
                        return 格式化完整地址(randomAddress.City, randomAddress.Area, randomAddress.Road, "7", "7", "7", "7", "7");
                    }
                )
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
                .RuleFor(o => o.JOB_TYPE_OTHER, f => "") // 其他的話這邊要隨便填
                .RuleFor(o => o.JOB_LV, f => f.PickRandom(AML職級別多筆代碼)) // 讀取AML職級別設定
                .RuleFor(o => o.MAIN_INCOME, f => String.Join(",", f.PickRandom(主要所得及資金來源多筆代碼, 2))) // 讀取主要所得及資金來源設定
                .RuleFor(o => o.MAIN_INCOME_OTHER, f => "") // 選取其他這個值要填寫
                .RuleFor(o => o.ACCEPT_DM_FLG, f => f.PickRandom(new[] { "0", "1" })) // 0. 否 1.是
                .RuleFor(o => o.M_PROJECT_CODE_ID, (f, s) => 產生專案代號(s.ID_TYPE))
                .RuleFor(o => o.PROM_UNIT_SER, f => "911T")
                .RuleFor(o => o.PROM_USER_NAME, f => "1001234")
                .RuleFor(o => o.AGREE_MARKETING_FLG, f => "1")
                .RuleFor(o => o.NOT_ACCEPT_EPAPAER_FLG, f => "1")
                .RuleFor(o => o.BILL_TYPE, f => f.PickRandom(new[] { "1", "2", "3", "4" }))
                .RuleFor(o => o.SOURCE_TYPE, f => f.PickRandom(new[] { "ECARD" }))
                .RuleFor(o => o.SOURCE_IP, f => f.Internet.IpAddress().ToString())
                .RuleFor(o => o.OTP_MOBILE_PHONE, f => f.Phone.PhoneNumber("09########"))
                .RuleFor(o => o.OTP_TIME, f => DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"))
                .RuleFor(o => o.DIGI_CARD_FLG, f => f.PickRandom(new[] { "Y", "N" }))
                .RuleFor(o => o.CARD_KIND, f => f.PickRandom(new[] { "1", "2", "3" })) //  1.實體 2.數位 3.實體 + 數位
                .RuleFor(o => o.APPLICATION_FILE_NAME, f => f.PickRandom(申請書資訊))
                .RuleFor(
                    o => o.CARD_ADDR,
                    (f, s) =>
                    {
                        var random = new Random();
                        var randomAddress = 地址.OrderBy(i => random.Next()).First();
                        return 格式化完整地址(randomAddress.City, randomAddress.Area, randomAddress.Road, "7", "7", "7", "7", "7");
                    }
                )
                .RuleFor(o => o.KYC_CHG_FLG, (f, s) => s.ID_TYPE == "卡友" ? f.PickRandom(new[] { "Y", "N" }) : "")
                .RuleFor(o => o.CONSUM_NOTICE_FLG, f => "Y")
                .RuleFor(o => o.STUDENT_FLG, (f, s) => "N")
                .FinishWith((f, s) => s.APPLY_NO = 產生申請書編號(f, s.ID_TYPE));

            var 卡友fakerInfo = 卡友假資料.Generate();
            JsonConvert.SerializeObject(卡友fakerInfo);
            return 卡友fakerInfo;
        }

        private Ecard測試種類? 轉換測試種類(測試種類 測試種類) =>
            測試種類 switch
            {
                測試種類.原卡友 => Ecard測試種類.原卡友,
                _ => null,
            };
    }
}
