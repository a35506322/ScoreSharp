namespace ScoreSharp.API.Modules.Manage.Common;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("B99-管理作業通用")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ManageCommonController : Controller
{
    private readonly IMediator _mediator;

    public ManageCommonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
