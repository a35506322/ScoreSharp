namespace ScoreSharp.API.Modules.SysPersonnel.MailSet.GetMailSetById;

[ExampleAnnotation(Name = "[2000]查詢單筆郵件設定", ExampleType = ExampleType.Response)]
public class 查詢單筆郵件設定_2000_ResEx : IExampleProvider<ResultResponse<GetMailSetByIdResponse>>
{
    public ResultResponse<GetMailSetByIdResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            new GetMailSetByIdResponse
            {
                SeqNo = 1,
                SystemErrorLog_Template = "TestEmail",
                SystemErrorLog_To = "arthurlin@uitc.com.tw",
                SystemErrorLog_Title = "測試Email",
                GuoLuKaCheckFailLog_Template = "ReviewerLogGuoLuKaCheckFail/ReviewerLogGuoLuKaCheckFail.cshtml",
                GuoLuKaCheckFailLog_To = "lijungjhuang@uitc.com.tw",
                GuoLuKaCheckFailLog_Title = "國旅卡客戶檢核排程異常通知",
                KYCErrorLog_Template = "KYCErrorLog/KYCErrorLog.cshtml",
                KYCErrorLog_To = "lijungjhuang@uitc.com.tw",
                KYCErrorLog_Title = "KYC錯誤通知",
            }
        );
    }
}
