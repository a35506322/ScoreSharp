namespace ScoreSharp.API.Modules.Manage.CaseStatistics;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("B03-查詢派案統計")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class CaseStatisticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public CaseStatisticsController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
