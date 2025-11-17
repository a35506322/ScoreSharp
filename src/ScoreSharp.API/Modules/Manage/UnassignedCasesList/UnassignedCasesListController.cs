namespace ScoreSharp.API.Modules.Manage.UnassignedCasesList;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("B10-待派案案件清單")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class UnassignedCasesListController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnassignedCasesListController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
