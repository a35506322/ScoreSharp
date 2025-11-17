namespace ScoreSharp.Batch.Jobs.PaperCheckNewCase.Models;

public class ApplyCaseDetail
{
    public string ApplyNo { get; set; } = string.Empty;
    public string MainID { get; set; } = string.Empty;
    public string MainName { get; set; } = string.Empty;
    public string MainEmail { get; set; } = string.Empty;
    public string MainMobile { get; set; } = string.Empty;
    public CardOwner CardOwner { get; set; }
    public string? SupplementaryID { get; set; }
    public string? SupplementaryName { get; set; }
    public bool HasSupplementary => !string.IsNullOrEmpty(SupplementaryID);
    public string? MainIsOriginalCardholder { get; set; }
    public string? SupplementaryIsOriginalCardholder { get; set; }
}
