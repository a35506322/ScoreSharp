namespace ScoreSharp.API.Modules.OrgSetUp.User.Login;

[ExampleAnnotation(Name = "[2000]登入成功-Admin", ExampleType = ExampleType.Request)]
public class 登入成功Admin_2000_ReqEx : IExampleProvider<LoginRequest>
{
    public LoginRequest GetExample()
    {
        LoginRequest request = new() { UserId = "lijungjhuang", Mima = "51350505" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]登入成功-徵審人員", ExampleType = ExampleType.Request)]
public class 登入成功徵審人員_2000_ReqEx : IExampleProvider<LoginRequest>
{
    public LoginRequest GetExample()
    {
        LoginRequest request = new() { UserId = "arthurlin" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]登入成功-資訊科人員", ExampleType = ExampleType.Request)]
public class 登入成功資訊科人員_2000_ReqEx : IExampleProvider<LoginRequest>
{
    public LoginRequest GetExample()
    {
        LoginRequest request = new() { UserId = "linachen", Mima = "12345678" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]登入成功", ExampleType = ExampleType.Response)]
public class 登入成功_2000_ResEx : IExampleProvider<ResultResponse<LoginResponse>>
{
    public ResultResponse<LoginResponse> GetExample()
    {
        int expireMinutes = 480;
        var response = new LoginResponse { Token = "token", ExpireMinutes = expireMinutes };
        return ApiResponseHelper.Success(response, "登入成功");
    }
}

[ExampleAnnotation(Name = "[4003]登入帳密有誤", ExampleType = ExampleType.Response)]
public class 登入帳密有誤_4003_ResEx : IExampleProvider<ResultResponse<LoginResponse>>
{
    public ResultResponse<LoginResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, "帳密有誤，剩餘次數: 3");
    }
}

[ExampleAnnotation(Name = "[4003]使用者未啟用", ExampleType = ExampleType.Response)]
public class 使用者未啟用_4003_ResEx : IExampleProvider<ResultResponse<LoginResponse>>
{
    public ResultResponse<LoginResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<LoginResponse>(null, "使用者未啟用");
    }
}
