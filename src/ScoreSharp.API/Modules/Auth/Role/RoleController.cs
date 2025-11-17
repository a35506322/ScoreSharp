namespace ScoreSharp.API.Modules.Auth.Role;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("I4-權限作業-角色設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class RoleController : ControllerBase
{
    private readonly IMediator _mediator;

    public RoleController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
