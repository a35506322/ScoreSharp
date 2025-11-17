namespace ScoreSharp.API.Modules.OrgSetUp.User;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("C1-組織作業-人員設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
