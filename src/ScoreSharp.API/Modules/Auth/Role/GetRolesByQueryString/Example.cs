namespace ScoreSharp.API.Modules.Auth.Role.GetRolesByQueryString;

[ExampleAnnotation(Name = "[2000]取得角色", ExampleType = ExampleType.Response)]
public class 取得角色_2000_ResEx : IExampleProvider<ResultResponse<List<GetRolesByQueryStringResponse>>>
{
    public ResultResponse<List<GetRolesByQueryStringResponse>> GetExample()
    {
        var data = new List<GetRolesByQueryStringResponse>
        {
            new GetRolesByQueryStringResponse
            {
                RoleId = "Reviewer",
                RoleName = "徵審人員",
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
