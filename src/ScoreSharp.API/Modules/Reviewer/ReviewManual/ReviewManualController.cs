namespace ScoreSharp.API.Modules.Reviewer.ReviewManual;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("A3-人工徵審")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ReviewManualController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewManualController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
