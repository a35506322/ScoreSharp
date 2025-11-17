namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

public class RetryRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string? ID { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string? CHName { get; set; }

    /// <summary>
    /// 卡片種類
    /// </summary>
    public string? ApplyCardType { get; set; }

    /// <summary>
    /// 來源
    /// </summary>
    public string Source { get; set; } = null!;

    /// <summary>
    /// 主附卡別
    /// 1. 正卡
    /// 2. 附卡
    /// 3. 正卡+附卡
    /// 4. 附卡2
    /// 5. 正卡+附卡2
    /// </summary>
    public string CardOwner { get; set; } = null!;

    /// <summary>
    /// 新增時間
    /// </summary>
    public DateTime ApplyDate { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }
}
