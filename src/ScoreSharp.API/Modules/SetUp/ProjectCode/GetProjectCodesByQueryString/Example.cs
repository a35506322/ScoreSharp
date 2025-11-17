namespace ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodesByQueryString;

[ExampleAnnotation(Name = "[2000]取得專案代號", ExampleType = ExampleType.Response)]
public class 取得專案代號_2000_ResEx : IExampleProvider<ResultResponse<List<GetProjectCodesByQueryStringResponse>>>
{
    public ResultResponse<List<GetProjectCodesByQueryStringResponse>> GetExample()
    {
        var data = new List<GetProjectCodesByQueryStringResponse>
        {
            new GetProjectCodesByQueryStringResponse
            {
                ProjectCode = "201",
                ProjectName = "催收系統",
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
