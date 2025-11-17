namespace ScoreSharp.API.Modules.SetUp.ProjectCode.UpdateProjectCodeById;

[ExampleAnnotation(Name = "[2000]修改專案代號", ExampleType = ExampleType.Request)]
public class 修改專案代號_2000_ReqEx : IExampleProvider<UpdateProjectCodeByIdRequest>
{
    public UpdateProjectCodeByIdRequest GetExample()
    {
        UpdateProjectCodeByIdRequest request = new()
        {
            ProjectCode = "202",
            ProjectName = "徵審系統",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改專案代號", ExampleType = ExampleType.Response)]
public class 修改專案代號_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("202", "202");
    }
}

[ExampleAnnotation(Name = "[4001]修改專案代號-查無此資料", ExampleType = ExampleType.Response)]
public class 修改專案代號查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("307", "307");
    }
}

[ExampleAnnotation(Name = "[4003]修改專案代號-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改專案代號路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
