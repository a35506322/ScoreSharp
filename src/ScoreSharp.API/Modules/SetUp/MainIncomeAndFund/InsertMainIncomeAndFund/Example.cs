namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.InsertMainIncomeAndFund;

[ExampleAnnotation(Name = "[2000]新增主要所得及資金來源", ExampleType = ExampleType.Request)]
public class 新增主要所得及資金來源_2000_ReqEx : IExampleProvider<InsertMainIncomeAndFundRequest>
{
    public InsertMainIncomeAndFundRequest GetExample()
    {
        InsertMainIncomeAndFundRequest request = new()
        {
            MainIncomeAndFundCode = "8",
            MainIncomeAndFundName = "閒置資金",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增主要所得及資金來源", ExampleType = ExampleType.Response)]
public class 新增主要所得及資金來源_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("8", "8");
    }
}

[ExampleAnnotation(Name = "[4002]新增主要所得及資金來源-資料已存在", ExampleType = ExampleType.Response)]
public class 新增主要所得及資金來源資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "1");
    }
}
