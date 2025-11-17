namespace ScoreSharp.API.Modules.Auth.Role.DeleteRoleById;

[ExampleAnnotation(Name = "[2000]刪除角色", ExampleType = ExampleType.Response)]
public class 刪除角色_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4001]刪除角色-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除角色查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[4003]刪除角色-此資源已被使用", ExampleType = ExampleType.Response)]
public class 刪除角色此資源已被使用_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.此資源已被使用<string>(null, "刪除的ID");
    }
}
