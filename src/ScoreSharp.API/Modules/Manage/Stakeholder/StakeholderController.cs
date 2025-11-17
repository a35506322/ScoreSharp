namespace ScoreSharp.API.Modules.Manage.Stakeholder;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("B98-利害關係人設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class StakeholderController : Controller
{
    private readonly IMediator _mediator;

    public StakeholderController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
