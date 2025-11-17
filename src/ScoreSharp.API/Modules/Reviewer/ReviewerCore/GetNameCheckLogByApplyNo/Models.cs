namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetNameCheckLogByApplyNo;

public class GetNameCheckLogResponse
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// 使用者類型
    /// </summary>
    public UserType UserType { get; set; }

    /// <summary>
    /// 使用者類型名稱
    /// </summary>
    public string UserTypeName { get; set; }

    /// <summary>
    /// AML系統鍵值
    /// </summary>
    public string AMLId { get; set; }

    /// <summary>
    /// 開始時間
    /// </summary>
    public DateTime StartTime { get; set; }

    /// <summary>
    /// 結束時間
    /// </summary>
    public DateTime EndTime { get; set; }

    /// <summary>
    /// 回覆結果
    /// </summary>
    public string ResponseResult { get; set; }

    /// <summary>
    /// RC分數
    /// </summary>
    public int RcPoint { get; set; }
}
