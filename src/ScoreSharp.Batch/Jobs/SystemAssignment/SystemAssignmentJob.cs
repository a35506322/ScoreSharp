using Microsoft.Data.SqlClient;
using ScoreSharp.Batch.Jobs.SystemAssignment.Models;

namespace ScoreSharp.Batch.Jobs.SystemAssignment;

[Queue("batch")]
[AutomaticRetry(Attempts = 0)]
[Tag("網路進件_系統派案")]
[WorkdayCheck]
public class SystemAssignmentJob(ScoreSharpContext context, ILogger<SystemAssignmentJob> logger)
{
    private static readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);

    [DisplayName("系統派案 - 執行人員：{0}")]
    public async Task Execute(string createBy)
    {
        if (!await _semaphore.WaitAsync(0))
        {
            logger.LogWarning("上一個批次任務還在執行中，本次執行已取消");
            return;
        }

        try
        {
            var systemBatchSet = await context.SysParamManage_BatchSet.AsNoTracking().SingleOrDefaultAsync();

            var 派案配置清單 = new[]
            {
                new { IsEnabled = systemBatchSet!.SystemAssignment_WebCase_IsEnabled, CaseType = SystemAssignCaseType.網路件_待月收入預審 },
                new { IsEnabled = systemBatchSet!.SystemAssignment_Paper_IsEnabled, CaseType = SystemAssignCaseType.紙本件_待月收入預審 },
                new { IsEnabled = systemBatchSet!.SystemAssignment_ReviewManual_IsEnabled, CaseType = SystemAssignCaseType.人工徵信中 },
            };

            foreach (var 派案設置 in 派案配置清單)
            {
                if (派案設置.IsEnabled == "N")
                {
                    logger.LogInformation("系統參數設定不執行【網路進件_系統派案：{@CaseType}】排程，執行結束", 派案設置.CaseType.ToString());
                }
                else
                {
                    await 派案流程(派案設置.CaseType);
                }
            }
        }
        catch (Exception ex)
        {
            logger.LogError("系統派案失敗，錯誤訊息：{ex}", ex);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task 派案流程(SystemAssignCaseType 派案類別)
    {
        logger.LogInformation($"{派案類別} 系統派案開始");

        var (卡片狀態, 卡片階段) = 派案類型轉換(派案類別);

        var 待派案案件 = await 查詢待分派案件編號(卡片狀態);
        if (待派案案件.Count == 0)
            return;

        var 徵審人員 = await 查詢人員組織分案群組設定(卡片狀態);
        if (徵審人員.Count == 0)
        {
            logger.LogInformation($"{派案類別} 未設定人員組織分案群組設定");
            return;
        }
        else
        {
            logger.LogInformation("派案徵審人員：{UserIds}", string.Join("、", 徵審人員.Select(x => $"{x.UserId}({x.Sort})")));
        }

        var 最後派案人員 = await 查詢最新系統暫存紀錄(派案類別);
        var 分派結果 = 計算派發數量並排序(徵審人員, 最後派案人員, 待派案案件.Count);

        var 利害關係人 = await 查詢利害關係人();
        var userDic = await context.OrgSetUp_User.AsNoTracking().ToDictionaryAsync(x => x.UserId, x => x.EmployeeNo ?? string.Empty);
        Dictionary<string, List<string>> assignCaseDict = 系統派發案件(待派案案件, 分派結果.CaseAssignmentStatistics, 利害關係人, userDic, 卡片狀態);

        DateTime now = DateTime.Now;

        foreach (var item in assignCaseDict)
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var 員工帳號 = item.Key;
                var 案件編號List = item.Value;

                await 新增派案統計(員工帳號, 案件編號List, now);
                await 新增歷程(卡片狀態, 案件編號List, now, 員工帳號);

                await 更新當前狀態處理經辦及前手經辦(員工帳號, 案件編號List);
                await 更新卡片階段與審查人員(卡片狀態, 卡片階段, 員工帳號, 案件編號List);
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                logger.LogError("系統派案失敗，回滾交易。錯誤訊息：{ex}", ex);
            }
            finally
            {
                context.ChangeTracker.Clear();
            }
        }

        await 更新系統暫存紀錄(派案類別, 分派結果.LastAssignedUserId);

        logger.LogInformation("系統派案完成");
    }

    private CaseAssignmentStatisticDto 計算派發數量並排序(List<UserOrgCaseSetUpDto> 徵審人員, string 最後派案人員, int 待派案案件數量)
    {
        #region 算法邏輯
        /*
            算法邏輯： 派發案件 % 徵審人員數量 = 剩餘案件
            最後派案人員 = 剩餘案件最後一個人
            下次派案人員 = 最後派案人員

            派發 131 案件
            [2025/10/30 13:46:28 INF] 派案徵審人員：janehuang(1)、linachen(1)、arthurlin(2)、meichuanyeh(10)、yunxilin(10)
            [2025/10/30 13:46:28 INF] 開始派案人員：meichuanyeh
            [2025/10/30 13:46:28 INF] 派案人員：yunxilin(10)
            [2025/10/30 13:46:28 INF] 派案人員：janehuang(1)
            [2025/10/30 13:46:28 INF] 派案人員：linachen(1)
            [2025/10/30 13:46:28 INF] 派案人員：arthurlin(2)
            [2025/10/30 13:46:28 INF] 派案人員：meichuanyeh(10)
            [2025/10/30 13:46:28 INF] 最後派案人員：yunxilin

            派發 129 案件
            [2025/10/30 13:47:18 INF] 派案徵審人員：janehuang(1)、linachen(1)、arthurlin(2)、meichuanyeh(10)、yunxilin(10)
            [2025/10/30 13:47:18 INF] 開始派案人員：yunxilin
            [2025/10/30 13:47:18 INF] 派案人員：janehuang(1)
            [2025/10/30 13:47:18 INF] 派案人員：linachen(1)
            [2025/10/30 13:47:18 INF] 派案人員：arthurlin(2)
            [2025/10/30 13:47:18 INF] 派案人員：meichuanyeh(10)
            [2025/10/30 13:47:18 INF] 派案人員：yunxilin(10)
            [2025/10/30 13:47:18 INF] 最後派案人員：meichuanyeh

            派發 100 案件
            [2025/10/30 13:51:05 INF] 派案徵審人員：janehuang(1)、linachen(1)、arthurlin(2)、meichuanyeh(10)、yunxilin(10)
            [2025/10/30 13:51:05 INF] 開始派案人員：meichuanyeh
            [2025/10/30 13:51:05 INF] 派案人員：yunxilin(10)
            [2025/10/30 13:51:05 INF] 派案人員：janehuang(1)
            [2025/10/30 13:51:05 INF] 派案人員：linachen(1)
            [2025/10/30 13:51:05 INF] 派案人員：arthurlin(2)
            [2025/10/30 13:51:05 INF] 派案人員：meichuanyeh(10)
            [2025/10/30 13:51:05 INF] 最後派案人員：meichuanyeh

        */
        #endregion

        logger.LogInformation("開始派案人員：{UserId}", 最後派案人員);

        List<UserOrgCaseSetUpDto> sortedEmployees = [];

        int index = 徵審人員.FindIndex(x => x.UserId == 最後派案人員);
        for (int i = 0; i < 徵審人員.Count; i++)
        {
            index = (index + 1) % 徵審人員.Count;
            sortedEmployees.Add(徵審人員[index]); // 再移到下一個
        }

        foreach (var employee in sortedEmployees)
        {
            logger.LogInformation("派案人員：{UserId}({Sort})", employee.UserId, employee.Sort);
        }

        // 派案
        int 徵審人員總數 = 徵審人員.Count();
        int 平均分配案件數 = 待派案案件數量 / 徵審人員總數;
        var 剩餘未派送案件數量 = 待派案案件數量 % 徵審人員總數;

        string 最後派案人員UserId = string.Empty;
        List<(string userId, int assignedCount)> assignmentResult = [];

        foreach (var employee in sortedEmployees)
        {
            assignmentResult.Add((employee.UserId, 平均分配案件數));
        }

        if (剩餘未派送案件數量 > 0)
        {
            for (int i = 0; i < assignmentResult.Count && 剩餘未派送案件數量 > 0; i++)
            {
                var (userId, assignedCount) = assignmentResult[i];
                assignmentResult[i] = (userId, assignedCount + 1);
                剩餘未派送案件數量--;

                if (剩餘未派送案件數量 == 0)
                    最後派案人員UserId = userId;
            }
        }
        else
        {
            最後派案人員UserId = assignmentResult.Last().userId;
        }

        logger.LogInformation("最後派案人員：{UserId}", 最後派案人員UserId);

        var statisticDto = new CaseAssignmentStatisticDto { CaseAssignmentStatistics = assignmentResult, LastAssignedUserId = 最後派案人員UserId };

        return statisticDto;
    }

    private async Task<string> 查詢最新系統暫存紀錄(SystemAssignCaseType 派案類別)
    {
        var tmpRecord = await context.System_TmpRecord.AsNoTracking().OrderByDescending(x => x.SeqNo).FirstOrDefaultAsync();

        // 如果沒有記錄，返回空字串
        if (tmpRecord == null)
            return string.Empty;

        return 派案類別 switch
        {
            SystemAssignCaseType.網路件_待月收入預審 => tmpRecord.SystemAssignment_WebCaseLastUserId ?? string.Empty,
            SystemAssignCaseType.紙本件_待月收入預審 => tmpRecord.SystemAssignment_PaperCaseLastUserId ?? string.Empty,
            SystemAssignCaseType.人工徵信中 => tmpRecord.SystemAssignment_ManualCaseLastUserId ?? string.Empty,
            _ => throw new ArgumentException("錯誤的派案案件種類"),
        };
    }

    private async Task 更新系統暫存紀錄(SystemAssignCaseType 派案類別, string 最後派案人員UserId)
    {
        // 取得最新記錄或建立新記錄
        var tmpRecord = await context.System_TmpRecord.OrderByDescending(x => x.SeqNo).FirstOrDefaultAsync();

        if (tmpRecord == null)
        {
            tmpRecord = new System_TmpRecord();
            await context.System_TmpRecord.AddAsync(tmpRecord);
        }

        // 更新對應欄位
        switch (派案類別)
        {
            case SystemAssignCaseType.網路件_待月收入預審:
                tmpRecord.SystemAssignment_WebCaseLastUserId = 最後派案人員UserId;
                break;
            case SystemAssignCaseType.紙本件_待月收入預審:
                tmpRecord.SystemAssignment_PaperCaseLastUserId = 最後派案人員UserId;
                break;
            case SystemAssignCaseType.人工徵信中:
                tmpRecord.SystemAssignment_ManualCaseLastUserId = 最後派案人員UserId;
                break;
            default:
                throw new ArgumentException("錯誤的派案案件種類");
        }

        await context.SaveChangesAsync();
    }

    private async Task<HashSet<string>> 查詢員工休假()
    {
        var today = DateTime.Now;
        var userTakeVacations = await context.OrgSetUp_UserTakeVacation.Where(x => x.StartTime <= today && today <= x.EndTime).ToListAsync();
        return userTakeVacations.Select(x => x.UserId).ToHashSet();
    }

    private async Task 更新卡片階段與審查人員(CardStatus 卡片狀態, CardStep 卡片階段, string 員工帳號, List<string> 案件編號)
    {
        if (卡片狀態 == CardStatus.人工徵信中)
        {
            await context.Database.ExecuteSqlRawAsync(
                $"""
                WITH CTE AS (
                    SELECT value AS ApplyNo
                    FROM OPENJSON(@ApplyNoJson)
                )
                UPDATE T
                SET CardStep = @CardStep, ReviewerUserId = @ReviewerUserId, ReviewerTime = @ReviewerTime
                FROM (
                    SELECT H.ApplyNo, CardStep, ReviewerUserId, ReviewerTime
                    FROM Reviewer_ApplyCreditCardInfoHandle H
                    JOIN CTE T ON H.ApplyNo = T.ApplyNo
                ) T
                """,
                new SqlParameter("@ApplyNoJson", JsonSerializer.Serialize(案件編號)),
                new SqlParameter("@CardStep", 卡片階段),
                new SqlParameter("@ReviewerUserId", 員工帳號),
                new SqlParameter("@ReviewerTime", DateTime.Now)
            );
        }
        else
        {
            await context.Database.ExecuteSqlRawAsync(
                $"""
                WITH CTE AS (
                    SELECT value AS ApplyNo
                    FROM OPENJSON(@ApplyNoJson)
                )
                UPDATE T
                SET CardStep = @CardStep
                FROM (
                    SELECT H.ApplyNo, CardStep
                    FROM Reviewer_ApplyCreditCardInfoHandle H
                    JOIN CTE T ON H.ApplyNo = T.ApplyNo
                ) T
                """,
                new SqlParameter("@ApplyNoJson", JsonSerializer.Serialize(案件編號)),
                new SqlParameter("@CardStep", 卡片階段)
            );
        }
    }

    private async Task 更新當前狀態處理經辦及前手經辦(string 員工帳號, List<string> 案件編號) =>
        await context.Database.ExecuteSqlRawAsync(
            $"""
            WITH CTE AS (
                SELECT value AS ApplyNo
                FROM OPENJSON(@ApplyNoJson)
            )
            UPDATE T
            SET CurrentHandleUserId = @CurrentHandleUserId,
                PreviousHandleUserId = @PreviousHandleUserId,
                LastUpdateTime = @LastUpdateTime,
                LastUpdateUserId = @LastUpdateUserId
            FROM (
                SELECT M.ApplyNo, CurrentHandleUserId, PreviousHandleUserId, LastUpdateTime, LastUpdateUserId
                FROM Reviewer_ApplyCreditCardInfoMain M
                JOIN CTE T ON M.ApplyNo = T.ApplyNo
            ) T
            """,
            new SqlParameter("@ApplyNoJson", JsonSerializer.Serialize(案件編號)),
            new SqlParameter("@CurrentHandleUserId", 員工帳號),
            new SqlParameter("@PreviousHandleUserId", 員工帳號),
            new SqlParameter("@LastUpdateTime", DateTime.Now),
            new SqlParameter("@LastUpdateUserId", UserIdConst.SYSTEM)
        );

    private async Task 新增歷程(CardStatus 卡片狀態, List<string> 案件編號, DateTime now, string 員工帳號)
    {
        var 比對Processes = 案件編號.Select(x => new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = x,
                Process = ProcessConst.員工推廣比對及利害關係人比對,
                StartTime = now,
                EndTime = now,
                Notes = ProcessNoteConst.系統派案,
                ProcessUserId = UserIdConst.SYSTEM,
            })
            .ToList();
        await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(比對Processes);

        var processes = 案件編號.Select(x => new Reviewer_ApplyCreditCardInfoProcess
            {
                ApplyNo = x,
                Process = 卡片狀態.ToString(),
                StartTime = now,
                EndTime = now,
                Notes = $"【{ProcessNoteConst.系統派案}】SYSTEM 指派TO：{員工帳號}",
                ProcessUserId = UserIdConst.SYSTEM,
            })
            .ToList();
        await context.Reviewer_ApplyCreditCardInfoProcess.AddRangeAsync(processes);
    }

    private async Task 新增派案統計(string 員工帳號, List<string> 案件編號, DateTime now)
    {
        var caseStatistics = 案件編號.Select(x => new Reviewer_CaseStatistics
            {
                UserId = 員工帳號,
                AddTime = now,
                CaseType = CaseStatisticType.系統派案,
                ApplyNo = x,
            })
            .ToList();
        await context.Reviewer_CaseStatistics.AddRangeAsync(caseStatistics);
    }

    private async Task<Dictionary<string, HashSet<string>>> 查詢利害關係人()
    {
        var 利害關係人 = await context.Reviewer_Stakeholder.AsNoTracking().ToListAsync();

        var 利害關係人GroupBy = 利害關係人.GroupBy(x => x.UserId).ToDictionary(x => x.Key, x => x.Select(y => y.ID).ToHashSet());

        return 利害關係人GroupBy;
    }

    private Dictionary<string, List<string>> 系統派發案件(
        List<AssignCaseDto> 待派案案件,
        List<(string userId, int assignedCount)> 分派結果,
        Dictionary<string, HashSet<string>> 利害關係人,
        Dictionary<string, string> userDic,
        CardStatus 卡片狀態
    )
    {
        Dictionary<string, List<string>> result = [];

        foreach (var (分派員工UserId, 分派數量) in 分派結果)
        {
            var 分派員工EmpNo = userDic.GetValueOrDefault(分派員工UserId, string.Empty);

            var 利害關係人IDs = 利害關係人.TryGetValue(分派員工UserId, out HashSet<string>? value) ? value : [];

            var takeDto = 待派案案件
                // 選取未分派的案件
                .Where(x => !x.IsAssigned)
                //人工徵信案件: 派案人員不能與月收入確認人員一樣
                .Where(x => 卡片狀態 != CardStatus.人工徵信中 || x.MonthlyIncomeCheckUserId != 分派員工UserId)
                // 推廣人員不能與派案人員一樣
                .Where(x => string.IsNullOrEmpty(x.PromotionUser) || string.IsNullOrEmpty(分派員工EmpNo) || x.PromotionUser != 分派員工EmpNo)
                // 排除利害關係人
                .Where(x => x.ID.All(y => !利害關係人IDs.Contains(y)))
                .Take(分派數量)
                .ToList();

            logger.LogInformation($"員工 {分派員工UserId} 分派案件數量：{takeDto.Count}");

            result.Add(分派員工UserId, takeDto.Select(x => x.ApplyNo).ToList());

            // 標記案件為已分派
            takeDto.ForEach(x => x.IsAssigned = true);
        }

        var notAssignCases = 待派案案件.Where(x => x.IsAssigned == false).ToList();
        if (notAssignCases.Count > 0)
        {
            var notAssignedApplyNos = string.Join("、", notAssignCases.Select(x => x.ApplyNo));
            logger.LogWarning("剩餘未派送案件數量：{Count}，案件編號：{ApplyNos}", notAssignCases.Count, notAssignedApplyNos);
        }

        return result;
    }

    private async Task<List<UserOrgCaseSetUpDto>> 查詢人員組織分案群組設定(CardStatus 卡片狀態)
    {
        var 休假人員 = await 查詢員工休假();

        switch (卡片狀態)
        {
            case CardStatus.網路件_待月收入預審:
                var 網路件組織設定 = await context
                    .Database.SqlQueryRaw<UserOrgCaseSetUpDto>(
                        $"""
                        SELECT
                            A.UserId,
                            WebCaseSort AS Sort,
                            B.EmployeeNo
                        FROM OrgSetUp_UserOrgCaseSetUp A
                        JOIN OrgSetUp_User B ON A.UserId = B.UserId
                        WHERE A.IsWebCase = 'Y'
                        ORDER BY A.WebCaseSort, A.UserId
                        """
                    )
                    .ToListAsync();
                return 網路件組織設定.Where(x => !休假人員.Contains(x.UserId)).ToList();
            case CardStatus.紙本件_待月收入預審:
                var 紙本件組織設定 = await context
                    .Database.SqlQueryRaw<UserOrgCaseSetUpDto>(
                        $"""
                        SELECT
                            A.UserId,
                            PaperCaseSort AS Sort,
                            B.EmployeeNo
                        FROM OrgSetUp_UserOrgCaseSetUp A
                        JOIN OrgSetUp_User B ON A.UserId = B.UserId
                        WHERE A.IsPaperCase = 'Y'
                        ORDER BY A.PaperCaseSort, A.UserId
                        """
                    )
                    .ToListAsync();
                return 紙本件組織設定.Where(x => !休假人員.Contains(x.UserId)).ToList();
            case CardStatus.人工徵信中:
                var 人工徵信組織設定 = await context
                    .Database.SqlQueryRaw<UserOrgCaseSetUpDto>(
                        $"""
                        SELECT
                            A.UserId,
                            ManualCaseSort AS Sort,
                            B.EmployeeNo
                        FROM OrgSetUp_UserOrgCaseSetUp A
                        JOIN OrgSetUp_User B ON A.UserId = B.UserId
                        WHERE A.IsManualCase = 'Y'
                        ORDER BY A.ManualCaseSort, A.UserId
                        """
                    )
                    .ToListAsync();
                return 人工徵信組織設定.Where(x => !休假人員.Contains(x.UserId)).ToList();
            default:
                throw new ArgumentException("錯誤的卡片狀態");
        }
    }

    private async Task<List<AssignCaseDto>> 查詢待分派案件編號(CardStatus 卡片狀態)
    {
        var queryDtos = await context
            .Database.SqlQueryRaw<QueryAssignCaseDto>(
                $"""

                WITH FilteredApplyNos AS (
                        SELECT M.ApplyNo
                        FROM  [dbo].[Reviewer_ApplyCreditCardInfoMain] M WITH(NOLOCK)
                        WHERE 1=1
                        AND EXISTS (SELECT 1 FROM [dbo].[Reviewer_ApplyCreditCardInfoHandle] H  WHERE H.ApplyNo = M.ApplyNo AND H.CardStatus = @CardStatus)
                        AND M.CurrentHandleUserId is null
                )
                SELECT
                    h.ApplyNo,
                    STRING_AGG(h.ID, ',') AS IDs,
                    h.MonthlyIncomeCheckUserId,
                    m.PromotionUser
                FROM FilteredApplyNos f
                INNER JOIN Reviewer_ApplyCreditCardInfoHandle h ON f.ApplyNo = h.ApplyNo
                INNER JOIN Reviewer_ApplyCreditCardInfoMain m  ON h.ApplyNo = m.ApplyNo
                GROUP BY h.ApplyNo, h.MonthlyIncomeCheckUserId, m.PromotionUser
                ORDER BY NEWID()

                """,
                new SqlParameter("@CardStatus", 卡片狀態)
            )
            .ToListAsync();

        var dtos = queryDtos
            .Select(x => new AssignCaseDto
            {
                ApplyNo = x.ApplyNo,
                ID = x.IDs.Split(',').ToArray(),
                MonthlyIncomeCheckUserId = x.MonthlyIncomeCheckUserId,
                PromotionUser = x.PromotionUser,
            })
            .ToList();

        if (dtos.Count == 0)
        {
            logger.LogInformation("沒有待分派的案件。");
            return [];
        }

        string applyNos = string.Join("、", dtos.Select(x => x.ApplyNo));
        logger.LogInformation($"查詢待分配案件並亂數排列{Environment.NewLine}案件編號：{applyNos}");

        return dtos;
    }

    private (CardStatus 卡片狀態, CardStep 卡片階段) 派案類型轉換(SystemAssignCaseType systemAssignCaseType) =>
        systemAssignCaseType switch
        {
            SystemAssignCaseType.網路件_待月收入預審 => (CardStatus.網路件_待月收入預審, CardStep.月收入確認),
            SystemAssignCaseType.紙本件_待月收入預審 => (CardStatus.紙本件_待月收入預審, CardStep.月收入確認),
            SystemAssignCaseType.人工徵信中 => (CardStatus.人工徵信中, CardStep.人工徵審),
            _ => throw new ArgumentException("錯誤的派案案件種類"),
        };
}
