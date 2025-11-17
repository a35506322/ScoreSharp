namespace ScoreSharp.API.Modules.OrgSetUp.UserOrgCaseSetUp;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("C4-組織作業-人員組織分案群組設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class UserOrgCaseSetUpController : Controller
{
    private readonly IMediator _mediator;

    public UserOrgCaseSetUpController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
