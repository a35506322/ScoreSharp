namespace ScoreSharp.Batch.Jobs.RetryWebCaseFileError.Model;

public class ApplyFileErrorContext
{
    public string ApplyNo { get; set; } = null!;

    public DateTime ApplyDate { get; set; }

    public CardStatus CardStatus { get; set; }

    public string CardAppId { get; set; } = null!;

    public IDType? IDType { get; set; }
    public string IsCITSCard { get; set; }
    public string MyDataCaseNo { get; set; }
}
