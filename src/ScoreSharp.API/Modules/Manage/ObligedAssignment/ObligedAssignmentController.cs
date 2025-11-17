namespace ScoreSharp.API.Modules.Manage.ObligedAssignment;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("B02-強制派案")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ObligedAssignmentController : Controller
{
    private readonly IMediator _mediator;

    public ObligedAssignmentController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
