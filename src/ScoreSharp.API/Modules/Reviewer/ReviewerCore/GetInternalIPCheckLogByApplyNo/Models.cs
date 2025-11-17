namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalIPCheckLogByApplyNo;

public class GetInternalIPCheckLogByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; } = null!;

    /// <summary>
    /// 行內IP相同
    ///
    /// 1. Y/N
    /// 2. 需發送 API 檢驗該值
    /// 3. 須回壓 Pimary 主表
    ///
    /// </summary>
    public string IsEqualInternalIP { get; set; } = null!;

    /// <summary>
    /// 確認紀錄
    ///
    /// 1. 當 IsEqualInternalIP = Y 行員自行填寫原因
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
}
