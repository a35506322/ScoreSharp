namespace ScoreSharp.API.Modules.Auth.Action;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("I3-權限作業-操作設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ActionController : ControllerBase
{
    private readonly IMediator _mediator;

    public ActionController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
