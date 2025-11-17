namespace ScoreSharp.API.Modules.SetUp.BillDay.UpdateBillDayForIsActiveById;

[ExampleAnnotation(Name = "[2000]修改帳單日狀態", ExampleType = ExampleType.Request)]
public class 修改帳單日狀態_2000_ReqEx : IExampleProvider<UpdateBillDayForIsActiveByIdRequest>
{
    public UpdateBillDayForIsActiveByIdRequest GetExample()
    {
        UpdateBillDayForIsActiveByIdRequest request = new() { BillDay = "01", IsActive = "Y" };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改帳單日狀態", ExampleType = ExampleType.Response)]
public class 修改帳單日狀態_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("01", "01");
    }
}

[ExampleAnnotation(Name = "[4001]修改帳單日狀態-查無此資料", ExampleType = ExampleType.Response)]
public class 修改帳單日狀態查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "25");
    }
}

[ExampleAnnotation(Name = "[4003]修改帳單日狀態-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改帳單日狀態路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
