namespace ScoreSharp.API.Modules.Auth.Role.InsertRoleAuthById;

[ExampleAnnotation(Name = "[2000]新增單筆角色權限", ExampleType = ExampleType.Request)]
public class 新增單筆角色權限_2000_ReqEx : IExampleProvider<List<InsertRoleAuthByIdRequest>>
{
    public List<InsertRoleAuthByIdRequest> GetExample()
    {
        return new List<InsertRoleAuthByIdRequest>()
        {
            new InsertRoleAuthByIdRequest()
            {
                RoleId = "Admin",
                RouterId = "SetUpBillDay",
                ActionId = "GetBillDayById",
            },
            new InsertRoleAuthByIdRequest()
            {
                RoleId = "Admin",
                RouterId = "SetUpBillDay",
                ActionId = "GetBillDayByQueryString",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]新增單筆角色權限", ExampleType = ExampleType.Response)]
public class 新增單筆角色權限_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.InsertSuccess<string>("Admin", "Admin");
}

[ExampleAnnotation(Name = "[4003]RouterRoleId不符合", ExampleType = ExampleType.Response)]
public class 新增單筆角色權限RouterRoleId不符合_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, "Router RoleId 不符合，請檢查");
}

[ExampleAnnotation(Name = "[4001]查無此RoleId", ExampleType = ExampleType.Response)]
public class 新增單筆角色權限查無此RoleId_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "找不到的RoleId");
}

[ExampleAnnotation(Name = "[4003]ActionId與RoleId不符合", ExampleType = ExampleType.Response)]
public class 新增單筆角色權限ActionId與RoleId不符合_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, "ActionId 與 RoleId 不符合，請檢查");
}
