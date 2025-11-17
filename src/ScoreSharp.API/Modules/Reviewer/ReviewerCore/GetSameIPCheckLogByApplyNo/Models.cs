namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameIPCheckLogByApplyNo;

public class GetSameIPCheckLogByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 相同IP比對
    ///
    /// 1.Y/N
    /// 2.畫面的是否異常
    /// 3.需發送 API 檢驗該值
    ///
    /// </summary>
    public string SameIPChecked { get; set; } = null!;

    /// <summary>
    /// 確認紀錄
    ///
    /// 1.於月收入確認簽核時，當 SameIPChecked =Y，需填寫原因
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
    /// 相同IP記錄
    /// </summary>
    public List<SameIPCheckDetailDto> SameIPCheckDetails { get; set; }
}

public class SameIPCheckDetailDto
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 現行申請書編號，FK
    /// </summary>
    public string CurrentApplyNo { get; set; } = null!;

    /// <summary>
    /// 現行申請狀態
    /// </summary>
    public CardStatus CurrentCardStatus { get; set; }

    /// <summary>
    /// 現行申請狀態名稱
    /// </summary>
    public string CurrentCardStatusName => CurrentCardStatus.ToString();

    /// <summary>
    /// 現行ID
    /// </summary>
    public string CurrentID { get; set; } = null!;

    /// <summary>
    /// 現行姓名
    /// </summary>
    public string CurrentName { get; set; } = null!;

    /// <summary>
    /// 現行公司名稱
    /// </summary>
    public string? CurrentCompName { get; set; }

    /// <summary>
    /// 現行IP 位址
    /// </summary>
    public string CurrentUserSourceIP { get; set; } = null!;

    /// <summary>
    /// 現行OTP 時間
    /// </summary>
    public DateTime? CurrentOTPTime { get; set; }

    /// <summary>
    /// 現行OTP 手機
    /// </summary>
    public string CurrentOTPMobile { get; set; } = null!;

    /// <summary>
    /// 現行推廣單位
    /// </summary>
    public string? CurrentPromotionUnit { get; set; }

    /// <summary>
    /// 現行推廣人員
    /// </summary>
    public string? CurrentPromotionUser { get; set; }

    /// <summary>
    /// 同IP之申請書編號
    /// </summary>
    public string SameApplyNo { get; set; } = null!;

    /// <summary>
    /// 同IP之ID
    /// </summary>
    public string SameID { get; set; } = null!;

    /// <summary>
    /// 同IP之姓名
    /// </summary>
    public string SameName { get; set; } = null!;

    /// <summary>
    /// 同IP之申請書狀態
    /// </summary>
    public CardStatus SameCardStatus { get; set; }

    /// <summary>
    /// 同IP之申請書狀態名稱
    /// </summary>
    public string SameCardStatusName => SameCardStatus.ToString();

    /// <summary>
    /// 同IP之公司名稱
    /// </summary>
    public string? SameCompName { get; set; }

    /// <summary>
    /// 同IP之IP位址
    /// </summary>
    public string SameUserSourceIP { get; set; } = null!;

    /// <summary>
    /// 同IP之OTP時間
    /// </summary>
    public DateTime SameOTPTime { get; set; }

    /// <summary>
    /// 同IP之OTP手機號碼
    /// </summary>
    public string SameOTPMobile { get; set; } = null!;
}

public class SameIPCheckLogDto
{
    public long SeqNo { get; set; }
    public string CurrentApplyNo { get; set; } = null!;
    public string SameApplyNo { get; set; } = null!;
    public string SameID { get; set; }
    public string SameName { get; set; }
    public string SameCompName { get; set; }
    public string SameUserSourceIP { get; set; }
    public string SameOTPMobile { get; set; }
    public DateTime SameOTPTime { get; set; }
    public CardStatus SameCardStatus { get; set; }
}
