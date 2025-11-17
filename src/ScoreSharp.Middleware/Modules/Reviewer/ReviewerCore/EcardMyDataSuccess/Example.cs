using ScoreSharp.Common.Helpers;
using ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardSupplement;

namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardMyDataSuccess;

[ExampleAnnotation(Name = "[0000]ECARD_MyData取件成功-匯入成功", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件成功_匯入成功_0000_ReqEx : IExampleProvider<EcardMyDataSuccessRequest>
{
    public EcardMyDataSuccessRequest GetExample()
    {
        EcardMyDataSuccessRequest request = new()
        {
            ID = "R120667319",
            ApplyNo = "12345678",
            AppendixFileName_01 = "income1.jpg",
            AppendixFileName_02 = "income2.png",
            AppendixFileName_03 = "income3.gif",
            AppendixFileName_04 = "income4.jpeg",
            AppendixFileName_05 = "income5.bmp",
            AppendixFileName_06 = null,
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0000]ECARD_MyData取件成功-匯入成功", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件成功_匯入成功_0000_ResEx : IExampleProvider<EcardMyDataSuccessResponse>
{
    public EcardMyDataSuccessResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0000\",\"RESULT\": \"匯入成功\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataSuccessResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0001]ECARD_MyData取件成功-必要欄位為空值", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件成功_必要欄位為空值_0001_ReqEx : IExampleProvider<EcardMyDataSuccessRequest>
{
    public EcardMyDataSuccessRequest GetExample()
    {
        EcardMyDataSuccessRequest request = new()
        {
            ID = "R120667319",
            ApplyNo = "",
            AppendixFileName_01 = "income1.jpg",
            AppendixFileName_02 = "income2.png",
            AppendixFileName_03 = "income3.gif",
            AppendixFileName_04 = "income4.jpeg",
            AppendixFileName_05 = "income5.bmp",
            AppendixFileName_06 = null,
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0001]ECARD_MyData取件成功-必要欄位為空值", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件成功_必要欄位為空值_0001_ResEx : IExampleProvider<EcardMyDataSuccessResponse>
{
    public EcardMyDataSuccessResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0001\",\"RESULT\": \"必要欄位為空值\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataSuccessResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0002]ECARD_MyData取件成功-長度過長", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件成功_長度過長_0002_ReqEx : IExampleProvider<EcardMyDataSuccessRequest>
{
    public EcardMyDataSuccessRequest GetExample()
    {
        EcardMyDataSuccessRequest request = new()
        {
            ID = "R120667319",
            ApplyNo = "12345678",
            AppendixFileName_01 = "income123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123.jpg",
            AppendixFileName_02 = "income2.png",
            AppendixFileName_03 = "income3.gif",
            AppendixFileName_04 = "income4.jpeg",
            AppendixFileName_05 = "income5.bmp",
            AppendixFileName_06 = null,
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0002]ECARD_MyData取件成功-長度過長", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件成功_長度過長_0002_ResEx : IExampleProvider<EcardMyDataSuccessResponse>
{
    public EcardMyDataSuccessResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0002\",\"RESULT\": \"長度過長\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataSuccessResponse>(jsonString);
        return data;
    }
}

[ExampleAnnotation(Name = "[0003]ECARD_MyData取件成功-其它異常訊息_查無申請書資料", ExampleType = ExampleType.Request)]
public class ECARD_MyData取件成功_其它異常訊息_查無申請書資料_0003_ReqEx : IExampleProvider<EcardMyDataSuccessRequest>
{
    public EcardMyDataSuccessRequest GetExample()
    {
        EcardMyDataSuccessRequest request = new()
        {
            ID = "C123456789",
            ApplyNo = "12345678",
            AppendixFileName_01 = "income1.jpg",
            AppendixFileName_02 = "income2.png",
            AppendixFileName_03 = "income3.gif",
            AppendixFileName_04 = "income4.jpeg",
            AppendixFileName_05 = "income5.bmp",
            AppendixFileName_06 = null,
            MyDataNo = "12345678",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[0003]ECARD_MyData取件成功-其它異常訊息_查無申請書資料", ExampleType = ExampleType.Response)]
public class ECARD_MyData取件成功_其它異常訊息_查無申請書資料_0003_ResEx : IExampleProvider<EcardMyDataSuccessResponse>
{
    public EcardMyDataSuccessResponse GetExample()
    {
        string jsonString = "{\"ID\": \"0003\",\"RESULT\": \"其它異常訊息\"}";
        var data = JsonHelper.反序列化物件不分大小寫<EcardMyDataSuccessResponse>(jsonString);
        return data;
    }
}
