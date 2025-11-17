namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.UpdateManualReviewCaseChangeWithTransferInByApplyNo;

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_核卡作業", ExampleType = ExampleType.Request)]
public class 核卡作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest()
            {
                SeqNo = "14564646",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.核卡作業,
                CardLimit = 150000,
                CreditCheckCode = "A02",
                IsForceCard = "N",
                NuclearCardNote = "測試",
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_退件作業", ExampleType = ExampleType.Request)]
public class 退件作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest()
            {
                SeqNo = "14564646",
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.退件作業,
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

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_補件作業", ExampleType = ExampleType.Request)]
public class 補件作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest()
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

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_撤件作業", ExampleType = ExampleType.Request)]
public class 撤件作業_2000_ReqEx : IExampleProvider<List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>>
{
    public List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest> GetExample()
    {
        return new List<UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest>()
        {
            new UpdateManualReviewCaseChangeWithTransferInByApplyNoRequest()
            {
                ApplyNo = "20241211X3074",
                CaseChangeAction = ManualReviewAction.撤件作業,
                WithdrawalNote = "撤件註記",
                IsCompleted = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新人工徵信案件異動資料_權限內", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動資料權限內_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success<string>("20241211X3074", "案件編號: 20241211X3074，更新成功(權限內)");
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動資料_權限內_核卡額度不足", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動資料權限內_核卡額度不足_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "卡別:JST59(正卡)，建議額度為150000，超過使用者核卡額度150000。");
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動資料_權限內_補退件地址不完整", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動資料權限內_補退件地址不完整_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "選取補件寄送地址填寫不完整，請重新確認。(0912468FFF)");
    }
}

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_選取權限內有誤", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_選取權限內有誤_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
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

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_核卡作業必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_核卡作業必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
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

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_退件作業必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_退件作業必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
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

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_補件作業必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_補件作業必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
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

[ExampleAnnotation(Name = "[4000]更新人工徵信案件異動_撤件作業必填資料", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_撤件作業必填資料_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        Dictionary<string, IEnumerable<string>> errorMsgs = new() { { "WithdrawalNote", new[] { "撤件註記不能為空" } } };
        return ApiResponseHelper.BadRequest(data: errorMsgs);
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動_卡片狀態不能更新", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_卡片狀態不能更新_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "卡片狀態: 退回重審 不能更新");
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動_權限內使用者核卡額度為0", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_權限內使用者核卡額度為0_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "使用者核卡額度為0，請聯絡管理員設定核卡額度。");
    }
}

[ExampleAnnotation(Name = "[4003]更新人工徵信案件異動_核卡案件的徵信代碼皆需要一致", ExampleType = ExampleType.Response)]
public class 更新人工徵信案件異動_核卡案件的徵信代碼皆需要一致_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.BusinessLogicFailed<string>("20241211X3074", "核卡案件的徵信代碼皆需要一致。");
    }
}
