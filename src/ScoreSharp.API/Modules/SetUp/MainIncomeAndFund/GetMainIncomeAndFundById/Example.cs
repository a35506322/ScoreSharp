namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundById;

[ExampleAnnotation(Name = "[2000]取得主要所得及資金來源", ExampleType = ExampleType.Response)]
public class 取得主要所得及資金來源_2000_ResEx : IExampleProvider<ResultResponse<GetMainIncomeAndFundByIdResponse>>
{
    public ResultResponse<GetMainIncomeAndFundByIdResponse> GetExample()
    {
        GetMainIncomeAndFundByIdResponse response = new()
        {
            MainIncomeAndFundCode = "2",
            MainIncomeAndFundName = "薪資所得",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得主要所得及資金來源-查無此資料", ExampleType = ExampleType.Response)]
public class 取得主要所得及資金來源查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetMainIncomeAndFundByIdResponse>>
{
    public ResultResponse<GetMainIncomeAndFundByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetMainIncomeAndFundByIdResponse>(null, "13");
    }
}
