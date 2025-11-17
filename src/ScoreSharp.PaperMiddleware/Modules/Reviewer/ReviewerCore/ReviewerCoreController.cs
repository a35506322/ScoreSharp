namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore;

[Route("[controller]/[action]")]
[ServiceFilter(typeof(HeaderValidationFilter))]
public partial class ReviewerCoreController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewerCoreController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
