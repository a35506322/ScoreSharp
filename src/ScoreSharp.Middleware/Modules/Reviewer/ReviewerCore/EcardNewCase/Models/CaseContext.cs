namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore.EcardNewCase.Models;

public class CaseContext
{
    public string ApplyNo { get; set; } = string.Empty;
    public bool IsCITSCard { get; set; } = false;
    public DateTime ApplyDate => DateTime.Now;
    public 進件類型 CaseType { get; set; }
    public string ResultCode { get; set; } = string.Empty;
    public string MyDataCaseNo { get; set; } = string.Empty;
    public IDType? IDType { get; set; }

    public BillType? BillType { get; set; }
}
