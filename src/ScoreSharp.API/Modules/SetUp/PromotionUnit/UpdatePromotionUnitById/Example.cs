namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.UpdatePromotionUnitById;

[ExampleAnnotation(Name = "[2000]修改推廣單位", ExampleType = ExampleType.Request)]
public class 修改推廣單位_2000_ReqEx : IExampleProvider<UpdatePromotionUnitByIdRequest>
{
    public UpdatePromotionUnitByIdRequest GetExample()
    {
        UpdatePromotionUnitByIdRequest request = new()
        {
            PromotionUnitCode = "200",
            PromotionUnitName = "智能客服",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改推廣單位", ExampleType = ExampleType.Response)]
public class 修改推廣單位_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("200", "200");
    }
}

[ExampleAnnotation(Name = "[4001]修改推廣單位-查無此資料", ExampleType = ExampleType.Response)]
public class 修改推廣單位查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("150", "150");
    }
}

[ExampleAnnotation(Name = "[4003]修改推廣單位-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改推廣單位路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
