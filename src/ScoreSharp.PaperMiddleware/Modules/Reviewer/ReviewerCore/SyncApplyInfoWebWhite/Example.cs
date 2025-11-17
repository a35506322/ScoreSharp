namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoWebWhite;

[ExampleAnnotation(Name = "[200][2000]網路件小白同步案件資料", ExampleType = ExampleType.Request)]
public class 網路件小白同步案件資料_成功_2000_ReqEx : IExampleProvider<SyncApplyInfoWebWhiteRequest>
{
    public SyncApplyInfoWebWhiteRequest GetExample()
    {
        var request = new SyncApplyInfoWebWhiteRequest()
        {
            ApplyNo = "20250603X0001",
            SyncUserId = "同步員編",
            CardOwner = CardOwner.正卡,
            M_CHName = "王小明",
            M_ID = "A123456789",
            M_Sex = Sex.男,
            M_BirthDay = "0850101",
            M_ENName = "WANG HSIU MING",
            M_BirthCitizenshipCode = BirthCitizenshipCode.中華民國,
            M_CitizenshipCode = "TW",
            M_IDIssueDate = "0900522",
            M_IDCardRenewalLocationCode = "",
            M_IDTakeStatus = IDTakeStatus.初發,
            M_Reg_ZipCode = "2013",
            M_Reg_City = "台中市",
            M_Reg_District = "北區",
            M_Reg_Road = "中正路一段",
            M_Reg_Lane = "一巷",
            M_Reg_Alley = "二弄",
            M_Reg_Number = "248號",
            M_Reg_SubNumber = "之1",
            M_Reg_Floor = "2樓",
            M_Reg_Other = "A室",
            M_Live_AddressType = LiveAddressType.同戶籍地址,
            M_Live_ZipCode = "2013",
            M_Live_City = "台中市",
            M_Live_District = "北區",
            M_Live_Road = "中正路一段",
            M_Live_Lane = "一巷",
            M_Live_Alley = "二弄",
            M_Live_Number = "248號",
            M_Live_SubNumber = "之1",
            M_Live_Floor = "2樓",
            M_Live_Other = "A室",
            M_Bill_AddressType = BillAddressType.同戶籍地址,
            M_Bill_ZipCode = "2013",
            M_Bill_City = "台中市",
            M_Bill_District = "北區",
            M_Bill_Road = "中正路一段",
            M_Bill_Lane = "一巷",
            M_Bill_Alley = "二弄",
            M_Bill_Number = "248號",
            M_Bill_SubNumber = "之1",
            M_Bill_Floor = "2樓",
            M_Bill_Other = "A室",
            M_SendCard_AddressType = SendCardAddressType.同戶籍地址,
            M_SendCard_ZipCode = "2013",
            M_SendCard_City = "台中市",
            M_SendCard_District = "北區",
            M_SendCard_Road = "中正路一段",
            M_SendCard_Lane = "一巷",
            M_SendCard_Alley = "二弄",
            M_SendCard_Number = "248號",
            M_SendCard_SubNumber = "之1",
            M_SendCard_Floor = "2樓",
            M_SendCard_Other = "A室",
            M_Mobile = "0912345678",
            M_EMail = "test@example.com",
            M_AMLProfessionCode = "12",
            M_AMLProfessionOther = "",
            M_AMLJobLevelCode = "0",
            M_CompName = "宮治飯糰店",
            M_Comp_ZipCode = "243",
            M_Comp_City = "台中市",
            M_Comp_District = "北區",
            M_Comp_Road = "中山路二段",
            M_Comp_Lane = "一巷",
            M_Comp_Alley = "二弄",
            M_Comp_Number = "37號",
            M_Comp_SubNumber = "",
            M_Comp_Floor = "1樓",
            M_Comp_Other = "",
            M_CompPhone = "02-28363433",
            M_CurrentMonthIncome = 50000,
            M_MainIncomeAndFundCodes = "2,3",
            M_MainIncomeAndFundOther = "",
            M_IsAgreeDataOpen = "Y",
            PromotionUnit = "UITC",
            PromotionUser = "100134",
            IsAgreeMarketing = "Y",
            M_IsAcceptEasyCardDefaultBonus = "Y",
            BillType = BillType.紙本帳單,
            M_Education = Education.大學,
            M_HouseRegPhone = "02-24335678",
            M_LivePhone = "02-24335678",
            M_GraduatedElementarySchool = "台中市立北區國民小學",
            M_CompID = "12345678",
            M_CompJobTitle = "店員",
            M_CompSeniority = 5,
            LiveOwner = LiveOwner.本人,
            FirstBrushingGiftCode = "09",
            ProjectCode = "OTN",
            CardInfo = new List<CardInfo>
            {
                new CardInfo
                {
                    ID = "A123456789",
                    UserType = UserType.正卡人,
                    CardStatus = CardStatus.網路件_書面申請等待MyData,
                    ApplyCardType = "JST59",
                    ApplyCardKind = ApplyCardKind.實體,
                },
            },
            ApplyProcess = new List<ApplyProcess>
            {
                new ApplyProcess
                {
                    Process = "網路件小白同步案件資料",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    ProcessUserId = "同步員編",
                    Notes = "同步成功",
                },
            },
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[400][4000]網路件小白同步案件資料-格式驗證失敗", ExampleType = ExampleType.Request)]
public class 網路件小白同步案件資料_格式驗證失敗_4000_ReqEx : IExampleProvider<SyncApplyInfoWebWhiteRequest>
{
    public SyncApplyInfoWebWhiteRequest GetExample()
    {
        var request = new SyncApplyInfoWebWhiteRequest()
        {
            ApplyNo = "20250603X0001",
            SyncUserId = "同步員編",
            CardOwner = CardOwner.正卡,
            M_CHName = "王小明",
            M_ID = "", // 故意留空以觸發格式驗證失敗
            M_Sex = Sex.男,
            M_BirthDay = "0850231",
            M_ENName = "WANG HSIU MING",
            M_BirthCitizenshipCode = BirthCitizenshipCode.中華民國,
            M_CitizenshipCode = "TW",
            M_IDIssueDate = "0900522",
            M_IDCardRenewalLocationCode = "",
            M_IDTakeStatus = IDTakeStatus.初發,
            M_Reg_ZipCode = "2013",
            M_Reg_City = "台中市",
            M_Reg_District = "北區",
            M_Reg_Road = "中正路一段",
            M_Reg_Lane = "一巷",
            M_Reg_Alley = "二弄",
            M_Reg_Number = "248號",
            M_Reg_SubNumber = "之1",
            M_Reg_Floor = "2樓",
            M_Reg_Other = "A室",
            M_Live_AddressType = LiveAddressType.同戶籍地址,
            M_Live_ZipCode = "2013",
            M_Live_City = "台中市",
            M_Live_District = "北區",
            M_Live_Road = "中正路一段",
            M_Live_Lane = "一巷",
            M_Live_Alley = "二弄",
            M_Live_Number = "248號",
            M_Live_SubNumber = "之1",
            M_Live_Floor = "2樓",
            M_Live_Other = "A室",
            M_Bill_AddressType = BillAddressType.同戶籍地址,
            M_Bill_ZipCode = "2013",
            M_Bill_City = "台中市",
            M_Bill_District = "北區",
            M_Bill_Road = "中正路一段",
            M_Bill_Lane = "一巷",
            M_Bill_Alley = "二弄",
            M_Bill_Number = "248號",
            M_Bill_SubNumber = "之1",
            M_Bill_Floor = "2樓",
            M_Bill_Other = "A室",
            M_SendCard_AddressType = SendCardAddressType.同戶籍地址,
            M_SendCard_ZipCode = "2013",
            M_SendCard_City = "台中市",
            M_SendCard_District = "北區",
            M_SendCard_Road = "中正路一段",
            M_SendCard_Lane = "一巷",
            M_SendCard_Alley = "二弄",
            M_SendCard_Number = "248號",
            M_SendCard_SubNumber = "之1",
            M_SendCard_Floor = "2樓",
            M_SendCard_Other = "A室",
            M_Mobile = "0912345678",
            M_EMail = "test@example.com",
            M_AMLProfessionCode = "12",
            M_AMLProfessionOther = "",
            M_AMLJobLevelCode = "0",
            M_CompName = "宮治飯糰店",
            M_Comp_ZipCode = "243",
            M_Comp_City = "台中市",
            M_Comp_District = "北區",
            M_Comp_Road = "中山路二段",
            M_Comp_Lane = "一巷",
            M_Comp_Alley = "二弄",
            M_Comp_Number = "37號",
            M_Comp_SubNumber = "",
            M_Comp_Floor = "1樓",
            M_Comp_Other = "",
            M_CompPhone = "02-28363433",
            M_CurrentMonthIncome = 50000,
            M_MainIncomeAndFundCodes = "2,3",
            M_MainIncomeAndFundOther = "",
            M_IsAgreeDataOpen = "Y",
            PromotionUnit = "UITC",
            PromotionUser = "100134",
            IsAgreeMarketing = "Y",
            M_IsAcceptEasyCardDefaultBonus = "Y",
            BillType = BillType.紙本帳單,
            M_Education = Education.大學,
            M_HouseRegPhone = "02-24335678",
            M_LivePhone = "02-24335678",
            M_GraduatedElementarySchool = "台中市立北區國民小學",
            M_CompID = "12345678",
            M_CompJobTitle = "店員",
            M_CompSeniority = 5,
            LiveOwner = LiveOwner.本人,
            FirstBrushingGiftCode = "09",
            ProjectCode = "OTN",
            CardInfo = new List<CardInfo>
            {
                new CardInfo
                {
                    ID = "A123456789",
                    UserType = UserType.正卡人,
                    CardStatus = CardStatus.網路件_書面申請等待MyData,
                    ApplyCardType = "JST59",
                },
            },
            ApplyProcess = new List<ApplyProcess>
            {
                new ApplyProcess
                {
                    Process = "網路件小白同步案件資料",
                    StartTime = DateTime.Now,
                    EndTime = DateTime.Now,
                    ProcessUserId = "同步員編",
                    Notes = "同步成功",
                },
            },
        };

        return request;
    }
}

[ExampleAnnotation(Name = "[200][2000]網路件小白同步案件資料-同步成功", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_成功_200_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 2000,\"returnMessage\": \"同步成功: 20250603X0001\",\"successData\": \"20250603X0001\",\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4000]網路件小白同步案件資料-格式驗證失敗", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_格式驗證失敗_400_4000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"格式驗證失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": {\"M_ID\": [\"正卡_身分證字號 欄位為必填。\"]},\"errorMessage\": \"\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4003]網路件小白同步案件資料-商業邏輯有誤", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_商業邏輯有誤_400_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4003,\"returnMessage\": \"商業邏輯有誤\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"申請書編號 20250603X0001 的卡片狀態不符合要求: 核卡作業中\",\"traceId\": \"00-a25d480828726cbf43e36e4153e3158d-2197cb78b17580c4-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[401][4004]網路件小白同步案件資料-標頭驗證失敗", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_標頭驗證失敗_401_4004_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4004,\"returnMessage\": \"標頭驗證失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"缺少必要的標頭: X-APPLYNO\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[404][4002]網路件小白同步案件資料-查無資料", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_查無資料_404_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4002,\"returnMessage\": \"查無此資料\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"查無申請書編號為20250603X0001的處理檔資料。\",\"traceId\": \"00-bbe2264aeee006fe3c9ea18a7e9a7768-7c06a958d951c8e7-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[500][5000]網路件小白同步案件資料-內部程式失敗", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_內部程式失敗_500_5000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5000,\"returnMessage\": \"內部程式失敗\",\"successData\": null,\"errorDetail\": \"System.Exception: 未預期的程式錯誤\\n at ScoreSharp.PaperMiddleware.Handler.Handle()\",\"validationErrors\": null,\"errorMessage\": \"未預期的程式錯誤\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[500][5002]網路件小白同步案件資料-資料庫執行失敗", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_資料庫執行失敗_500_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5002,\"returnMessage\": \"資料庫執行失敗\",\"successData\": null,\"errorDetail\": \"System.Data.SqlClient.SqlException: 資料庫連線逾時\\n at ScoreSharp.PaperMiddleware.Handler.Handle()\",\"validationErrors\": null,\"errorMessage\": \"修改資料庫資料失敗\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4001]網路件小白同步案件資料-資料庫定義值錯誤", ExampleType = ExampleType.Response)]
public class 網路件小白同步案件資料_資料庫定義值錯誤_400_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4001,\"returnMessage\": \"資料庫定義值錯誤\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": {\"M_BirthCitizenshipCodeOther\": [\"正卡_出生地其他 ZZ 不存在。\"],\"M_CitizenshipCode\": [\"正卡_國籍 ZZ 不存在。\"],\"M_AMLProfessionOther\": [\"當「正卡_AML職業別」選擇「其他」時，請填寫「正卡_AML職業別其他」欄位。\"],\"M_MainIncomeAndFundCodes\": [\"正卡_所得及資金來源 10 不存在。\",\"當「正卡_所得及資金來源」選擇「其他」時，請填寫「正卡_所得及資金來源其他」欄位。\"],\"CardInfo[0].ApplyCardType\": [\"申請卡別 BAC456 不存在。\"],\"CardInfo[1].ApplyCardType\": [\"申請卡別 ABC123 不存在。\"]},\"errorMessage\": \"\",\"traceId\": \"00-03a05f0df3a74c8b88d7764cabe23858-dea98087965e92f9-00\"\r\n}\r\n";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}
