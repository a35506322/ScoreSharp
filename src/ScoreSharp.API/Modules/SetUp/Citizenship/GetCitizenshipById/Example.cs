namespace ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipById;

[ExampleAnnotation(Name = "[2000]取得國籍", ExampleType = ExampleType.Response)]
public class 取得國籍_2000_ResEx : IExampleProvider<ResultResponse<GetCitizenshipByIdResponse>>
{
    public ResultResponse<GetCitizenshipByIdResponse> GetExample()
    {
        GetCitizenshipByIdResponse response = new()
        {
            CitizenshipCode = "TW",
            CitizenshipName = "台灣",
            IsActive = "Y",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得國籍-查無資料", ExampleType = ExampleType.Response)]
public class 取得國籍查無資料_4001_ResEx : IExampleProvider<ResultResponse<GetCitizenshipByIdResponse>>
{
    public ResultResponse<GetCitizenshipByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetCitizenshipByIdResponse>(null, "TKG");
    }
}
