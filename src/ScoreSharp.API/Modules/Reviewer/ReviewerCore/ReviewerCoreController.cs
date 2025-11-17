namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("A1-徵審核心")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ReviewerCoreController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewerCoreController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
