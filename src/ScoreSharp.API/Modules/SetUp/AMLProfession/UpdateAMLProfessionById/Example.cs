namespace ScoreSharp.API.Modules.SetUp.AMLProfession.UpdateAMLProfessionById;

[ExampleAnnotation(Name = "[2000]修改AML職業別", ExampleType = ExampleType.Request)]
public class 修改AML職業別_2000_ReqEx : IExampleProvider<UpdateAMLProfessionByIdRequest>
{
    public UpdateAMLProfessionByIdRequest GetExample()
    {
        UpdateAMLProfessionByIdRequest request = new()
        {
            SeqNo = "01J57D7NVXFHHD4QSZQDABTH3A",
            AMLProfessionCode = "20",
            Version = "20240101",
            AMLProfessionName = "珠寶藝術品/菸酒",
            AMLProfessionCompareResult = "Y",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改AML職業別", ExampleType = ExampleType.Response)]
public class 修改AML職業別_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("01J57D7NVXFHHD4QSZQDABTH3A", "01J57D7NVXFHHD4QSZQDABTH3A");
    }
}

[ExampleAnnotation(Name = "[4001]修改AML職業別-查無此資料", ExampleType = ExampleType.Response)]
public class 修改AML職業別查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>("01J57D7NVXFHHD4QSZQDABTH3B", "01J57D7NVXFHHD4QSZQDABTH3B");
    }
}

[ExampleAnnotation(Name = "[4003]修改AML職業別-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改AML職業別路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>("01J57D7NVXFHHD4QSZQDABTH3B");
    }
}

[ExampleAnnotation(Name = "[4000]修改AML職業別-版本不符合格式", ExampleType = ExampleType.Response)]
public class 修改AML職業別版本不符合格式_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString = "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"版本\": [\"不符合格式\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}
