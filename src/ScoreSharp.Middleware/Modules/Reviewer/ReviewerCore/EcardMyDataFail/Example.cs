using ScoreSharp.Common.Helpers;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataFail;

[ExampleAnnotation(Name = "[0000]ECARD_MyData取件失敗-匯入成功", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件失敗_匯入成功_0000_ReqEx : IExampleProvider<EcardMyDataFailRequest>
{
    public EcardMyDataFailRequest GetExample()
    {
        EcardMyDataFailRequest request = new()
        {
            ID = "R120667319",
            ApplyNo = "12345678",
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0000]ECARD_MyData取件失敗-匯入成功", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件失敗_匯入成功_0000_ResEx : IExampleProvider<EcardMyDataFailResponse>
{
    public EcardMyDataFailResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0000\",\"RESULT\": \"匯入成功\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataFailResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0001]ECARD_MyData取件失敗-必要欄位為空值", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件失敗_必要欄位為空值_0001_ReqEx : IExampleProvider<EcardMyDataFailRequest>
{
    public EcardMyDataFailRequest GetExample()
    {
        EcardMyDataFailRequest request = new()
        {
            ID = "R120667319",
            ApplyNo = "",
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0001]ECARD_MyData取件失敗-必要欄位為空值", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件失敗_必要欄位為空值_0001_ResEx : IExampleProvider<EcardMyDataFailResponse>
{
    public EcardMyDataFailResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0001\",\"RESULT\": \"必要欄位為空值\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataFailResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0002]ECARD_MyData取件失敗-長度過長", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件失敗_長度過長_0002_ReqEx : IExampleProvider<EcardMyDataFailRequest>
{
    public EcardMyDataFailRequest GetExample()
    {
        EcardMyDataFailRequest request = new()
        {
            ID = "R120667319",
            ApplyNo = "123456789012345",
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0002]ECARD_MyData取件失敗-長度過長", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件失敗_長度過長_0002_ResEx : IExampleProvider<EcardMyDataFailResponse>
{
    public EcardMyDataFailResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0002\",\"RESULT\": \"長度過長\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataFailResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0003]ECARD_MyData取件失敗-其它異常訊息_查無申請書資料", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件失敗_其它異常訊息_查無申請書資料_0003_ReqEx : IExampleProvider<EcardMyDataFailRequest>
{
    public EcardMyDataFailRequest GetExample()
    {
        EcardMyDataFailRequest request = new()
        {
            ID = "C123456789",
            ApplyNo = "12345678",
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0003]ECARD_MyData取件失敗-其它異常訊息_查無申請書資料", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件失敗_其它異常訊息_查無申請書資料_0003_ResEx : IExampleProvider<EcardMyDataFailResponse>
{
    public EcardMyDataFailResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0003\",\"RESULT\": \"其它異常訊息\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataFailResponse>(jsonString);
        return data;
    }
}
