namespace ScoreSharp.API.Modules.Reviewer3rd;

[ApiController]
[Route("[controller]/[action]")]
[OpenApiTags("A5-徵審第三方服務")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class Reviewer3rdController : ControllerBase
{
    private readonly IMediator _mediator;

    public Reviewer3rdController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
