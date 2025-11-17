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

namespace ScoreSharp.Batch.Jobs;

[ApiController]
[Route("[controller]/[action]")]
[OpenApiTags("手動執行排程")]
[SwaggerResponse(StatusCodes.Status200OK, typeof(string), Description = "一律回傳 Success")]
public class ManualJobController : ControllerBase
{
    /// <summary>
    /// 非卡友檢核排程
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult EcardNotA02CheckNewCaseJob()
    {
        BackgroundJob.Enqueue<EcardNotA02CheckNewCaseJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("EcardNotA02CheckNewCaseJob Success");
    }

    /// <summary>
    /// 批次補件報表作業
    /// </summary>
    [HttpGet]
    public IResult SupplementTemplateReportJob()
    {
        BackgroundJob.Enqueue<SupplementTemplateReportJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("TemplateReportJob Success");
    }

    /// <summary>
    /// 寄信 系統錯誤日誌
    /// </summary>
    [HttpGet]
    public IResult SendSystemErrorLogJob()
    {
        BackgroundJob.Enqueue<SendSystemErrorLogJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("SendSystemErrorLogJob Success");
    }

    /// <summary>
    /// 更新成人工徵審案件
    /// </summary>
    [HttpGet]
    public IResult UpdateReviewManualCaseJob()
    {
        BackgroundJob.Enqueue<UpdateReviewManualCaseJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("UpdateReviewManualCaseJob Success");
    }

    /// <summary>
    /// 每2小時比對漏進案件
    /// </summary>
    /// <param name="date"></param>
    /// <returns></returns>
    [HttpGet]
    public IResult CompareMissingCasesJob(string date)
    {
        BackgroundJob.Enqueue<CompareMissingCasesJob>(job => job.Execute("系統管理員 Excute Job", date));
        return Results.Ok("CompareMissingCasesJob Success");
    }

    /// <summary>
    /// 網路件_申請書檔案重試
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult RetryWebCaseFileErrorJob()
    {
        BackgroundJob.Enqueue<RetryWebCaseFileErrorJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("RetryWebCaseFileErrorJob Success");
    }

    /// <summary>
    /// 網路進件_卡友檢核新案件
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult EcardA02CheckNewCaseJob()
    {
        BackgroundJob.Enqueue<EcardA02CheckNewCaseJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("EcardA02CheckNewCaseJob Success");
    }

    /// <summary>
    /// 國旅卡人士資料檢核排程
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult GuoLuKaCheckJob()
    {
        BackgroundJob.Enqueue<GuoLuKaCheckJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("GuoLuKaCheckJob Success");
    }

    /// <summary>
    /// 測試排程呼叫紙本件同步網路小白件
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult TestCallSyncApplyInfoWebWhite()
    {
        BackgroundJob.Enqueue<TestCallSyncApplyInfoWebWhiteJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("TestCallSyncApplyInfoWebWhite Success");
    }

    /// <summary>
    /// 紙本件檢核新案件
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult PaperCheckNewCaseJob()
    {
        BackgroundJob.Enqueue<PaperCheckNewCaseJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("PaperCheckNewCaseJob Success");
    }

    /// <summary>
    /// 網路件_重試KYC入檔作業
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult RetryKYCSyncJob()
    {
        BackgroundJob.Enqueue<RetryKYCSyncJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("RetryKYCSyncJob Success");
    }

    /// <summary>
    /// 寄信KYC錯誤
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    public IResult SendKYCErrorLogJob()
    {
        BackgroundJob.Enqueue<SendKYCErrorLogJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("SendKYCErrorLogJob Success");
    }

    [HttpGet]
    public IResult SystemAssignmentJob()
    {
        BackgroundJob.Enqueue<SystemAssignmentJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("SystemAssignmentJob Success");
    }

    [HttpGet]
    public IResult A02KYCSyncJob()
    {
        BackgroundJob.Enqueue<A02KYCSyncJob>(job => job.Execute("系統管理員 Excute Job"));
        return Results.Ok("A02KYCSyncJob Success");
    }
}
