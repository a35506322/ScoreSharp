namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalEmailCheckLogByApplyNo;

public class GetInternalEmailCheckLogByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 行內電子信箱是否相同
    ///
    /// 1.Y/N
    /// 2.畫面的是否異常
    /// 3.需發送 API 檢驗該值
    ///
    /// </summary>
    public string SameInternalEmailChecked { get; set; } = null!;

    /// <summary>
    /// 確認紀錄
    ///
    /// 1.於月收入確認簽核時，當 SameInternalEmailChecked =Y，需填寫原因
    /// </summary>
    public string? CheckRecord { get; set; }

    /// <summary>
    /// 確認人員
    /// </summary>
    public string? UpdateUserId { get; set; }

    /// <summary>
    /// 確認時間
    /// </summary>
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 確認人員名稱
    /// </summary>
    public string? UpdateUserName { get; set; }

    /// <summary>
    /// 是否異常，Y｜Ｎ
    /// </summary>
    public string? IsError { get; set; }

    /// <summary>
    /// 確認關係
    /// </summary> <summary>
    /// 當 是否異常 = N，需填寫確認關係
    /// </summary>
    /// <value>
    /// 1. 父母
    /// 2. 子女
    /// 3. 配偶
    /// 4. 兄弟姊妹
    /// 5. 配偶父母
    /// 6. 其他關係
    /// 7. 無關係
    /// </value>
    public SameDataRelation? Relation { get; set; }

    /// <summary>
    /// 確認關係名稱
    /// </summary>
    public string? RelationName => Relation?.ToString();

    /// <summary>
    /// 相同電子信箱記錄
    /// </summary>
    public List<SameInternalEmailCheckDetailDto> SameInternalEmailCheckDetails { get; set; } = new();
}

public class SameInternalEmailCheckDetailDto
{
    /// <summary>
    /// 現行申請書編號，FK
    /// </summary>
    public string CurrentApplyNo { get; set; } = null!;

    /// <summary>
    /// 現行ID
    /// </summary>
    public string CurrentID { get; set; } = null!;

    /// <summary>
    /// 現行姓名
    /// </summary>
    public string CurrentName { get; set; } = null!;

    /// <summary>
    /// 現行申請狀態
    /// </summary>
    public List<CardStatusDto> CurrentCardStatusList { get; set; }

    /// <summary>
    /// 現行電子信箱
    /// </summary>
    public string CurrentEmail { get; set; } = null!;

    /// <summary>
    /// 現行推廣單位
    /// </summary>
    public string? CurrentPromotionUnit { get; set; }

    /// <summary>
    /// 現行推薦人
    /// </summary>
    public string? CurrentPromotionUser { get; set; }

    /// <summary>
    /// 同行內電子信箱之ID
    /// </summary>
    public string SameID { get; set; } = null!;

    /// <summary>
    /// 同行內電子信箱之姓名
    /// </summary>
    public string SameName { get; set; } = null!;

    /// <summary>
    /// 同行內電子信箱之帳單地址
    /// </summary>
    public string? SameBillAddr { get; set; }
}

public class CardStatusDto
{
    /// <summary>
    /// PK
    /// </summary>
    public string SeqNo { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public CardStatus CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    public string CardStatusName => CardStatus.ToString();
}

public class InternalEmailDetailDto
{
    public string ApplyNo { get; set; }
    public string SeqNo { get; set; }
    public string ID { get; set; }
    public string CHName { get; set; }
    public CardStatus CardStatus { get; set; }
    public string Email { get; set; }
    public string PromotionUnit { get; set; }
    public string PromotionUser { get; set; }
    public string SameID { get; set; }
    public string SameName { get; set; }
    public string? SameBillAddr { get; set; }
}
