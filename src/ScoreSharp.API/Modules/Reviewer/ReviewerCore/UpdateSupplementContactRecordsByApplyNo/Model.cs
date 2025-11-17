namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.UpdateSupplementContactRecordsByApplyNo;

public class UpdateSupplementContactRecordsByApplyNoRequest
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Required]
    [MaxLength(14)]
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 補聯繫紀錄_補件類別
    /// </summary>
    [Required]
    [ValidEnumValue]
    [Display(Name = "補聯繫紀錄_補件類別")]
    public SupplementContactRecordsType? SupplementContactRecords_Type { get; set; }

    /// <summary>
    /// 補聯繫紀錄_聯繫結果
    /// </summary>
    [Required]
    [ValidEnumValue]
    [Display(Name = "補聯繫紀錄_聯繫結果")]
    public SupplementContactRecordsResult? SupplementContactRecords_Result { get; set; }

    /// <summary>
    /// 補聯繫紀錄_其他摘要
    /// </summary>
    [Display(Name = "補聯繫紀錄_其他摘要")]
    public string? SupplementContactRecords_Summary { get; set; }
}
