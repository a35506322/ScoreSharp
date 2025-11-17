namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetShortTimeIDCheckLogByApplyNo;

public class GetShortTimeIDCheckLogByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 短時間ID頻繁申請相同
    ///
    /// 1. Y/N
    /// 2. 於進件時檢核，當 ShortTimeID_Flag =Y，在執行系統審查時會直接進入人工審核，需填寫原因
    /// </summary>
    public string? ShortTimeID_Flag { get; set; }

    /// <summary>
    /// 確認紀錄 ShortTimeID_Flag =Y，需填寫原因
    /// </summary>
    public string? ShortTimeID_CheckRecord { get; set; }

    /// <summary>
    /// 確認人員
    /// </summary>
    public string? ShortTimeID_UpdateUserId { get; set; }

    /// <summary>
    /// 確認人員姓名
    /// </summary>
    public string? ShortTimeID_UpdateUserName { get; set; }

    /// <summary>
    /// 確認時間
    /// </summary>
    public DateTime? ShortTimeID_UpdateTime { get; set; }

    /// <summary>
    /// 是否異常
    ///
    /// 1. Y/N
    /// 2. ShortTimeID_Flag =Y，需填寫是否異常
    /// </summary>
    public string? ShortTimeID_IsError { get; set; }

    public List<CheckTraceDto> CheckTraceDtos { get; set; } = new();
}

public class CheckTraceDto
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 進件日期
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 申請卡別：以&apos;/&apos;串接，如JA00/JC00，關聯　SetUp_Card
    /// </summary>
    public string ApplyCardType { get; set; }

    /// <summary>
    /// 申請卡別名稱
    /// </summary>
    public string ApplyCardTypeName { get; set; }

    /// <summary>
    /// 卡片狀態
    /// </summary>
    public string CardStatus { get; set; }

    /// <summary>
    /// 卡片狀態名稱
    /// </summary>
    public string CardStatusName { get; set; }
}

public class ShortTimeIDCheckLogDto
{
    public string ApplyNo { get; set; } = null!;

    public string ShortTimeID_Flag { get; set; }

    public string ShortTimeID_CheckRecord { get; set; }

    public string ShortTimeID_UpdateUserId { get; set; }

    public DateTime? ShortTimeID_UpdateTime { get; set; }

    public string ShortTimeID_IsError { get; set; }

    public string Trace_ApplyNo { get; set; } = null!;

    public DateTime Trace_ApplyDate { get; set; }

    public string Trace_ApplyCardType { get; set; }

    public CardStatus Trace_CardStatus { get; set; }

    public string Trace_CardStatusName => Trace_CardStatus.ToString();
}

public class UserDto
{
    public string UserId { get; set; }
    public string UserName { get; set; }
}

public class CardDto
{
    public string CardCode { get; set; }
    public string CardName { get; set; }
}
