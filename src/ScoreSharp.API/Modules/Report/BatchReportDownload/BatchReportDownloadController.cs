namespace ScoreSharp.API.Modules.Report.BatchReportDownload;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("E1-批次報表下載")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class BatchReportDownloadController : ControllerBase
{
    private readonly IMediator _mediator;

    public BatchReportDownloadController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
