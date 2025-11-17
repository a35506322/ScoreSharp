namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet.GetBatchSetById;

[ExampleAnnotation(Name = "[2000]查詢單筆排程設定", ExampleType = ExampleType.Response)]
public class 查詢單筆排程設定_2000_ResEx : IExampleProvider<ResultResponse<GetBatchSetByIdResponse>>
{
    public ResultResponse<GetBatchSetByIdResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetBatchSetByIdResponse
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
            }
        );
    }
}
