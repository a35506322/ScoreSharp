namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetInternalCommunicateByApplyNo;

public class GetInternalCommunicateByApplyNoResponse
{
    /// <summary>
    /// 申請書編號
    /// </summary>
    [Display(Name = "申請書編號")]
    public string ApplyNo { get; set; }

    /// <summary>
    /// 徵審照會摘要_註記
    /// </summary>
    [Display(Name = "徵審照會摘要_註記")]
    public string? CommunicationNotes { get; set; }

    /// <summary>
    /// 補聯繫紀錄_登錄人員
    /// </summary>
    [Display(Name = "補聯繫紀錄_登錄人員")]
    public string? SupplementContactRecords_UserId { get; set; }

    /// <summary>
    /// 補聯繫紀錄_登錄人員姓名
    /// </summary>
    [Display(Name = "補聯繫紀錄_登錄人員姓名")]
    public string? SupplementContactRecords_UserName { get; set; }

    /// <summary>
    /// 補聯繫紀錄_補件類別
    /// </summary>
    public SupplementContactRecordsType? SupplementContactRecords_Type { get; set; }

    /// <summary>
    /// 補聯繫紀錄_補件類別名稱
    /// </summary>
    [Display(Name = "補聯繫紀錄_補件類別")]
    public string? SupplementContactRecords_TypeName =>
        this.SupplementContactRecords_Type is not null ? this.SupplementContactRecords_Type.ToString() : null;

    /// <summary>
    /// 補聯繫紀錄_聯繫結果
    /// </summary>
    public SupplementContactRecordsResult? SupplementContactRecords_Result { get; set; }

    /// <summary>
    /// 補聯繫紀錄_聯繫結果名稱
    /// </summary>
    [Display(Name = "補聯繫紀錄_聯繫結果")]
    public string? SupplementContactRecords_ResultName =>
        this.SupplementContactRecords_Result is not null ? this.SupplementContactRecords_Result.ToString() : null;

    /// <summary>
    /// 補聯繫紀錄_其他摘要
    /// </summary>
    [Display(Name = "補聯繫紀錄_其他摘要")]
    public string? SupplementContactRecords_Summary { get; set; }
}
