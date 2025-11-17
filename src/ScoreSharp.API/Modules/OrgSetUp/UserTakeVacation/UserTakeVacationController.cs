namespace ScoreSharp.API.Modules.OrgSetUp.UserTakeVacation;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("C2-組織作業-員工休假")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class UserTakeVacationController : Controller
{
    private readonly IMediator _mediator;

    public UserTakeVacationController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
