namespace ScoreSharp.API.Modules.Auth.Router;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("I2-權限作業-路由設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class RouterController : ControllerBase
{
    private readonly IMediator _mediator;

    public RouterController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
