namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoQueryLogByApplyNo;

public class GetApplyCreditCardInfoQueryLogByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 查詢用戶ID
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 查詢時間
    /// </summary>
    public DateTime QueryTime { get; set; }

    /// <summary>
    /// 查詢用戶名稱
    /// </summary>
    public string UserName { get; set; }
}
