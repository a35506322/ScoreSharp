namespace ScoreSharp.API.Modules.Report.ReportCommon;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("E99-報表共用")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ReportCommonController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReportCommonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
