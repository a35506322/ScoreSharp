namespace ScoreSharp.API.Modules.SetUp.ProjectCode.DeleteProjectCodeById;

[ExampleAnnotation(Name = "[2000]刪除專案代號", ExampleType = ExampleType.Response)]
public class 刪除專案代號_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("202");
    }
}

[ExampleAnnotation(Name = "[4001]刪除專案代號-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除專案代號查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("307", "307");
    }
}
