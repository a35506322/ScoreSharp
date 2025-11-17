namespace ScoreSharp.API.Modules.Reviewer3rd.GetUseCreditCardInfoByApplyNo;

public class QueryIBM7020IDDto
{
    public UserType UserType { get; set; }
    public string ID { get; set; } = null!;
}

public class GetUseCreditCardInfoByApplyNoResponse
{
    /// <summary>
    /// 查詢日期
    /// </summary>
    public DateTime QueryDate { get; set; }

    /// <summary>
    ///
    /// </summary>
    public string Note => GetNote();

    /// <summary>
    /// 卡人信用使用狀況
    /// </summary>
    public List<UseCreditCardInfo> UseCreditCardInfos { get; set; } = [];

    private string GetNote()
    {
        List<string> note = [];
        foreach (var useCreditCardInfo in UseCreditCardInfos)
        {
            if (useCreditCardInfo.IsSuccess)
            {
                note.Add($"{useCreditCardInfo.UserType}：{useCreditCardInfo.ID}：有資料");
            }
            else
            {
                note.Add($"{useCreditCardInfo.UserType}：{useCreditCardInfo.ID}：查無資料");
            }
        }
        return string.Join("／", note);
    }
}

public class UseCreditCardInfo
{
    /// <summary>
    /// 使用者類別
    /// </summary>
    [Display(Name = "使用者類別")]
    public UserType UserType { get; set; }

    /// <summary>
    /// 使用者類別名稱
    /// </summary>
    [Display(Name = "使用者類別名稱")]
    public string UserTypeName { get; set; }

    /// <summary>
    /// 身份證字號
    /// </summary>
    [Display(Name = "身份證字號")]
    public string ID { get; set; }

    /// <summary>
    /// 是否成功查到該人的資料
    /// </summary>
    [Display(Name = "是否找到資料")]
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 個人信用卡資料
    /// 1. 預現餘額
    /// 2. 可用餘額
    /// 3. 調整額度日期
    /// 4. 摘要欄
    /// </summary>
    [Display(Name = "個人信用卡資料")]
    public CardBalanceAndSummary CardBalanceAndSummary { get; set; }

    /// <summary>
    /// 本行信用卡清單欄位 +
    /// 最近信用額度日期
    /// 最近信用額度(帳務資料)
    /// 最近繳款日期
    /// 最近繳款金額(帳務資料)
    /// </summary>
    [Display(Name = "本行信用卡清單欄位(含最近信用額度及繳款訊息)")]
    public List<CreditCardDetailsAndRecords> CreditCardDetailsAndRecords { get; set; }
}

public class CreditCardDetailsAndRecords
{
    [Display(Name = "信用卡卡號")]
    public string CreditCardNumber { get; set; }

    [Display(Name = "卡片名稱")]
    public string CardName { get; set; }

    [Display(Name = "卡片種類")]
    public string CardType { get; set; }

    [Display(Name = "額度")]
    public int CardCrlimit { get; set; }

    [Display(Name = "開戶日")]
    public string AccountOpeningDate { get; set; }

    [Display(Name = "卡片到期日")]
    public string CardExpiryDate { get; set; }

    [Display(Name = "現欠總額")]
    public int OutstandingBalance { get; set; }

    [Display(Name = "控管碼")]
    public string BlockCode { get; set; }

    [Display(Name = "最近12個月還款紀錄")]
    public string Last12MonthsRepaymentRecord { get; set; }

    [Display(Name = "最近信用額度日期")]
    public string LastCreditLimitChangeDate { get; set; }

    [Display(Name = "最近信用額度(帳務資料)")]
    public int LastCreditLimit_AccountData { get; set; }

    [Display(Name = "最近繳款日期")]
    public string LastPaymentDate { get; set; }

    [Display(Name = "最近繳款金額(帳務資料)")]
    public int LastPayment_AccountData { get; set; }
}

public class CardBalanceAndSummary
{
    [Display(Name = "預現餘額")]
    public int CashAdvanceBalance { get; set; }

    [Display(Name = "可用餘額")]
    public int AvailableBalance { get; set; }

    [Display(Name = "調整額度日期")]
    public string CreditLimitAdjustmentDate { get; set; }

    [Display(Name = "摘要欄")]
    public string Memo { get; set; }
}
