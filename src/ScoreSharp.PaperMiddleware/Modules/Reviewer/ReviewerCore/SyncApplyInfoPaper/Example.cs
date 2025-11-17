namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoPaper;

[ExampleAnnotation(Name = "[200][2000]紙本同步案件資料", ExampleType = ExampleType.Request)]
public class 紙本同步案件資料_成功_200_2000_ReqEx : IExampleProvider<SyncApplyInfoPaperRequest>
{
    public SyncApplyInfoPaperRequest GetExample()
    {
        var request = new SyncApplyInfoPaperRequest()
        {
            ApplyNo = "20250603X0001",
            SyncStatus = SyncStatus.修改,
            SyncUserId = "同步員編",
            CardOwner = CardOwner.正卡,
            M_CHName = "北信介",
            M_ID = "A123456789",
            M_IDIssueDate = "0850101",
            M_IDCardRenewalLocationCode = "0900723",
            M_IDTakeStatus = IDTakeStatus.補發,
            M_Sex = Sex.男,
            M_MarriageState = MarriageState.未婚,
            M_ChildrenCount = 0,
            M_BirthDay = "0830705",
            M_ENName = "Shinsuke Kita",
            M_BirthCitizenshipCode = BirthCitizenshipCode.其他,
            M_BirthCitizenshipCodeOther = "JP",
            M_CitizenshipCode = "JP",
            M_Education = Education.大學,
            M_GraduatedElementarySchool = "佐野小學校",
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
            M_HouseRegPhone = "02-24335678",
            M_LivePhone = "02-24335678",
            M_Mobile = "0912345678",
            M_LiveOwner = LiveOwner.親屬,
            M_LiveYear = 31,
            M_EMail = "sk175@gmail.com",
            M_CompName = "宮治飯糰店",
            M_CompTrade = CompTrade.休閒_娛樂_服務業,
            M_AMLProfessionCode = "12",
            M_AMLProfessionOther = "",
            M_AMLJobLevelCode = "0",
            M_CompJobLevel = CompJobLevel.服務生_門市人員,
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
            M_CompID = "",
            M_CompJobTitle = "店長",
            M_DepartmentName = "",
            M_CurrentMonthIncome = 50000,
            M_EmploymentDate = "1010101",
            M_CompSeniority = 10,
            M_ReissuedCardType = "",
            M_IsAgreeDataOpen = "Y",
            M_MainIncomeAndFundCodes = "2,3",
            M_MainIncomeAndFundOther = "",
            ElecCodeId = "",
            M_IsHoldingBankCard = "N",
            FirstBrushingGiftCode = "",
            AnnualFeePaymentType = "01",
            M_IsAcceptEasyCardDefaultBonus = "Y",
            IsAgreeMarketing = "Y",
            BillType = BillType.紙本帳單,
            ProjectCode = "",
            PromotionUnit = "UITC",
            PromotionUser = "100134",
            AcceptType = AcceptType.自來件,
            AnliNo = "",
            CaseType = CaseType.急件,
            S1_ApplicantRelationship = null,
            S1_CHName = "",
            S1_ID = "",
            S1_IDIssueDate = "",
            S1_IDCardRenewalLocationCode = "",
            S1_IDTakeStatus = null,
            S1_Sex = null,
            S1_MarriageState = null,
            S1_BirthDay = "",
            S1_ENName = "",
            S1_BirthCitizenshipCode = null,
            S1_CitizenshipCode = "",
            S1_Live_ZipCode = "",
            S1_Live_City = "",
            S1_Live_District = "",
            S1_Live_Road = "",
            S1_Live_Lane = "",
            S1_Live_Alley = "",
            S1_Live_Number = "",
            S1_Live_SubNumber = "",
            S1_Live_Floor = "",
            S1_Live_Other = "",
            S1_SendCard_ZipCode = "",
            S1_SendCard_City = "",
            S1_SendCard_District = "",
            S1_SendCard_Road = "",
            S1_SendCard_Lane = "",
            S1_SendCard_Alley = "",
            S1_SendCard_Number = "",
            S1_SendCard_SubNumber = "",
            S1_SendCard_Floor = "",
            S1_SendCard_Other = "",
            S1_LivePhone = "",
            S1_Mobile = "",
            S1_CompPhone = "",
            S1_CompName = "",
            S1_CompJobTitle = "",
            CardInfo = new List<CardInfoDto>
            {
                new CardInfoDto
                {
                    ID = "A123456789",
                    UserType = UserType.正卡人,
                    CardStatus = CardStatus.紙本件_初始,
                    ApplyCardType = "JST59",
                },
            },
            ApplyProcess = new List<ApplyProcessDto>
            {
                new ApplyProcessDto
                {
                    Process = "紙本同步案件資料",
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

[ExampleAnnotation(Name = "[400][4000]紙本同步案件資料-格式驗證失敗", ExampleType = ExampleType.Request)]
public class 紙本同步案件資料_格式驗證失敗_400_4000_ReqEx : IExampleProvider<SyncApplyInfoPaperRequest>
{
    public SyncApplyInfoPaperRequest GetExample()
    {
        var request = new SyncApplyInfoPaperRequest()
        {
            ApplyNo = "20250603X0001",
            SyncStatus = SyncStatus.修改,
            SyncUserId = "同步員編",
            CardOwner = CardOwner.正卡,
            M_CHName = "北信介",
            M_ID = "A123456789",
            M_IDIssueDate = "1010230",
            M_IDCardRenewalLocationCode = "0900723",
            M_IDTakeStatus = IDTakeStatus.補發,
            M_Sex = Sex.男,
            M_MarriageState = MarriageState.未婚,
            M_ChildrenCount = 0,
            M_BirthDay = "0830705",
            M_ENName = "Shinsuke Kita",
            M_BirthCitizenshipCode = BirthCitizenshipCode.其他,
            M_CitizenshipCode = "JP",
            M_Education = Education.大學,
            M_GraduatedElementarySchool = "佐野小學校",
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
            M_HouseRegPhone = "02-24335678",
            M_LivePhone = "02-24335678",
            M_Mobile = "0912345678",
            M_LiveOwner = LiveOwner.親屬,
            M_LiveYear = 31,
            M_EMail = "sk175@gmail.com",
            M_CompName = "宮治飯糰店",
            M_CompTrade = CompTrade.休閒_娛樂_服務業,
            M_AMLProfessionCode = "12",
            M_AMLProfessionOther = "",
            M_AMLJobLevelCode = "0",
            M_CompJobLevel = CompJobLevel.服務生_門市人員,
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
            M_CompID = "",
            M_CompJobTitle = "店長",
            M_DepartmentName = "",
            M_CurrentMonthIncome = 50000,
            M_EmploymentDate = "1010101",
            M_CompSeniority = 10,
            M_ReissuedCardType = "",
            M_IsAgreeDataOpen = "Y",
            M_MainIncomeAndFundCodes = "2,3",
            M_MainIncomeAndFundOther = "",
            ElecCodeId = "",
            M_IsHoldingBankCard = "N",
            FirstBrushingGiftCode = "",
            AnnualFeePaymentType = "01",
            M_IsAcceptEasyCardDefaultBonus = "Y",
            IsAgreeMarketing = "Y",
            BillType = BillType.紙本帳單,
            ProjectCode = "",
            PromotionUnit = "UITC",
            PromotionUser = "100134",
            AcceptType = AcceptType.自來件,
            AnliNo = "",
            CaseType = CaseType.一般件,
            S1_ApplicantRelationship = null,
            S1_CHName = "",
            S1_ID = "",
            S1_IDIssueDate = "",
            S1_IDCardRenewalLocationCode = "",
            S1_IDTakeStatus = null,
            S1_Sex = null,
            S1_MarriageState = null,
            S1_BirthDay = "",
            S1_ENName = "",
            S1_BirthCitizenshipCode = null,
            S1_CitizenshipCode = "",
            S1_Live_ZipCode = "",
            S1_Live_City = "",
            S1_Live_District = "",
            S1_Live_Road = "",
            S1_Live_Lane = "",
            S1_Live_Alley = "",
            S1_Live_Number = "",
            S1_Live_SubNumber = "",
            S1_Live_Floor = "",
            S1_Live_Other = "",
            S1_SendCard_ZipCode = "",
            S1_SendCard_City = "",
            S1_SendCard_District = "",
            S1_SendCard_Road = "",
            S1_SendCard_Lane = "",
            S1_SendCard_Alley = "",
            S1_SendCard_Number = "",
            S1_SendCard_SubNumber = "",
            S1_SendCard_Floor = "",
            S1_SendCard_Other = "",
            S1_LivePhone = "",
            S1_Mobile = "",
            S1_CompPhone = "",
            S1_CompName = "",
            S1_CompJobTitle = "",
            CardInfo = new List<CardInfoDto>
            {
                new CardInfoDto
                {
                    ID = "A123456789",
                    UserType = UserType.正卡人,
                    CardStatus = CardStatus.紙本件_初始,
                    ApplyCardType = "JST59",
                },
            },
            ApplyProcess = new List<ApplyProcessDto>
            {
                new ApplyProcessDto
                {
                    Process = "紙本同步案件資料",
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

[ExampleAnnotation(Name = "[200][2000]紙本同步案件資料-同步成功", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_成功_200_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 2000,\"returnMessage\": \"同步成功: 20250603X0001\",\"successData\": \"20250603X0001\",\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4000]紙本同步案件資料-格式驗證失敗", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_格式驗證失敗_400_4000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"格式驗證失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": {\"M_ID\": [\"正卡_身分證字號 欄位為必填。\"]},\"errorMessage\": \"\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4003]紙本同步案件資料-商業邏輯有誤", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_商業邏輯有誤_400_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4003,\"returnMessage\": \"商業邏輯有誤\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"申請書編號 20250603X0001 的卡片狀態不符合要求: 核卡作業中\",\"traceId\": \"00-a25d480828726cbf43e36e4153e3158d-2197cb78b17580c4-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[401][4004]紙本同步案件資料-標頭驗證失敗", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_標頭驗證失敗_401_4004_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4004,\"returnMessage\": \"標頭驗證失敗\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"缺少必要的標頭: X-APPLYNO\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[404][4002]紙本同步案件資料-查無資料", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_查無資料_404_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4002,\"returnMessage\": \"查無此資料\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": null,\"errorMessage\": \"查無申請書編號為20250603X0001的處理檔資料。\",\"traceId\": \"00-bbe2264aeee006fe3c9ea18a7e9a7768-7c06a958d951c8e7-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[500][5000]紙本同步案件資料-內部程式失敗", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_內部程式失敗_500_5000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5000,\"returnMessage\": \"內部程式失敗\",\"successData\": null,\"errorDetail\": \"System.Exception: 未預期的程式錯誤\\n   at ScoreSharp.PaperMiddleware.Handler.Handle()\",\"validationErrors\": null,\"errorMessage\": \"未預期的程式錯誤\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[500][5002]紙本同步案件資料-資料庫執行失敗", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_資料庫執行失敗_500_5002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5002,\"returnMessage\": \"資料庫執行失敗\",\"successData\": null,\"errorDetail\": \"System.Data.SqlClient.SqlException: 資料庫連線逾時\\n   at ScoreSharp.PaperMiddleware.Handler.Handle()\",\"validationErrors\": null,\"errorMessage\": \"修改資料庫資料失敗\",\"traceId\": \"00-e12e3e0b2d285985b7de05a477e7197c-9453b591fb5b521c-00\"}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[500][5000]紙本同步案件資料-查無對應版本號的 AML 職業別「其他」代碼。", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_查無對應版本號_500_5000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 5000,\"returnMessage\": \"內部程式失敗\",\"successData\": null,\"errorDetail\": \"ScoreSharp.PaperMiddleware.Infrastructures.ExceptionHandler.InternalServerException: 查無對應版本號的 AML 職業別「其他」代碼。\\r\\n   at ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.SyncApplyInfoPaper.Handler.Handle(Command request, CancellationToken cancellationToken) in D:\\\\Project\\\\scoresharp\\\\scoresharp.backend\\\\src\\\\ScoreSharp.PaperMiddleware\\\\Modules\\\\Reviewer\\\\ReviewerCore\\\\SyncApplyInfoPaper\\\\Endpoint.cs:line 135\\r\\n   at ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.ReviewerCoreController.SyncApplyInfoPaper(String applyNo, String syncUserId, SyncApplyInfoPaperRequest request) in D:\\\\Project\\\\scoresharp\\\\scoresharp.backend\\\\src\\\\ScoreSharp.PaperMiddleware\\\\Modules\\\\Reviewer\\\\ReviewerCore\\\\SyncApplyInfoPaper\\\\Endpoint.cs:line 74\\r\\n   at lambda_method549(Closure, Object)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ActionMethodExecutor.AwaitableObjectResultExecutor.Execute(ActionContext actionContext, IActionResultTypeMapper mapper, ObjectMethodExecutor executor, Object controller, Object[] arguments)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeActionMethodAsync>g__Awaited|12_0(ControllerActionInvoker invoker, ValueTask`1 actionResultValueTask)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeNextActionFilterAsync>g__Awaited|10_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Rethrow(ActionExecutedContextSealed context)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.Next(State& next, Scope& scope, Object& state, Boolean& isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ControllerActionInvoker.<InvokeInnerFilterAsync>g__Awaited|13_0(ControllerActionInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeFilterPipelineAsync>g__Awaited|20_0(ResourceInvoker invoker, Task lastTask, State next, Scope scope, Object state, Boolean isCompleted)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\\r\\n   at Microsoft.AspNetCore.Mvc.Infrastructure.ResourceInvoker.<InvokeAsync>g__Awaited|17_0(ResourceInvoker invoker, Task task, IDisposable scope)\\r\\n   at Microsoft.AspNetCore.Authorization.AuthorizationMiddleware.Invoke(HttpContext context)\\r\\n   at Microsoft.AspNetCore.Diagnostics.ExceptionHandlerMiddlewareImpl.<Invoke>g__Awaited|10_0(ExceptionHandlerMiddlewareImpl middleware, HttpContext context, Task task)\",\"validationErrors\": null,\"errorMessage\": \"查無對應版本號的 AML 職業別「其他」代碼。\",\"traceId\": \"00-6e594c7fe87bdb012fdb2b29ad9b5056-a8a47edea6740847-00\"\r\n}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[400][4001]紙本同步案件資料-資料庫定義值錯誤", ExampleType = ExampleType.Response)]
public class 紙本同步案件資料_資料庫定義值錯誤_400_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string jsonString =
            "{\"returnCodeStatus\": 4001,\"returnMessage\": \"資料庫定義值錯誤\",\"successData\": null,\"errorDetail\": null,\"validationErrors\": {\"M_BirthCitizenshipCodeOther\": [\"正卡_出生地其他 ZZ 不存在。\"],\"M_CitizenshipCode\": [\"正卡_國籍 ZZ 不存在。\"],\"M_AMLProfessionOther\": [\"當「正卡_AML職業別」選擇「其他」時，請填寫「正卡_AML職業別其他」欄位。\"],\"M_MainIncomeAndFundCodes\": [\"正卡_所得及資金來源 10 不存在。\",\"當「正卡_所得及資金來源」選擇「其他」時，請填寫「正卡_所得及資金來源其他」欄位。\"],\"CardInfo[0].ApplyCardType\": [\"申請卡別 BAC456 不存在。\"],\"CardInfo[1].ApplyCardType\": [\"申請卡別 ABC123 不存在。\"]},\"errorMessage\": \"\",\"traceId\": \"00-03a05f0df3a74c8b88d7764cabe23858-dea98087965e92f9-00\"\r\n}\r\n";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<string>>(jsonString);
        return data;
    }
}
