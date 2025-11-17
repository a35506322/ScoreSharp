namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteMonthlyIncomeByApplyNo;

[ExampleAnnotation(Name = "[2000]完成月收入確認", ExampleType = ExampleType.Response)]
public class 完成月收入確認_2000_ResEx : IExampleProvider<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
{
    public ResultResponse<CompleteMonthlyIncomeByApplyNoResponse> GetExample() =>
        ApiResponseHelper.Success(
            data: new CompleteMonthlyIncomeByApplyNoResponse()
            {
                ApplyNo = "20240903X8997",
                KYC_RtnCode = "K0000",
                KYC_Message = "KYC入檔成功",
                KYC_RiskLevel = "A",
                KYC_QueryTime = DateTime.Now,
                KYC_Exception = null,
            },
            message: "完成月收入確認，申請書編號：20240903X8997"
        );
}

[ExampleAnnotation(Name = "[4003]完成月收入確認_卡片狀態錯誤", ExampleType = ExampleType.Response)]
public class 完成月收入確認_卡片狀態錯誤_4003_ResEx : IExampleProvider<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
{
    public ResultResponse<CompleteMonthlyIncomeByApplyNoResponse> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(null, "卡片狀態錯誤，只能進行紙本件月收入確認或待月收入預審");
}

[ExampleAnnotation(Name = "[4003]完成月收入確認欄位必填", ExampleType = ExampleType.Response)]
public class 完成月收入確認欄位必填_4003_ResEx : IExampleProvider<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
{
    public ResultResponse<CompleteMonthlyIncomeByApplyNoResponse> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(null, "徵信代碼需必填\n現職月收入需必填");
}

[ExampleAnnotation(Name = "[4003]非卡友案件需有姓名檢核紀錄，請重新執行", ExampleType = ExampleType.Response)]
public class 非卡友案件需有姓名檢核紀錄_4003_ResEx : IExampleProvider<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
{
    public ResultResponse<CompleteMonthlyIncomeByApplyNoResponse> GetExample() =>
        ApiResponseHelper.BusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(null, "非卡友案件需有姓名檢核紀錄，請重新執行");
}

[ExampleAnnotation(Name = "[5002]完成月收入確認，KYC入檔發查失敗", ExampleType = ExampleType.Response)]
public class 完成月收入確認_KYC入檔發查失敗_5002_ResEx : IExampleProvider<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
{
    public ResultResponse<CompleteMonthlyIncomeByApplyNoResponse> GetExample() =>
        ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(
            data: new CompleteMonthlyIncomeByApplyNoResponse()
            {
                ApplyNo = "20240903X8997",
                KYC_RtnCode = "KD000",
                KYC_Message = "KYC入檔失敗",
                KYC_RiskLevel = "",
                KYC_QueryTime = DateTime.Now,
                KYC_Exception = "KYC入檔發查失敗",
            },
            checkThirdPartyApiName: "KYC入檔"
        );
}

[ExampleAnnotation(Name = "[4006]完成月收入確認_資料檢核失敗", ExampleType = ExampleType.Response)]
public class 完成月收入確認_資料檢核失敗_4006_ResEx : IExampleProvider<ResultResponse<CompleteMonthlyIncomeByApplyNoResponse>>
{
    public ResultResponse<CompleteMonthlyIncomeByApplyNoResponse> GetExample()
    {
        var retryChecks = new List<RetryCheck>()
        {
            new RetryCheck
            {
                Message = "正卡人 929 檢核尚未完成，請先執行檢核",
                Field = "Checked929",
                APIUrl = "Get929ByApplyNo",
                ID = "A123456789",
                Name = "測試正卡人",
                UserType = UserType.正卡人,
            },
            new RetryCheck
            {
                Message = "正卡人 姓名檢核尚未完成，請先執行檢核",
                Field = "NameChecked",
                APIUrl = "GetNameCheck",
                ID = "A123456789",
                Name = "測試正卡人",
                UserType = UserType.正卡人,
            },
            new RetryCheck
            {
                Message = "【行內 E-Mail 相同】，未檢核，請案重新查詢。",
                Field = "InternalEmailSame_Flag",
                APIUrl = "CheckInternalEmailByApplyNo",
                ID = "A123456789",
                Name = "測試正卡人",
                UserType = UserType.正卡人,
            },
        };

        return ApiResponseHelper.ReviewerBusinessLogicFailed<CompleteMonthlyIncomeByApplyNoResponse>(
            data: new CompleteMonthlyIncomeByApplyNoResponse() { RetryChecks = retryChecks },
            message: "資料檢核失敗"
        );
    }
}
