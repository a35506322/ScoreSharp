namespace ScoreSharp.API.Modules.Auth.Role.GetRoleById;

[ExampleAnnotation(Name = "[2000]取得角色", ExampleType = ExampleType.Response)]
public class 取得角色_2000_ResEx : IExampleProvider<ResultResponse<GetRoleByIdResponse>>
{
    public ResultResponse<GetRoleByIdResponse> GetExample()
    {
        var data = new GetRoleByIdResponse()
        {
            RoleId = "Admin",
            RoleName = "最高權限管理者",
            IsActive = "Y",
            AddTime = DateTime.Now,
            AddUserId = "ADMIN",
            UpdateTime = DateTime.Now,
            UpdateUserId = "ADMIN",
        };

        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[4001]取得角色-查無此資料", ExampleType = ExampleType.Response)]
public class 取得角色查無此資料_4001_ResEx : IExampleProvider<ResultResponse<GetRoleByIdResponse>>
{
    public ResultResponse<GetRoleByIdResponse> GetExample()
    {
        return ApiResponseHelper.NotFound<GetRoleByIdResponse>(null, "顯示找不到的ID");
    }
}
