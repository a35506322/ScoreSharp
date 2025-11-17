namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.InsertAnnualFeeCollectionMethod;

[ExampleAnnotation(Name = "[2000]新增年費收取方式", ExampleType = ExampleType.Request)]
public class 新增年費收取方式_2000_ReqEx : IExampleProvider<InsertAnnualFeeCollectionMethodRequest>
{
    public InsertAnnualFeeCollectionMethodRequest GetExample()
    {
        InsertAnnualFeeCollectionMethodRequest request = new()
        {
            AnnualFeeCollectionCode = "01",
            AnnualFeeCollectionName = "年費收取名稱範例",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增年費收取方式", ExampleType = ExampleType.Response)]
public class 新增年費收取方式_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        string seqno = Ulid.NewUlid().ToString();
        return ApiResponseHelper.InsertSuccess(seqno, seqno);
    }
}

[ExampleAnnotation(Name = "[4002]新增年費收取方式-資料已存在", ExampleType = ExampleType.Response)]
public class 新增年費收取方式資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "1");
    }
}
