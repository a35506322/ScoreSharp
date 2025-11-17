namespace ScoreSharp.API.Modules.SetUp.InternalIP.InsertInternalIP;

[ExampleAnnotation(Name = "[2000]新增行內IP", ExampleType = ExampleType.Request)]
public class 新增行內IP_2000_ReqEx : IExampleProvider<InsertInternalIPRequest>
{
    public InsertInternalIPRequest GetExample()
    {
        InsertInternalIPRequest request = new() { IP = "172.28.234.11", IsActive = "Y" };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增行內IP", ExampleType = ExampleType.Response)]
public class 新增行內IP_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("172.28.234.11", "172.28.234.11");
    }
}

[ExampleAnnotation(Name = "[4002]行內IP-資料已存在", ExampleType = ExampleType.Response)]
public class 新增行內IP資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "172.28.234.11");
    }
}
