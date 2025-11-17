namespace ScoreSharp.Batch.Jobs.RetryKYCSync.Models;

public class SyncHandleGroupBy
{
    public string ApplyNo { get; set; }

    public List<GetSyncHandle> Handles { get; set; }
}
