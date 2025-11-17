namespace ScoreSharp.Middleware.Modules.Reviewer.ReviewerCore;

[ApiController]
[Route("[controller]/[action]")]
public partial class ReviewerCoreController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewerCoreController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
