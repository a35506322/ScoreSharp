namespace ScoreSharp.InfoCheckMiddleware.Modules.Test;

[Route("[controller]/[action]")]
public partial class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
