namespace ScoreSharp.Batch.Jobs.A02KYCSync.Models;

public class SyncHandleGroupBy
{
    public string ApplyNo { get; set; }

    public List<GetSyncHandle> Handles { get; set; }
}
