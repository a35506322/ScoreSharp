namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodId;

[ExampleAnnotation(Name = "[2000]取得年費收取方式", ExampleType = ExampleType.Response)]
public class 取得年費收取方式_2000_ResEx : IExampleProvider<ResultResponse<GetAnnualFeeCollectionMethodIdResponse>>
{
    public ResultResponse<GetAnnualFeeCollectionMethodIdResponse> GetExample()
    {
        GetAnnualFeeCollectionMethodIdResponse response = new()
        {
            AnnualFeeCollectionCode = "04",
            AnnualFeeCollectionName = "按月分期",
            IsActive = "Y",
            AddTime = DateTime.Now,
            AddUserId = "ADMIN",
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得年費收取方式-查無此資料", ExampleType = ExampleType.Response)]
public class 取得年費收取方式查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetAnnualFeeCollectionMethodIdResponse>>
{
    public ResultResponse<GetAnnualFeeCollectionMethodIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetAnnualFeeCollectionMethodIdResponse>(null, "12");
    }
}
