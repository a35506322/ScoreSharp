namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.UpdateMainIncomeAndFundById;

[ExampleAnnotation(Name = "[2000]修改主要所得及資金來源", ExampleType = ExampleType.Request)]
public class 修改主要所得及資金來源_2000_ReqEx : IExampleProvider<UpdateMainIncomeAndFundByIdRequest>
{
    public UpdateMainIncomeAndFundByIdRequest GetExample()
    {
        UpdateMainIncomeAndFundByIdRequest request = new()
        {
            MainIncomeAndFundCode = "1",
            MainIncomeAndFundName = "經營事業收入",
            IsActive = "N",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改主要所得及資金來源", ExampleType = ExampleType.Response)]
public class 修改主要所得及資金來源_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]修改主要所得及資金來源-查無此資料", ExampleType = ExampleType.Response)]
public class 修改主要所得及資金來源查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "18");
    }
}

[ExampleAnnotation(Name = "[4003]修改主要所得及資金來源-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改主要所得及資金來源路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
