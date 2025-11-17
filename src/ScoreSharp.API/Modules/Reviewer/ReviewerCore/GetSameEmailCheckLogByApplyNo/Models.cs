namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetSameEmailCheckLogByApplyNo;

public class GetSameEmailCheckLogByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 網路件電子信箱是否相同
    ///
    /// 1.Y/N
    /// 2.畫面的是否異常
    /// 3.需發送 API 檢驗該值
    ///
    /// </summary>
    public string SameWebCaseEmailChecked { get; set; } = null!;

    /// <summary>
    /// 確認紀錄
    ///
    /// 1.於月收入確認簽核時，當 SameWebCaseEmailChecked =Y，需填寫原因
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
    /// 相同電子信箱記錄
    /// </summary>
    public List<SameEmailCheckDetailDto> SameEmailCheckDetails { get; set; } = new();
}

public class SameEmailCheckDetailDto
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
    public CardStatus CurrentCardStatus { get; set; }

    /// <summary>
    /// 現行申請狀態名稱
    /// </summary>
    public string CurrentCardStatusName => CurrentCardStatus.ToString();

    /// <summary>
    /// 現行公司名稱
    /// </summary>
    public string? CurrentCompName { get; set; }

    /// <summary>
    /// 現行電子信箱
    /// </summary>
    public string CurrentEmail { get; set; } = null!;

    /// <summary>
    /// 現行OTP 手機
    /// </summary>
    public string? CurrentOTPMobile { get; set; }

    /// <summary>
    /// 現行推廣單位
    /// </summary>
    public string? CurrentPromotionUnit { get; set; }

    /// <summary>
    /// 現行推薦人
    /// </summary>
    public string? CurrentPromotionUser { get; set; }

    /// <summary>
    /// 同電子信箱之申請書編號
    /// </summary>
    public string SameApplyNo { get; set; } = null!;

    /// <summary>
    /// 同電子信箱之ID
    /// </summary>
    public string SameID { get; set; } = null!;

    /// <summary>
    /// 同電子信箱之姓名
    /// </summary>
    public string SameName { get; set; } = null!;

    /// <summary>
    /// 同電子信箱之申請書狀態
    /// </summary>
    public CardStatus SameCardStatus { get; set; }

    /// <summary>
    /// 同電子信箱之申請書狀態名稱
    /// </summary>
    public string SameCardStatusName => SameCardStatus.ToString();

    /// <summary>
    /// 同電子信箱之公司名稱
    /// </summary>
    public string? SameCompName { get; set; }

    /// <summary>
    /// 同電子信箱之OTP 手機
    /// </summary>
    public string SameOTPMobile { get; set; } = null!;
}

public class SameEmailCheckLogDto
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
