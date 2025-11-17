namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation.GetUserTakeVacationByQueryString;

public class GetUserTakeVacationByQueryStringRequest
{
    /// <summary>
    /// 查詢年
    /// </summary>
    [Required]
    public int Year { get; set; }

    /// <summary>
    /// 查詢月
    /// </summary>
    [Range(1, 12)]
    [Required]
    public int Month { get; set; }
}

public class GetUserTakeVacationByQueryStringResponse
{
    /// <summary>
    /// 休假日期
    /// </summary>
    public string Date { get; set; }

    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 帳號
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 使用者名稱
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 新增員工
    /// </summary>
    public string AddUserId { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime AddTime { get; set; }
}
