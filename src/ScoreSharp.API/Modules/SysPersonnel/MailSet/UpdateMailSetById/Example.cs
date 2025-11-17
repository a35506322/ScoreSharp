namespace ScoreSharp.API.Modules.SysPersonnel.MailSet.UpdateMailSetById;

[ExampleAnnotation(Name = "[2000]更新單筆郵件設定", ExampleType = ExampleType.Request)]
public class 更新單筆郵件設定_2000_ReqEx : IExampleProvider<UpdateMailSetByIdRequest>
{
    public UpdateMailSetByIdRequest GetExample()
    {
        return new UpdateMailSetByIdRequest()
        {
            SeqNo = 1,
            SystemErrorLog_Template = "TestEmail",
            SystemErrorLog_To = "testmail@company.com",
            SystemErrorLog_Title = "Test Email",
            GuoLuKaCheckFailLog_Template = "GuoLuKaCheckFail.cshtml",
            GuoLuKaCheckFailLog_To = "lijungjhuang@uitc.com.tw",
            GuoLuKaCheckFailLog_Title = "國旅卡客戶檢核排程異常Notice",
            KYCErrorLog_Template = "KYCErrorLog/KYCErrorLog.cshtml",
            KYCErrorLog_To = "lijungjhuang@uitc.com.tw",
            KYCErrorLog_Title = "KYC錯誤通知",
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆郵件設定-修改成功", ExampleType = ExampleType.Response)]
public class 更新單筆郵件設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆郵件設定-查無資料", ExampleType = ExampleType.Request)]
public class 更新單筆郵件設定查無資料_4001_ReqEx : IExampleProvider<UpdateMailSetByIdRequest>
{
    public UpdateMailSetByIdRequest GetExample()
    {
        return new UpdateMailSetByIdRequest
        {
            SeqNo = 2,
            SystemErrorLog_Template = "EmailTemplate",
            SystemErrorLog_To = "testmail@company.com.tw",
            SystemErrorLog_Title = "Test Email",
            GuoLuKaCheckFailLog_Template = "GuoLuKaCheckFail.cshtml",
            GuoLuKaCheckFailLog_To = "lijungjhuang@uitc.com.tw",
            GuoLuKaCheckFailLog_Title = "國旅卡客戶檢核排程異常Notice",
        };
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆郵件設定-查無資料", ExampleType = ExampleType.Response)]
public class 更新單筆郵件設定查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "2");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆郵件設定-呼叫有誤", ExampleType = ExampleType.Response)]
public class 更新單筆郵件設定呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
