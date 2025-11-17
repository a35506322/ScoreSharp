namespace ScoreSharp.API.Modules.SetUp.InternalIP.DeleteInternalIPById;

[ExampleAnnotation(Name = "[4001]刪除單筆行內IP-查無此資料", ExampleType = ExampleType.Response)]
public class 刪除行內IP查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "刪除的ID");
    }
}

[ExampleAnnotation(Name = "[2000]刪除單筆行內IP", ExampleType = ExampleType.Response)]
public class 刪除行內IP_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DeleteByIdSuccess("刪除的ID");
    }
}
