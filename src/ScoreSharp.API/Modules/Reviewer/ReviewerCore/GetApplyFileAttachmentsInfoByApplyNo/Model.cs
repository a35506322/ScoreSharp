namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyFileAttachmentsInfoByApplyNo;

public class GetApplyFileAttachmentsInfoByApplyNoResponse
{
    public List<IDOption> IDOptions { get; set; } = new();
    public List<AttachmentsInfo> AttachmentsInfo { get; set; } = new();
}

public class IDOption
{
    public string ID { get; set; }
    public string Name { get; set; }
    public UserType UserType { get; set; }
    public string UserTypeName { get; set; }
}

public class AttachmentsInfo
{
    /// <summary>
    /// PK
    /// </summary>
    public long SeqNo { get; set; }

    /// <summary>
    /// 圖檔名稱
    /// </summary>
    public string FileName { get; set; }

    /// <summary>
    /// 上傳日期
    /// </summary>
    public DateTime AddTime { get; set; }

    /// <summary>
    /// 身分證字號
    /// </summary>
    public string ID { get; set; }

    /// <summary>
    /// 上傳經辦
    /// </summary>
    public string UserId { get; set; }

    /// <summary>
    /// 上傳經辦名稱
    /// </summary>
    public string UserName { get; set; }

    /// <summary>
    /// 檔案ID
    /// </summary>
    public Guid FileId { get; set; }

    /// <summary>
    /// 申請書編號
    /// </summary>
    public string ApplyNo { get; set; }
}

public class ReviewerApplyFileDto
{
    public long SeqNo { get; set; }
    public Guid FileId { get; set; }
    public string ApplyNo { get; set; }
    public string FileName { get; set; }
    public FileType FileType { get; set; }
}
