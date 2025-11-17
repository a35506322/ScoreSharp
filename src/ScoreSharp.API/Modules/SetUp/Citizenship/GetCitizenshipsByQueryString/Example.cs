namespace ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipsByQueryString;

[ExampleAnnotation(Name = "[2000]取得國籍", ExampleType = ExampleType.Response)]
public class 取得國籍_2000_ResEx : IExampleProvider<ResultResponse<List<GetCitizenshipsByQueryStringResponse>>>
{
    public ResultResponse<List<GetCitizenshipsByQueryStringResponse>> GetExample()
    {
        List<GetCitizenshipsByQueryStringResponse> response = new()
        {
            new GetCitizenshipsByQueryStringResponse
            {
                CitizenshipCode = "TW",
                CitizenshipName = "台灣",
                IsActive = "Y",
            },
            new GetCitizenshipsByQueryStringResponse
            {
                CitizenshipCode = "CH",
                CitizenshipName = "瑞士",
                IsActive = "Y",
            },
        };

        return ApiResponseHelper.Success(response);
    }
}
