namespace ScoreSharp.API.Modules.SetUp.BillDay.InsertIBillDay;

[ExampleAnnotation(Name = "[2000]新增帳單日", ExampleType = ExampleType.Request)]
public class 新增帳單日_2000_ReqEx : IExampleProvider<InsertBillDayRequest>
{
    public InsertBillDayRequest GetExample()
    {
        InsertBillDayRequest request = new() { BillDay = "03", IsActive = "Y" };

        return request;
    }
}

[ExampleAnnotation(Name = "[2000]新增帳單日", ExampleType = ExampleType.Response)]
public class 新增帳單日_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.InsertSuccess("03", "03");
    }
}

[ExampleAnnotation(Name = "[4002]帳單日-資料已存在", ExampleType = ExampleType.Response)]
public class 新增帳單日資料已存在_4002_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.DataAlreadyExists(string.Empty, "03");
    }
}
