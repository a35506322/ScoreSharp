using ScoreSharp.API.Modules.Reviewer.Helpers.Models;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferOutByApplyNo;

[ExampleAnnotation(Name = "[2000]完成人工徵信案件異動資料_權限外", ExampleType = ExampleType.Request)]
public class 完成人工徵信案件異動資料權限外_2000_ReqEx : IExampleProvider<List<CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest>>
{
    public List<CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest> GetExample()
    {
        return new List<CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest>()
        {
            new CompleteManualReviewCaseChangeWithTransferOutByApplyNoRequest()
            {
                SeqNo = "1",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.核卡作業,
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]完成人工徵信案件異動資料_權限外", ExampleType = ExampleType.Response)]
public class 完成人工徵信案件異動資料權限外_2000_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.Success(
            data: new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = "20241211X3074" },
            message: "案件編號: 20241211X3074，完成本案徵審成功(權限外_轉交)"
        );
    }
}

[ExampleAnnotation(Name = "[4000]完成人工徵信案件異動_選取權限外有誤", ExampleType = ExampleType.Response)]
public class 完成人工徵信案件異動_選取權限外有誤_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new()
        {
            { "ExecutionAction", new[] { "權限外不能進行核卡作業、退件作業、補件作業、撤件作業" } },
        };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4003]完成人工徵信案件異動_無指定主管資訊", ExampleType = ExampleType.Response)]
public class 完成人工徵信案件異動_無指定主管資訊_4003_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
            new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = "20241211X3074" },
            "ABCD 無指定主管資訊，請先設定指定主管"
        );
    }
}

[ExampleAnnotation(Name = "[4003]完成權限外人工徵信案件異動_卡片狀態錯誤請先按儲存", ExampleType = ExampleType.Response)]
public class 完成權限外人工徵信案件異動_卡片狀態錯誤請先按儲存_4003_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
            new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = "20241211X3074" },
            "卡片狀態錯誤，請先按儲存，再進行權限外本案徵審"
        );
    }
}

[ExampleAnnotation(Name = "[4006]完成權限外人工徵信案件異動_資料檢核失敗", ExampleType = ExampleType.Response)]
public class 完成權限外人工徵信案件異動_資料檢核失敗_4006_ResEx
    : IExampleProvider<ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>>
{
    public ResultResponse<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse> GetExample()
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

        return ApiResponseHelper.ReviewerBusinessLogicFailed<CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse>(
            data: new CompleteManualReviewCaseChangeWithTransferOutByApplyNoResponse() { ApplyNo = "20241211X3074", RetryChecks = retryChecks },
            message: "請再次執行查詢檢核資料"
        );
    }
}
