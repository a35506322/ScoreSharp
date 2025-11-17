namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.UpdateCaseChangeByApplyNo;

[ExampleAnnotation(Name = "[2000]更新月收入確認案件異動資料_補件作業", ExampleType = ExampleType.Request)]
public class 補件作業_2000_ReqEx : IExampleProvider<List<UpdateCaseChangeByApplyNoRequest>>
{
    public List<UpdateCaseChangeByApplyNoRequest> GetExample()
    {
        return new List<UpdateCaseChangeByApplyNoRequest>()
        {
            new UpdateCaseChangeByApplyNoRequest()
            {
                ApplyNo = "20241211X3074",
                CaseChangeAction = IncomeConfirmationAction.補件作業,
                SupplementReasonCode = new string[] { "08", "09" },
                OtherSupplementReason = null,
                SupplementNote = "補件原因",
                SupplementSendCardAddr = MailingAddressType.戶籍地址,
                WithdrawalNote = null,
                RejectionReasonCode = null,
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = null,
                IsPrintSMSAndPaper = "Y",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新月收入確認案件異動資料_撤件作業", ExampleType = ExampleType.Request)]
public class 撤件作業_2000_ReqEx : IExampleProvider<List<UpdateCaseChangeByApplyNoRequest>>
{
    public List<UpdateCaseChangeByApplyNoRequest> GetExample()
    {
        return new List<UpdateCaseChangeByApplyNoRequest>()
        {
            new UpdateCaseChangeByApplyNoRequest()
            {
                ApplyNo = "20241211X3074",
                CaseChangeAction = IncomeConfirmationAction.撤件作業,
                SupplementReasonCode = null,
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                WithdrawalNote = "撤件原因",
                RejectionReasonCode = null,
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = null,
                IsPrintSMSAndPaper = "N",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新月收入確認案件異動資料_退件作業", ExampleType = ExampleType.Request)]
public class 退件作業_2000_ReqEx : IExampleProvider<List<UpdateCaseChangeByApplyNoRequest>>
{
    public List<UpdateCaseChangeByApplyNoRequest> GetExample()
    {
        return new List<UpdateCaseChangeByApplyNoRequest>()
        {
            new UpdateCaseChangeByApplyNoRequest()
            {
                ApplyNo = "20241211X3074",
                CaseChangeAction = IncomeConfirmationAction.退件作業,
                SupplementReasonCode = null,
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                WithdrawalNote = null,
                RejectionReasonCode = new string[] { "01", "02" },
                OtherRejectionReason = null,
                RejectionNote = "退件原因",
                RejectionSendCardAddr = null,
                IsPrintSMSAndPaper = "Y",
            },
        };
    }
}

[ExampleAnnotation(Name = "[2000]更新月收入確認案件異動資料", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料_2000_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample()
    {
        return ApiResponseHelper.Success<string>("20241211X3074", "20241211X3074");
    }
}

[ExampleAnnotation(Name = "[4000]更新月收入確認案件異動資料-撤件註記必填", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料撤件註記必填_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"monIncConfCaseChg_WithdrawalNote\": [\"當異動為撤件時，月收入確認_撤件註記為必填。\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4000]更新月收入確認案件異動資料-退件原因代碼必填", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料退件原因代碼必填_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"monIncConfCaseChg_RejectionReasonCode\": [\"當異動為退件時，月收入確認_退件原因代碼為必填。\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4000]更新月收入確認案件異動資料-退件寄送地址必填", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料退件寄送地址必填_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"monIncConfCaseChg_RejectionSendCardAddr\": [\"當異動為退件時，月收入確認_退件寄送地址為必填。\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4000]更新月收入確認案件異動資料-補件原因代碼必填", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料補件原因代碼必填_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"monIncConfCaseChg_RejectionReasonCode\": [\"當異動為補件時，月收入確認_補件原因代碼為必填。\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4000]更新月收入確認案件異動資料-補件寄送地址必填", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料補件寄送地址必填_4000_ResEx : IExampleProvider<ResultResponse<Dictionary<string, IEnumerable<string>>>>
{
    public ResultResponse<Dictionary<string, IEnumerable<string>>> GetExample()
    {
        var jsonString =
            "{\"returnCodeStatus\": 4000,\"returnMessage\": \"\",\"returnData\": {\"monIncConfCaseChg_RejectionSendCardAddr\": [\"當異動為補件時，月收入確認_補件寄送地址為必填。\"]}}";
        var data = JsonHelper.反序列化物件不分大小寫<ResultResponse<Dictionary<string, IEnumerable<string>>>>(jsonString);

        return data;
    }
}

[ExampleAnnotation(Name = "[4003]更新月收入確認案件異動資料", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料_4003_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, "申請書編號與Req比對錯誤");
}

[ExampleAnnotation(Name = "[4001]更新月收入確認案件異動資料-查無案件", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.NotFound<string>(null, "20240903X8997");
}

[ExampleAnnotation(Name = "[4001]更新月收入確認案件異動資料-狀態變更需要一致", ExampleType = ExampleType.Response)]
public class 更新月收入確認案件異動資料狀態變更需要一致_4001_ResEx : IExampleProvider<ResultResponse<string>>
{
    public ResultResponse<string> GetExample() => ApiResponseHelper.BusinessLogicFailed<string>(null, "狀態變更需要一致");
}
