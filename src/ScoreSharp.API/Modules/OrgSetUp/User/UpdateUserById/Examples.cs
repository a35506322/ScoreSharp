namespace ScoreSharp.API.Modules.OrgSetUp.User.UpdateUserById;

[ExampleAnnotation(Name = "[2000]修改使用者", ExampleType = ExampleType.Request)]
public class 修改使用者_2000_ReqEx : IExampleProvider<UpdateUserByIdRequest>
{
    public UpdateUserByIdRequest GetExample()
    {
        UpdateUserByIdRequest request = new UpdateUserByIdRequest
        {
            UserId = "SuperAdmin",
            IsActive = "Y",
            CaseDispatchGroups = [CaseDispatchGroup.台北徵審科, CaseDispatchGroup.高雄徵審科],
            OrganizeCode = "DP",
            UserName = "HAHA",
            RoleId = ["Admin", "Reviewer"],
            EmployeeNo = null,
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改使用者", ExampleType = ExampleType.Response)]
public class 修改使用者_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("SuperAdmin", "SuperAdmin");
    }
}

[ExampleAnnotation(Name = "[4001]修改使用者-查無此資料", ExampleType = ExampleType.Response)]
public class 修改使用者查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "Super");
    }
}

[ExampleAnnotation(Name = "[4003]修改使用者-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改使用者路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}

[ExampleAnnotation(Name = "[4003]修改使用者-非AD 請輸入使用者名稱", ExampleType = ExampleType.Response)]
public class 修改使用者非AD請輸入使用者名稱_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>(null, "非AD 請輸入使用者名稱");
    }
}

[ExampleAnnotation(Name = "[4000]修改使用者-自創帳號密碼不能為空", ExampleType = ExampleType.Response)]
public class 修改使用者自創帳號密碼不能為空_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString = "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"mima\": [\"密碼不能為空\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4000]修改使用者-自創帳號密碼長度至少8個字", ExampleType = ExampleType.Response)]
public class 修改使用者自創帳號密碼長度至少8個字_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString = "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"mima\": [\"密碼長度至少8個字\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}
