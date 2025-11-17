namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.UpdateManualReviewCaseChangeWithTransferOutByApplyNo;

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_排入核卡", ExampleType = ExampleType.Request)]
public class 核卡作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest()
            {
                SeqNo = "14564646",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.排入核卡,
                CardLimit = 150000,
                CreditCheckCode = "A02",
                IsForceCard = "N",
                NuclearCardNote = "測試",
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_排入退件", ExampleType = ExampleType.Request)]
public class 退件作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest()
            {
                SeqNo = "14564646",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.排入退件,
                RejectionReasonCode = new string[] { "08", "09" },
                OtherRejectionReason = "其他退件原因",
                RejectionNote = "退件註記",
                RejectionSendCardAddr = MailingAddressType.戶籍地址,
                IsPrintSMSAndPaper = "Y",
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_排入補件", ExampleType = ExampleType.Request)]
public class 補件作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest()
            {
                SeqNo = "14564646",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.補件作業,
                SupplementReasonCode = new string[] { "08", "09" },
                OtherSupplementReason = "其他補件原因",
                SupplementNote = "補件註記",
                SupplementSendCardAddr = MailingAddressType.戶籍地址,
                IsPrintSMSAndPaper = "Y",
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_排入撤件", ExampleType = ExampleType.Request)]
public class 撤件作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferOutByApplyNoRequest()
            {
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.排入撤件,
                WithdrawalNote = "撤件註記",
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_權限外", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動資料權限外_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success<string>("20241211X3074", "案件編號: 20241211X3074，更新成功(權限外_轉交)");
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動資料_權限外＿補退件地址不完整", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動資料權限外_補退件地址不完整_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "選取補件寄送地址填寫不完整，請重新確認。(0912468FFF)");
    }
}

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_選取權限外有誤", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_選取權限外有誤_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
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

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_排入核卡必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_排入核卡必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new()
        {
            { "CardLimit", new[] { "核卡額度不能為空" } },
            { "CreditCheckCode", new[] { "徵信代碼不能為空" } },
        };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_排入退件必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_排入退件必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new()
        {
            { "RejectionReasonCode", new[] { "退件原因代碼不能為空" } },
            { "RejectionSendCardAddr", new[] { "退件寄送地址不能為空" } },
        };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_排入補件必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_排入補件必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new()
        {
            { "SupplementReasonCode", new[] { "補件原因代碼不能為空" } },
            { "SupplementSendCardAddr", new[] { "補件寄送地址不能為空" } },
        };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_排入撤件必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_排入撤件必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new() { { "WithdrawalNote", new[] { "撤件註記不能為空" } } };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動_排入核卡案件的徵信代碼皆需要一致", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_核卡案件的徵信代碼皆需要一致_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "核卡案件的徵信代碼皆需要一致。");
    }
}
