namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.UpdateAnnualFeeCollectionMethodById;

[ExampleAnnotation(Name = "[2000]更新年費收取方式", ExampleType = ExampleType.Request)]
public class 更新年費收取方式_2000_ReqEx : IExampleProvider<UpdateAnnualFeeCollectionMethodByIdRequest>
{
    public UpdateAnnualFeeCollectionMethodByIdRequest GetExample()
    {
        return new UpdateAnnualFeeCollectionMethodByIdRequest
        {
            AnnualFeeCollectionCode = "01",
            AnnualFeeCollectionName = "自動扣款",
            IsActive = "Y",
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新年費收取方式成功", ExampleType = ExampleType.Response)]
public class 更新年費收取方式_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("01", "01");
    }
}

[ExampleAnnotation(Name = "[4001]更新年費收取方式查無資料", ExampleType = ExampleType.Response)]
public class 更新年費收取方式查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "12");
    }
}

[ExampleAnnotation(Name = "[4003]更新年費收取方式路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 更新年費收取方式路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
