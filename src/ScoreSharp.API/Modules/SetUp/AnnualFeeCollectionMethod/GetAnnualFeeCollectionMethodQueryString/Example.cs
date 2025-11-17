namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodQueryString;

[ExampleAnnotation(Name = "[2000]取得年費收取方式", ExampleType = ExampleType.Response)]
public class 取得年費收取方式_2000_ResEx : IExampleProvider<ResultResponse<List<GetAnnualFeeCollectionMethodQueryStringResponse>>>
{
    public ResultResponse<List<GetAnnualFeeCollectionMethodQueryStringResponse>> GetExample()
    {
        var data = new List<GetAnnualFeeCollectionMethodQueryStringResponse>
        {
            new GetAnnualFeeCollectionMethodQueryStringResponse
            {
                AnnualFeeCollectionCode = "04",
                AnnualFeeCollectionName = "按月分期",
                IsActive = "Y",
                AddTime = new DateTime(2025, 1, 1, 10, 30, 0),
                AddUserId = "ADMIN",
                UpdateTime = new DateTime(2025, 10, 13, 14, 20, 0),
                UpdateUserId = "ADMIN",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
