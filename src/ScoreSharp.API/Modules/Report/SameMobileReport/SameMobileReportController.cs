namespace ScoreSharp.API.Modules.Report.SameMobileReport;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("E49-線上辦卡手機號碼比對相同報表_線上區")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class SameMobileReportController : Controller
{
    private readonly IMediator _mediator;

    public SameMobileReportController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
