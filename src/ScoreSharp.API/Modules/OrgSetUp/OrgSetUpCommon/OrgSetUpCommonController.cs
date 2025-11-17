namespace ScoreSharp.API.Modules.OrgSetUp.OrgSetUpCommon;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("C6-組織作業-通用")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class OrgSetUpCommonController : Controller
{
    private readonly IMediator _mediator;

    public OrgSetUpCommonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
