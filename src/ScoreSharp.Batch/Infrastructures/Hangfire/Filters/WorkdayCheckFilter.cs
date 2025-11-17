using System;
using System.Linq;
using Hangfire.Server;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ScoreSharp.Batch.Infrastructures.Data;

namespace ScoreSharp.Batch.Infrastructures.Hangfire.Filters;

/// <summary>
/// 工作日檢查 Hangfire Filter
/// 檢查 Job 是否標記 WorkdayCheckAttribute，若有則檢查今日是否為工作日
/// 若為假日則跳過執行
/// </summary>
public class WorkdayCheckFilter : IServerFilter
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<WorkdayCheckFilter> _logger;

    /// <summary>
    /// 初始化工作日檢查 Filter
    /// </summary>
    /// <param name="serviceProvider">服務提供者</param>
    /// <param name="logger">日誌記錄器</param>
    public WorkdayCheckFilter(IServiceProvider serviceProvider, ILogger<WorkdayCheckFilter> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    /// <summary>
    /// Job 執行前檢查
    /// </summary>
    /// <param name="context">執行上下文</param>
    public void OnPerforming(PerformingContext context)
    {
        // 檢查 Job 方法是否有 WorkdayCheckAttribute
        var hasMethodAttribute = context.BackgroundJob.Job.Method.GetCustomAttributes(typeof(WorkdayCheckAttribute), false).Any();

        // 檢查 Job 類別是否有 WorkdayCheckAttribute
        var hasClassAttribute =
            context.BackgroundJob.Job.Method.DeclaringType?.GetCustomAttributes(typeof(WorkdayCheckAttribute), false).Any() ?? false;

        // 如果沒有標記 WorkdayCheckAttribute，則跳過檢查
        if (!hasMethodAttribute && !hasClassAttribute)
        {
            return;
        }

        var jobName = context.BackgroundJob.Job.Method.Name;
        var className = context.BackgroundJob.Job.Method.DeclaringType?.Name ?? "Unknown";

        _logger.LogInformation("檢查工作日：{ClassName}.{JobName}", className, jobName);

        try
        {
            // 使用 Scoped 服務獲取 DbContext
            using var scope = _serviceProvider.CreateScope();
            using var dbContext = scope.ServiceProvider.GetRequiredService<ScoreSharpContext>();

            var today = DateTime.Today;
            var todayString = today.ToString("yyyyMMdd");

            // 查詢今日是否為假日
            var isHoliday = dbContext.SetUp_WorkDay.Any(x => x.Date == todayString && x.IsHoliday == "Y");

            if (isHoliday)
            {
                _logger.LogInformation("今日 {Date} 為假日，Job {ClassName}.{JobName} 不執行", today.ToString("yyyy-MM-dd"), className, jobName);

                // 取消 Job 執行
                context.Canceled = true;
                return;
            }

            _logger.LogInformation("今日 {Date} 為工作日，Job {ClassName}.{JobName} 正常執行", today.ToString("yyyy-MM-dd"), className, jobName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "檢查工作日時發生錯誤，Job {ClassName}.{JobName} 將正常執行", className, jobName);

            // 發生錯誤時，為了安全起見，讓 Job 正常執行
            // 不取消執行，避免因為工作日檢查錯誤而影響重要業務
        }
    }

    /// <summary>
    /// Job 執行後處理
    /// </summary>
    /// <param name="context">執行完成上下文</param>
    public void OnPerformed(PerformedContext context)
    {
        // 工作日檢查不需要在執行後做任何處理
    }
}
