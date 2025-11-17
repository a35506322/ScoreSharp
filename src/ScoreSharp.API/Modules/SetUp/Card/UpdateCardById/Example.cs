namespace ScoreSharp.API.Modules.SetUp.Card.UpdateCardById;

[ExampleAnnotation(Name = "[2000]修改信用卡卡片種類", ExampleType = ExampleType.Request)]
public class 修改信用卡卡片種類_2000_ReqEx : IExampleProvider<UpdateCardByIdRequest>
{
    public UpdateCardByIdRequest GetExample()
    {
        UpdateCardByIdRequest request = new()
        {
            BINCode = "35665765",
            CardCode = "JST65",
            CardName = "全國悠遊晶緻卡",
            CardCategory = CardCategory.一般發卡,
            SampleRejectionLetter = SampleRejectionLetter.拒件函_信用卡,
            DefaultBillDay = "01",
            SaleLoanCategory = SaleLoanCategory.其他,
            DefaultDiscount = "1999",
            IsActive = "N",
            PrimaryCardQuotaUpperlimit = 8000000,
            PrimaryCardQuotaLowerlimit = 10000,
            PrimaryCardYearUpperlimit = 99,
            PrimaryCardYearLowerlimit = 18,
            SupplementaryCardQuotaUpperlimit = 8000000,
            SupplementaryCardQuotaLowerlimit = 10000,
            SupplementaryCardYearUpperlimit = 99,
            SupplementaryCardYearLowerlimit = 15,
            IsCARDPAUnderLimit = "N",
            CARDPACQuotaLimit = 20,
            IsApplyAdditionalCard = "Y",
            IsIndependentCard = "Y",
            IsIVRvCTIQuery = "Y",
            IsCITSCard = "N",
            IsQuickCardIssuance = "Y",
            IsTicket = "Y",
            IsJointGroup = "Y",
            OptionalCardPromotions = ["1999", "1991", "1992", "1993"],
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改信用卡卡片種類", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("0003", "0003");
    }
}

[ExampleAnnotation(Name = "[4001]修改信用卡卡片種類-查無資料", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "0043");
    }
}

[ExampleAnnotation(Name = "[4003]修改信用卡卡片種類-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改信用卡卡片種類-查無帳單日資料", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類查無帳單日資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無帳單日資料，請檢查");
    }
}

[ExampleAnnotation(Name = "[4003]修改信用卡卡片種類-查無優惠辦法資料", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類查無優惠辦法資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無優惠辦法資料，請檢查");
    }
}

[ExampleAnnotation(Name = "[4000]修改信用卡卡片種類-預設優惠不在可選名單中", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類預設優惠不在可選名單中_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"預設優惠辦法\": [\"不在可選優惠辦法名單中\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[4000]修改信用卡卡片種類-可選優惠至少一個", ExampleType = ExampleType.Response)]
public class 修改信用卡卡片種類可選優惠至少一個_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"可選優惠辦法\": [\"可選優惠辦法至少為一個\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);
        return data;
    }
}
