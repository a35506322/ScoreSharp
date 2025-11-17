namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.UpdateCreditCheckCodeById;

[ExampleAnnotation(Name = "[2000]修改徵信代碼", ExampleType = ExampleType.Request)]
public class 修改徵信代碼_2000_ReqEx : IExampleProvider<UpdateCreditCheckCodeByIdRequest>
{
    public UpdateCreditCheckCodeByIdRequest GetExample()
    {
        UpdateCreditCheckCodeByIdRequest request = new()
        {
            IsActive = "Y",
            CreditCheckCode = "A02",
            CreditCheckCodeName = "原持卡人(依原額度核發)",
        };
        return request;
    }
}

[ExampleAnnotation(Name = "[2000]修改徵信代碼", ExampleType = ExampleType.Response)]
public class 修改徵信代碼_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess("A02", "A02");
    }
}

[ExampleAnnotation(Name = "[4001]修改徵信代碼-查無此資料", ExampleType = ExampleType.Response)]
public class 修改徵信代碼查無此資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "A06");
    }
}

[ExampleAnnotation(Name = "[4003]修改徵信代碼-路由與Req比對錯誤", ExampleType = ExampleType.Response)]
public class 修改徵信代碼路由與Req比對錯誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
