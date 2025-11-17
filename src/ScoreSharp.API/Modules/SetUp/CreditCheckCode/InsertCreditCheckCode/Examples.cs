namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.InsertCreditCheckCode;

[ExampleAnnotation(Name = "[2000]新增徵信代碼", ExampleType = ExampleType.Request)]
public class 新增徵信代碼_2000_ReqEx : IExampleProvider<InsertCreditCheckCodeRequest>
{
    public InsertCreditCheckCodeRequest GetExample()
    {
        InsertCreditCheckCodeRequest request = new()
        {
            CreditCheckCode = "A01",
            CreditCheckCodeName = "聯徵辦卡",
            IsActive = "Y",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增徵信代碼", ExampleType = ExampleType.Response)]
public class 新增徵信代碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("A01", "A01");
    }
}

[ExampleAnnotation(Name = "[4002]新增徵信代碼-資料已存在", ExampleType = ExampleType.Response)]
public class 新增徵信代碼資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists<string>(null, "A01");
    }
}
