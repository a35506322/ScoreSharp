namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.InsertPromotionUnit;

[ExampleAnnotation(Name = "[2000]新增推廣單位", ExampleType = ExampleType.Request)]
public class 新增推廣單位_2000_ReqEx : IExampleProvider<InsertPromotionUnitRequest>
{
    public InsertPromotionUnitRequest GetExample()
    {
        InsertPromotionUnitRequest request = new()
        {
            PromotionUnitCode = "100",
            PromotionUnitName = "聯邦文教推廣",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增推廣單位", ExampleType = ExampleType.Response)]
public class 新增推廣單位_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("100", "100");
    }
}

[ExampleAnnotation(Name = "[4002]新增推廣單位-資料已存在", ExampleType = ExampleType.Response)]
public class 新增推廣單位資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "100");
    }
}
