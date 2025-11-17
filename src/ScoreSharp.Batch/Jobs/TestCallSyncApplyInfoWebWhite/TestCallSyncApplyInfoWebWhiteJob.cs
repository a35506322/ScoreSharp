using ScoreSharp.Batch.Infrastructures.Adapter.Models;
using ScoreSharp.Batch.Infrastructures.Adapter.PaperMiddleware;

namespace ScoreSharp.Batch.Jobs.TestCallSyncApplyInfoWebWhite;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("測試排程_呼叫紙本件同步網路小白件")]
[WorkdayCheck]
public class TestCallSyncApplyInfoWebWhiteJob(
    ScoreSharpContext context,
    ILogger<TestCallSyncApplyInfoWebWhiteJob> logger,
    IPaperMiddlewareAdapter paperMWAdapter
)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("測試排程_呼叫紙本件同步網路小白件 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        logger.LogInformation("測試排程_呼叫紙本件同步網路小白件 - 執行人員：{createBy} 開始執行", createBy);

        if (!await _semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var handles = await context
                .Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
                .Where(x =>
                    x.CardStatus == CardStatus.網路件_書面申請等待MyData || x.CardStatus == CardStatus.網路件_書面申請等待列印申請書及回郵信封
                )
                .ToListAsync();

            if (!handles.Any())
            {
                logger.LogInformation("未有狀態為書面申請等待MyData或書面申請等待列印申請書及回郵信封的紙本件需要同步");
                return;
            }

            logger.LogInformation("未同步紙本件數量：{@Count}", handles.Count);
            logger.LogInformation("案件編號：{applyNo}", String.Join("、", handles.Select(x => x.ApplyNo).ToArray()));

            foreach (var handle in handles)
            {
                logger.LogInformation("開始處理案件：{applyNo}", handle.ApplyNo);

                try
                {
                    CreateSyncApplyInfoWebWhiteReqRequest createSyncReq = new() { ApplyNo = handle.ApplyNo, SyncUserId = UserIdConst.SYSTEM };
                    var createReq = await paperMWAdapter.CreateSyncApplyInfoWebWhiteReq(createSyncReq);
                    var response = await paperMWAdapter.SyncApplyInfoWebWhite(createReq);

                    if (response.ReturnCodeStatus != PaperMiddlewareReturnCodeStatus.成功)
                    {
                        logger.LogInformation("案件編號：{applyNo} 同步失敗，原因:{errorMsg}", handle.ApplyNo, response.ErrorMessage);
                    }

                    logger.LogInformation("案件編號：{applyNo} 同步成功", handle.ApplyNo);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "案件編號：{applyNo} 同步失敗，原因:{errorMsg}", handle.ApplyNo, ex.ToString());
                    continue;
                }
            }
        }
        finally
        {
            _semaphore.Release();
        }

        logger.LogInformation("測試排程_呼叫紙本件同步網路小白件 - 執行人員：{createBy} 執行完成", createBy);
    }
}
