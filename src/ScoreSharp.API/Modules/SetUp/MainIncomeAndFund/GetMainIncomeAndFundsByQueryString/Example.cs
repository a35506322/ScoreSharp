namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.GetMainIncomeAndFundsByQueryString;

[ExampleAnnotation(Name = "[2000]取得主要所得及資金來源", ExampleType = ExampleType.Response)]
public class 取得主要所得及資金來源_2000_ResEx : IExampleProvider<ResultResponse<List<GetMainIncomeAndFundsByQueryStringResponse>>>
{
    public ResultResponse<List<GetMainIncomeAndFundsByQueryStringResponse>> GetExample()
    {
        var data = new List<GetMainIncomeAndFundsByQueryStringResponse>
        {
            new GetMainIncomeAndFundsByQueryStringResponse
            {
                MainIncomeAndFundCode = "2",
                MainIncomeAndFundName = "薪資所得",
                IsActive = "Y",
                AddTime = DateTime.Now,
                AddUserId = "ADMIN",
                UpdateTime = DateTime.Now,
                UpdateUserId = "ADMIN",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
