namespace ScoreSharp.Middleware.Modules.Test;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("測試")]
public partial class TestController : ControllerBase
{
    private readonly IMediator _mediator;

    public TestController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
