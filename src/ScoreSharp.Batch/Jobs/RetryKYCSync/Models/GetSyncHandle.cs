namespace ScoreSharp.Batch.Jobs.RetryKYCSync.Models;

public class GetSyncHandle
{
    public string SeqNo { get; set; }

    public string ApplyNo { get; set; }

    public string ID { get; set; }

    public UserType UserType { get; set; }

    public CardStatus CardStatus { get; set; }
}
