namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetKYCStrongReviewTableByApplyNoAndId;

public class GetKYCStrongReviewTableByApplyNoAndIdResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }

    /// <summary>
    /// ID
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// KYC加強審核表格
    /// </summary>
    public byte[] KYCStrongReviewTable { get; set; }

    /// <summary>
    /// 檔案名稱
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 內容類型
    /// </summary>
    public string ContentType { get; set; }
}
