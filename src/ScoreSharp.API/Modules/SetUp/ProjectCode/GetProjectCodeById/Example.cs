namespace ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodeById;

[ExampleAnnotation(Name = "[2000]取得專案代號", ExampleType = ExampleType.Response)]
public class 取得專案代號_2000_ResEx : IExampleProvider<ResultResponse<GetProjectCodeByIdResponse>>
{
    public ResultResponse<GetProjectCodeByIdResponse> GetExample()
    {
        GetProjectCodeByIdResponse response = new()
        {
            ProjectCode = "201",
            ProjectName = "催繳系統",
            IsActive = "Y",
            AddUserId = "ADMIN",
            AddTime = DateTime.Now,
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };
        return ApiResponseHelper.Success(response);
    }
}

[ExampleAnnotation(Name = "[4001]取得專案代號-查無此資料", ExampleType = ExampleType.Response)]
public class 取得專案代號查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetProjectCodeByIdResponse>>
{
    public ResultResponse<GetProjectCodeByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetProjectCodeByIdResponse>(null, "307");
    }
}
