using ScoreSharp.Batch.Infrastructures.Adapter.Models;

namespace ScoreSharp.Batch.Infrastructures.Adapter.PaperMiddleware;

public interface IPaperMiddlewareAdapter
{
    Task<SyncApplyInfoWebWhiteRequest> CreateSyncApplyInfoWebWhiteReq(CreateSyncApplyInfoWebWhiteReqRequest request);
    Task<SyncApplyInfoWebWhiteResponse> SyncApplyInfoWebWhite(SyncApplyInfoWebWhiteRequest request);
}
