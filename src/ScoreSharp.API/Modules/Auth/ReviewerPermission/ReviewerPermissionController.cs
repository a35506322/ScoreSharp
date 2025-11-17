namespace ScoreSharp.API.Modules.Auth.ReviewerPermission;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("I5-權限作業-徵審權限設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ReviewerPermissionController : Controller
{
    private readonly IMediator _mediator;

    public ReviewerPermissionController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
