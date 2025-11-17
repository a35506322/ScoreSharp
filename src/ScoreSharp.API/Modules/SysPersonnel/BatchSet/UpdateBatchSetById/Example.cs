namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet.UpdateBatchSetById;

[ExampleAnnotation(Name = "[2000]更新單筆排程設定", ExampleType = ExampleType.Request)]
public class 更新單筆排程設定_2000_ReqEx : IExampleProvider<UpdateBatchSetByIdRequest>
{
    public UpdateBatchSetByIdRequest GetExample()
    {
        return new UpdateBatchSetByIdRequest()
        {
            SeqNo = 1,
            RetryWebCaseFileErrorJob_IsEnabled = "Y",
            RetryWebCaseFileErrorJob_BatchSize = 500,
            A02KYCSyncJob_IsEnabled = "Y",
            A02KYCSyncJob_BatchSize = 100,
            CompareMissingCases_IsEnabled = "Y",
            EcardA02CheckNewCase_IsEnabled = "Y",
            EcardA02CheckNewCase_BatchSize = 100,
            EcardNotA02CheckNewCase_IsEnabled = "Y",
            EcardNotA02CheckNewCase_BatchSize = 100,
            GuoLuKaCheck_IsEnabled = "Y",
            GuoLuKaCheck_BatchSize = 1000,
            GuoLuKaCaseWithdrawDays = 1,
            PaperCheckNewCase_IsEnabled = "Y",
            PaperCheckNewCase_BatchSize = 100,
            RetryKYCSync_IsEnabled = "Y",
            RetryKYCSync_BatchSize = 500,
            SendKYCErrorLog_IsEnabled = "Y",
            SendKYCErrorLog_BatchSize = 150,
            SendSystemErrorLog_IsEnabled = "Y",
            SendSystemErrorLog_BatchSize = 150,
            SupplementTemplateReport_IsEnabled = "Y",
            SystemAssignment_WebCase_IsEnabled = "Y",
            SystemAssignment_Paper_IsEnabled = "Y",
            SystemAssignment_ReviewManual_IsEnabled = "Y",
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新單筆排程設定-修改成功", ExampleType = ExampleType.Response)]
public class 更新單筆排程設定_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.UpdateByIdSuccess<string>("1", "1");
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆排程設定-查無資料", ExampleType = ExampleType.Request)]
public class 更新單筆排程設定查無資料_4001_ReqEx : IExampleProvider<UpdateBatchSetByIdRequest>
{
    public UpdateBatchSetByIdRequest GetExample()
    {
        return new UpdateBatchSetByIdRequest
        {
            SeqNo = 2,
            RetryWebCaseFileErrorJob_IsEnabled = "Y",
            RetryWebCaseFileErrorJob_BatchSize = 500,
            A02KYCSyncJob_IsEnabled = "Y",
            A02KYCSyncJob_BatchSize = 100,
            CompareMissingCases_IsEnabled = "Y",
            EcardA02CheckNewCase_IsEnabled = "Y",
            EcardA02CheckNewCase_BatchSize = 100,
            EcardNotA02CheckNewCase_IsEnabled = "Y",
            EcardNotA02CheckNewCase_BatchSize = 100,
            GuoLuKaCheck_IsEnabled = "Y",
            GuoLuKaCheck_BatchSize = 1000,
            GuoLuKaCaseWithdrawDays = 1,
            PaperCheckNewCase_IsEnabled = "Y",
            PaperCheckNewCase_BatchSize = 100,
            RetryKYCSync_IsEnabled = "Y",
            RetryKYCSync_BatchSize = 500,
            SendKYCErrorLog_IsEnabled = "Y",
            SendKYCErrorLog_BatchSize = 150,
            SendSystemErrorLog_IsEnabled = "Y",
            SendSystemErrorLog_BatchSize = 150,
            SupplementTemplateReport_IsEnabled = "Y",
            SystemAssignment_WebCase_IsEnabled = "Y",
            SystemAssignment_Paper_IsEnabled = "Y",
            SystemAssignment_ReviewManual_IsEnabled = "Y",
        };
    }
}

[ExampleAnnotation(Name = "[4001]更新單筆排程設定-查無資料", ExampleType = ExampleType.Response)]
public class 更新單筆排程設定查無資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.NotFound<string>(null, "2");
    }
}

[ExampleAnnotation(Name = "[4003]更新單筆排程設定-呼叫有誤", ExampleType = ExampleType.Response)]
public class 更新單筆排程設定呼叫有誤_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.路由與Req比對錯誤<string>(null);
    }
}
