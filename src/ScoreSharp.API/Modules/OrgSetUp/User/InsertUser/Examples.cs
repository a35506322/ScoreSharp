namespace ScoreSharp.API.Modules.OrgSetUp.User.InsertUser;

[ExampleAnnotation(Name = "[2000]新增使用者", ExampleType = ExampleType.Request)]
public class 新增使用者_2000_ReqEx : IExampleProvider<InsertUserRequest>
{
    public InsertUserRequest GetExample()
    {
        InsertUserRequest request = new()
        {
            UserId = "dp3a",
            IsActive = "Y",
            RoleId = ["Admin", "Reviewer"],
            IsAD = "Y",
            Mima = null,
            OrganizeCode = "DP",
            UserName = "HAHA",
            CaseDispatchGroups = [CaseDispatchGroup.台北徵審科],
            EmployeeNo = null,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增使用者", ExampleType = ExampleType.Response)]
public class 新增使用者_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("dp3a", "dp3a，Role :Admin");
    }
}

[ExampleAnnotation(Name = "[4002]新增使用者-資料已存在", ExampleType = ExampleType.Response)]
public class 新增使用者資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "TH3I");
    }
}

[ExampleAnnotation(Name = "[4003]使用者資訊與 AD Server 不符合", ExampleType = ExampleType.Response)]
public class 使用者資訊與ADServer不符合_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "此使用者資訊與 AD Server 不符合，請重新確認");
    }
}

[ExampleAnnotation(Name = "[4000]新增使用者-自創帳號密碼不能為空", ExampleType = ExampleType.Response)]
public class 新增使用者自創帳號密碼不能為空_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString = "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"mima\": [\"密碼不能為空\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4000]新增使用者-自創帳號密碼長度至少8個字", ExampleType = ExampleType.Response)]
public class 新增使用者自創帳號密碼長度至少8個字_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString = "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"mima\": [\"密碼長度至少8個字\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}
