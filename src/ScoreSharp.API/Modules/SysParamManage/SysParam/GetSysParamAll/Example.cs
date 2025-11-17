namespace ScoreSharp.API.Modules.SysParamManage.SysParam.GetSysParamAll;

[ExampleAnnotation(Name = "[2000]查詢全部系統參數設定", ExampleType = ExampleType.Response)]
public class 查詢全部系統參數設定_2000_ResEx : IExampleProvider<ResultResponse<GetSysParamAllResponse>>
{
    public ResultResponse<GetSysParamAllResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetSysParamAllResponse
            {
                IPCompareHour = 72,
                IPMatchCount = 1,
                QueryHisDataDayRange = 3,
                SeqNo = 1,
                WebCaseEmailCompareHour = 48,
                WebCaseEmailMatchCount = 2,
                WebCaseMobileCompareHour = 48,
                WebCaseMobileMatchCount = 2,
                ShortTimeIDCompareHour = 168,
                ShortTimeIDMatchCount = 5,
                GuoLuKaCaseWithdrawDays = 60,
                GuoLuKaCheckBatchCaseCount = 1000,
                AMLProfessionCode_Version = "20250101",
                KYCFixStartTime = DateTime.Now,
                KYCFixEndTime = DateTime.Now.AddDays(1),
                KYC_StrongReVersion = "20250101",
            }
        );
    }
}
