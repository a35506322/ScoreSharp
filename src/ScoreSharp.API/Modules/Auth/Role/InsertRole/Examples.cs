namespace ScoreSharp.API.Modules.Auth.Role.InsertRole;

[ExampleAnnotation(Name = "[2000]新增角色", ExampleType = ExampleType.Request)]
public class 新增角色_2000_ReqEx : IExampleProvider<InsertRoleRequest>
{
    public InsertRoleRequest GetExample()
    {
        InsertRoleRequest request = new()
        {
            RoleName = "顧問",
            RoleId = "Consultant",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增角色", ExampleType = ExampleType.Response)]
public class 新增角色_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("Consultant", "Consultant");
    }
}

[ExampleAnnotation(Name = "[4002]新增角色-資料已存在", ExampleType = ExampleType.Response)]
public class 新增角色資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "Consultant");
    }
}
