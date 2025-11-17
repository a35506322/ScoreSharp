namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetMonthlyIncomeInfoByApplyNo;

public class GetMonthlyIncomeInfoByApplyNoResponse
{
    /// <summary>
    /// 案件編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 卡片資訊
    /// </summary>
    public List<CardInfo> CardInfoList { get; set; }

    /// <summary>
    /// 正附卡：1. 正卡 2. 附卡 3. 正卡+附卡 4. 附卡2  5. 正卡+附卡2
    /// </summary>
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 正附卡名稱
    /// </summary>
    public string CardOwnerName => CardOwner.ToString();

    /// <summary>
    /// 現職月收入(元)
    /// </summary>
    public int? CurrentMonthIncome { get; set; }

    /// <summary>
    /// 徵信代碼
    /// 1. 當前最新徵信代碼
    /// 2. 關聯 SetUp_CreditCheckCode
    /// 3. 提供進行月收入確認時使用，是否僅能在月收入確認
    /// 4. 多張卡片的徵信代碼會一致
    /// </summary>
    public string? CreditCheckCode { get; set; }
}

public class GetMonthlyIncomeInfoByApplyNoDto
{
    /// <summary>
    /// 案件編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 申請卡別代碼
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 正附卡：1. 正卡 2. 附卡 3. 正卡+附卡 4. 附卡2  5. 正卡+附卡2
    /// </summary>
    public CardOwner CardOwner { get; set; }

    /// <summary>
    /// 現職月收入(元)
    /// </summary>
    public int? CurrentMonthIncome { get; set; }

    /// <summary>
    /// 徵信代碼
    /// 1. 當前最新徵信代碼
    /// 2. 關聯 SetUp_CreditCheckCode
    /// 3. 提供進行月收入確認時使用，是否僅能在月收入確認
    /// 4. 多張卡片的徵信代碼會一致
    /// </summary>
    public string? CreditCheckCode { get; set; }

    /// <summary>
    /// 正附卡：1. 正卡人 2. 附卡人
    /// </summary>
    public UserType UserType { get; set; }
}

public class CardInfo
{
    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    public string CardStatusName => CardStatus.ToString();

    /// <summary>
    /// 申請卡別代碼
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡別名稱
    /// </summary>
    public string ApplyCardTypeName { get; set; }

    /// <summary>
    /// 正附卡：1. 正卡人 2. 附卡人
    /// </summary>
    public UserType UserType { get; set; }
}
