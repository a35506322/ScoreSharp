namespace ScoreSharp.API.Modules.OrgSetUp.Organize;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("C5-組織作業-組織設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class OrganizeController : ControllerBase
{
    private readonly IMediator _mediator;

    public OrganizeController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
