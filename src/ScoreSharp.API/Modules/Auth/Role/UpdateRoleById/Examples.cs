namespace ScoreSharp.API.Modules.Auth.Role.UpdateRoleById;

[ExampleAnnotation(Name = "[2000]修改角色", ExampleType = ExampleType.Request)]
public class 修改角色_2000_ReqEx : IExampleProvider<UpdateRoleByIdRequest>
{
    public UpdateRoleByIdRequest GetExample()
    {
        UpdateRoleByIdRequest request = new() { IsActive = "Y", RoleName = "Agent" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改角色", ExampleType = ExampleType.Response)]
public class 修改角色_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("Agent", "Agent");
    }
}

[ExampleAnnotation(Name = "[4001]修改角色-查無此資料", ExampleType = ExampleType.Response)]
public class 修改角色查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "Agent");
    }
}

[ExampleAnnotation(Name = "[4003]修改角色-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改角色路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
