using Hangfire.Console;
using Hangfire.Console.Extensions;
using Hangfire.SqlServer;
using Hangfire.Tags;
using Hangfire.Tags.SqlServer;
using ScoreSharp.Batch.Infrastructures.Hangfire.Filters;
using ScoreSharp.Batch.Jobs.A02KYCSync;
using ScoreSharp.Batch.Jobs.CompareMissingCases;
using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase;
using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase;
using ScoreSharp.Batch.Jobs.GuoLuKaCheck;
using ScoreSharp.Batch.Jobs.PaperCheckNewCase;
using ScoreSharp.Batch.Jobs.RetryKYCSync;
using ScoreSharp.Batch.Jobs.RetryWebCaseFileError;
using ScoreSharp.Batch.Jobs.SendKYCErrorLog;
using ScoreSharp.Batch.Jobs.SendSystemErrorLog;
using ScoreSharp.Batch.Jobs.SupplementTemplateReport;
using ScoreSharp.Batch.Jobs.SystemAssignment;
using ScoreSharp.Batch.Jobs.TestCallSyncApplyInfoWebWhite;
using ScoreSharp.Batch.Jobs.UpdateReviewManualCase;

namespace ScoreSharp.Batch.Infrastructures.Hangfire;

public static class HangfieConfig
{
    public static void ConfigureHangfire(this IServiceCollection services, IConfiguration config)
    {
        var uitcSecurityHelper = services.BuildServiceProvider().GetService<IUITCSecurityHelper>();
        string connectionString = uitcSecurityHelper.DecryptConn(config.GetConnectionString("ScoreSharp_Util"));

        // hangfire
        services.AddHangfire(configuration =>
            configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(
                    connectionString,
                    new SqlServerStorageOptions
                    {
                        CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                        SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                        QueuePollInterval = TimeSpan.Zero,
                        UseRecommendedIsolationLevel = true,
                        DisableGlobalLocks = true,
                    }
                )
                .WithJobExpirationTimeout(TimeSpan.FromDays(180)) // 保留天數
                .UseConsole() // add hangfire console msg
                .UseTagsWithSql(new TagsOptions() { TagsListStyle = TagsListStyle.Dropdown })
                .UseFilter(
                    new WorkdayCheckFilter(
                        services.BuildServiceProvider(),
                        services.BuildServiceProvider().GetService<ILogger<WorkdayCheckFilter>>()!
                    )
                )
        );

        services.AddHangfireConsoleExtensions(); // add hangfire console msg

        // Add the processing server as IHostedService
        // 千萬 Queues 不要叫default，因為有多台HangfireServer時，會造成他們不知道要不要執行
        // 因為 default誰都可以執行
        services.AddHangfireServer(options =>
        {
            options.Queues = new[] { "batch" };
        });
    }

    public static void SetupHangfire(this IApplicationBuilder app, IConfiguration config, IWebHostEnvironment environment)
    {
        // 使用私有的 AddJobTask 方法來註冊工作
        // AddJobTask<ScoreSharp.Batch.Jobs.TestJob.JobService>("TestJob", job => job.Execute("System Execute Job"), "* * * * *");

        // 如果需要啟用這個工作，請移除註解並使用 AddJobTask
        // AddJobTask<EcardNotA02CheckNewCaseJob>(
        //     "[網路進件]非卡友檢核新案件",
        //     job => job.Execute("System"),
        //     Cron.Daily);

        if (environment.IsDevelopment())
        {
            return;
        }

        AddJobTask<SupplementTemplateReportJob>(
            "報表作業_補件函批次",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:SupplementTemplateReportJob")
        );

        AddJobTask<EcardNotA02CheckNewCaseJob>(
            "網路進件_非卡友檢核新案件",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:EcardNotA02CheckNewCaseJob")
        );

        AddJobTask<SendSystemErrorLogJob>(
            "寄信_系統錯誤",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:SendSystemErrorLogJob")
        );

        AddJobTask<UpdateReviewManualCaseJob>(
            "更新成人工徵審案件",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:UpdateReviewManualCaseJob")
        );

        AddJobTask<CompareMissingCasesJob>(
            "每2小時比對漏進案件批次",
            job => job.Execute("SYSTEM", DateTime.Now.ToString("yyyyMMdd")),
            config.GetValue<string>("BatchExecCron:CompareMissingCasesJob")
        );

        AddJobTask<RetryWebCaseFileErrorJob>(
            "網路件_重新抓取申請書檔案異常",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:RetryWebCaseFileErrorJob")
        );

        AddJobTask<EcardA02CheckNewCaseJob>(
            "網路進件_卡友檢核新案件",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:EcardA02CheckNewCaseJob")
        );

        AddJobTask<GuoLuKaCheckJob>("國旅卡檢核", job => job.Execute("SYSTEM"), config.GetValue<string>("BatchExecCron:GuoLuKaCheckJob"));

        AddJobTask<TestCallSyncApplyInfoWebWhiteJob>(
            "測試排程_呼叫紙本件同步網路小白件",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:TestCallSyncApplyInfoWebWhiteJob")
        );

        AddJobTask<PaperCheckNewCaseJob>(
            "紙本件_檢核新案件",
            job => job.Execute("SYSTEM"),
            config.GetValue<string>("BatchExecCron:PaperCheckNewCaseJob")
        );

        AddJobTask<RetryKYCSyncJob>("重試 KYC 入檔作業", job => job.Execute("SYSTEM"), config.GetValue<string>("BatchExecCron:RetryKYCSyncJob"));

        AddJobTask<SendKYCErrorLogJob>("寄信_KYC錯誤", job => job.Execute("SYSTEM"), config.GetValue<string>("BatchExecCron:SendKYCErrorLogJob"));

        AddJobTask<SystemAssignmentJob>("系統派案作業", job => job.Execute("SYSTEM"), config.GetValue<string>("BatchExecCron:SystemAssignmentJob"));

        AddJobTask<A02KYCSyncJob>("A02 KYC 入檔作業", job => job.Execute("SYSTEM"), config.GetValue<string>("BatchExecCron:A02KYCSyncJob"));
    }

    private static void AddJobTask<T>(string id, Expression<Action<T>> job, string cron)
    {
        RecurringJob.RemoveIfExists(id);
        var options = new RecurringJobOptions { TimeZone = TimeZoneInfo.Local };
        RecurringJob.AddOrUpdate(id, "batch", job, cron, options);
    }
}
