namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetZipCodeByAddressInfo;

[ExampleAnnotation(Name = "[2000]查詢郵遞區號", ExampleType = ExampleType.Request)]
public class 查詢郵遞區號_2000_ReqEx : IExampleProvider<GetZipCodeByAddressInfoRequest>
{
    public GetZipCodeByAddressInfoRequest GetExample()
    {
        GetZipCodeByAddressInfoRequest request = new()
        {
            City = "臺北市",
            District = "中正區",
            Road = "八德路一段",
            Number = 1,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]查詢郵遞區號")]
public class 查詢郵遞區號_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success("10058");
    }
}

[ExampleAnnotation(Name = "[4001]查詢郵遞區號-查無資料")]
public class 查詢郵遞區號查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "查無資料");
    }
}
