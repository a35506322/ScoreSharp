using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferInByApplyNo;

[ExampleAnnotation(Name = "[2000]完成人工徵信案件異動資料_權限內", ExampleType = ExampleType.Request)]
public class 完成人工徵信案件異動資料權限內_2000_ReqEx : IExampleProvider<List<CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest>>
{
    public List<CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest> GetExample()
    {
        return new List<CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest>()
        {
            new CompleteManualReviewCaseChangeWithTransferInByApplyNoRequest()
            {
                SeqNo = "1",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.核卡作業,
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]完成人工徵信案件異動資料_權限內", ExampleType = ExampleType.Response)]
public class 完成人工徵信案件異動資料權限內_2000_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
            {
                ApplyNo = "20241211X3074",
                KYC_RtnCode = "K000",
                KYC_Message = string.Empty,
                KYC_Exception = string.Empty,
                KYC_Rc2 = "M000",
            },
            message: $"案件編號: 20241211X3074，完成本案徵審成功({ExecutionAction.權限內})"
        );
    }
}

[ExampleAnnotation(Name = "[4000]完成人工徵信案件異動_選取權限內有誤", ExampleType = ExampleType.Response)]
public class 完成人工徵信案件異動_選取權限內有誤_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new()
        {
            { "ExecutionAction", new[] { "權限內不能進行排入撤件、排入退件、排入補件、排入核卡" } },
        };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4003]完成權限內人工徵信案件異動_卡片狀態錯誤請先按儲存", ExampleType = ExampleType.Response)]
public class 完成權限內人工徵信案件異動_卡片狀態錯誤請先按儲存_4003_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(
            null,
            "卡片狀態錯誤，請先按儲存，再進行權限內本案徵審"
        );
    }
}

[ExampleAnnotation(Name = "[5002]發查第三方API失敗_API意外錯誤", ExampleType = ExampleType.Response)]
public class 發查第三方API失敗_API意外錯誤_5002_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(
            data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
            {
                ApplyNo = "20241211X3074",
                KYC_RtnCode = string.Empty,
                KYC_Message = string.Empty,
                KYC_Exception = "Exception Message",
                KYC_Rc2 = string.Empty,
            },
            checkThirdPartyApiName: "更改建議核准KYC"
        );
    }
}

[ExampleAnnotation(Name = "[5002]發查第三方API失敗_MW3回覆錯誤代碼", ExampleType = ExampleType.Response)]
public class 發查第三方API失敗_MW3回覆錯誤代碼_5002_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.CheckThirdPartyApiErrorWithApiName(
            data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse
            {
                ApplyNo = "20241211X3074",
                KYC_RtnCode = "KD003",
                KYC_Message = "uninumber/身分證格式錯誤,uninumber/此客戶為新戶,",
                KYC_Exception = string.Empty,
                KYC_Rc2 = "KD003",
            },
            checkThirdPartyApiName: "更改建議核准KYC"
        );
    }
}

[ExampleAnnotation(Name = "[4006]完成人工徵信案件異動_資料檢核失敗", ExampleType = ExampleType.Response)]
public class 完成人工徵信案件異動_資料檢核失敗_4006_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse> GetExample()
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

        return ApiResponseHelper.ReviewerBusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse>(
            data: new CompleteManualReviewCaseChangeWithTransferInByApplyNoResponse() { RetryChecks = retryChecks },
            message: "請再次執行查詢檢核資料"
        );
    }
}
