namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.CompleteMonthlyIncomeByApplyNo;

public class CaseContext
{
    public 案件種類 案件種類 { get; set; }
    public bool 是否系統維護 { get; set; } = false;
    public bool 是否有徵信代碼 { get; set; } = false;
    public KYCAPIResult KYCAPIResult { get; set; } = new();
    public string 申請書編號 { get; set; } = string.Empty;
}

public enum 案件種類
{
    網路件_原卡友 = 1,
    網路件_非卡友 = 2,
    紙本件_原卡友 = 3,
    紙本件_非卡友 = 4,
}

public class KYCAPIResult
{
    public bool 呼叫是否成功 { get; set; } = false;
    public string KYC_RtnCode { get; set; } = string.Empty;
    public string KYC_Rc { get; set; } = string.Empty;
    public string KYC_Rc2 { get; set; } = string.Empty;
    public string KYC_Message { get; set; } = string.Empty;
    public string KYC_RiskLevel { get; set; } = string.Empty;
    public string ExceptionMessage { get; set; } = string.Empty;

    public bool 簡單查詢風險等級_是否回傳錯誤訊息()
    {
        if (!呼叫是否成功)
        {
            return true;
        }
        else if (KYC_Rc != "M000" && KYC_Rc2 != "M000")
        {
            return true;
        }
        else if (KYC_RtnCode != MW3RtnCodeConst.簡單查詢風險等級_成功)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool KYC入檔作業_是否回傳錯誤訊息()
    {
        if (!呼叫是否成功)
        {
            return true;
        }
        else if (KYC_Rc != "M000" && KYC_Rc2 != "M000")
        {
            return true;
        }
        else if (KYC_RtnCode != MW3RtnCodeConst.入檔KYC_成功 && KYC_RtnCode != MW3RtnCodeConst.入檔KYC_主機TimeOut)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class CompleteMonthlyIncomeByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔回傳代碼
    /// </summary>
    public string KYC_RtnCode { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔回傳訊息
    /// </summary>
    public string KYC_Message { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔風險等級
    /// </summary>
    public string KYC_RiskLevel { get; set; } = string.Empty;

    /// <summary>
    /// KYC 入檔查詢時間
    /// </summary>
    public DateTime? KYC_QueryTime { get; set; } = null;

    /// <summary>
    /// KYC 入檔例外訊息
    /// </summary>
    public string KYC_Exception { get; set; } = string.Empty;

    /// <summary>
    /// 需要重試的檢核項目
    /// </summary>
    public List<RetryCheck> RetryChecks { get; set; } = [];
}

public class CaseDataBundle
{
    public Reviewer_ApplyCreditCardInfoMain Main { get; set; }
    public Reviewer_FinanceCheckInfo MainFinanceCheck { get; set; }
    public List<Reviewer_ApplyCreditCardInfoHandle> Handles { get; set; }
    public Reviewer_ApplyCreditCardInfoSupplementary? Supplementary { get; set; }
    public Reviewer_FinanceCheckInfo? SupplementaryFinanceCheck { get; set; }
    public Reviewer_BankTrace BankTrace { get; set; }
    public Dictionary<string, string> CardDict { get; set; }
}

public class KYCProcessResult
{
    public bool IsSuccess { get; set; } = false;
    public string? RtnCode { get; set; }
    public string? Message { get; set; }
    public string? RiskLevel { get; set; }
    public DateTime? QueryTime { get; set; }
    public string? Exception { get; set; }

    /// <summary>
    /// 是否需要重試
    /// </summary>
    /// <value></value>
    public bool RequiresRetry { get; set; } = false;

    public List<Reviewer_ApplyCreditCardInfoProcess> Processes { get; set; } = [];
    public List<Reviewer3rd_KYCQueryLog> KYCQueryLogs { get; set; } = [];

    public static KYCProcessResult 無需執行KYC()
    {
        return new KYCProcessResult
        {
            IsSuccess = true,
            RequiresRetry = false,
            Message = "無須執行KYC流程",
        };
    }

    public static KYCProcessResult 系統維護中()
    {
        return new KYCProcessResult
        {
            IsSuccess = true,
            RequiresRetry = true,
            Message = "KYC 系統維護中，已排入重試排程",
        };
    }
}
