namespace ScoreSharp.Batch.Jobs.UpdateReviewManualCase;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("更新成人工徵審案件")]
[WorkdayCheck]
public class UpdateReviewManualCaseJob(ILogger<UpdateReviewManualCaseJob> logger, ScoreSharpContext context)
{
    private static readonly SemaphoreSlim semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("更新成人工徵審案件 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            logger.LogInformation("更新成人工徵審案件開始");
            var now = DateTime.Now;

            // 狀態為完成月收入確認 = 30012 To 人工徵信中 = 10201
            var updateCase = await context
                .Reviewer_ApplyCreditCardInfoHandle.AsNoTracking()
                .Where(x => x.CardStatus == CardStatus.完成月收入確認 || x.CardStatus == CardStatus.網路件_卡友_完成KYC入檔作業)
                .Select(x => new UpdateCaseDto() { SeqNo = x.SeqNo, ApplyNo = x.ApplyNo })
                .ToListAsync();

            if (updateCase.Count == 0)
            {
                logger.LogInformation("沒有需要更新的案件");
                return;
            }

            var updateMain = await context
                .Reviewer_ApplyCreditCardInfoMain.AsNoTracking()
                .Where(x => updateCase.Select(y => y.ApplyNo).Distinct().Contains(x.ApplyNo))
                .Select(x => new UpdateMainDto() { ApplyNo = x.ApplyNo })
                .ToListAsync();

            await context
                .Reviewer_ApplyCreditCardInfoHandle.Where(x => updateCase.Select(y => y.SeqNo).Distinct().Contains(x.SeqNo))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.CardStatus, CardStatus.人工徵信中));

            await context
                .Reviewer_ApplyCreditCardInfoMain.Where(x => updateMain.Select(y => y.ApplyNo).Distinct().Contains(x.ApplyNo))
                .ExecuteUpdateAsync(x => x.SetProperty(y => y.CurrentHandleUserId, y => null));

            var processes = updateMain.Select(x => new Reviewer_ApplyCreditCardInfoProcess()
            {
                ApplyNo = x.ApplyNo,
                ProcessUserId = "SYSTEM",
                StartTime = now,
                EndTime = now,
                Process = CardStatus.人工徵信中.ToString(),
                Notes = "測試_更新成人工徵審案件",
            });

            context.Reviewer_ApplyCreditCardInfoProcess.AddRange(processes);

            await context.SaveChangesAsync();

            logger.LogInformation(
                "更新成人工徵審案件案件編號為{applyNo}",
                string.Join(Environment.NewLine, updateCase.Select(x => x.ApplyNo).Distinct())
            );

            logger.LogInformation("更新成人工徵審案件完成，共{count}筆", updateCase.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "更新成人工徵審案件發生錯誤");
        }
        finally
        {
            semaphore.Release();
        }
    }

    private class UpdateMainDto
    {
        public string ApplyNo { get; set; }
    }

    private class UpdateCaseDto
    {
        public string SeqNo { get; set; }
        public string ApplyNo { get; set; }
    }
}
