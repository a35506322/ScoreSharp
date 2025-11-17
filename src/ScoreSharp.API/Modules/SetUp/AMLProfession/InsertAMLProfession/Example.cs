namespace ScoreSharp.API.Modules.SetUp.AMLProfession.InsertAMLProfession;

[ExampleAnnotation(Name = "[2000]新增AML職級別", ExampleType = ExampleType.Request)]
public class 新增AML職級別_2000_ReqEx : IExampleProvider<InsertAMLProfessionRequest>
{
    public InsertAMLProfessionRequest GetExample()
    {
        InsertAMLProfessionRequest request = new()
        {
            AMLProfessionCode = "4",
            Version = "20230101",
            AMLProfessionName = "金融/保險/第三方支付/電支或電票/當鋪",
            AMLProfessionCompareResult = "Y",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增AML職級別", ExampleType = ExampleType.Response)]
public class 新增AML職級別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string seqno = Ulid.NewUlid().ToString();
        return ApiResponseHelper.InsertSuccess(seqno, seqno);
    }
}

[ExampleAnnotation(Name = "[4002]新增AML職級別-資料已存在", ExampleType = ExampleType.Response)]
public class 新增AML職級別資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "01J5779A27GEZ7RPQVHHANSAVG");
    }
}

[ExampleAnnotation(Name = "[4000]新增AML職級別-版本不符合格式", ExampleType = ExampleType.Response)]
public class 新增AML職級別版本不符合格式_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString = "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"版本\": [\"不符合格式\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}
