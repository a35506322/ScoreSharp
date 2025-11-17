namespace ScoreSharp.API.Modules.SetUp.InternalIP;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D23-設定作業-行內IP設定")]
public partial class InternalIPController : ControllerBase
{
    private readonly IMediator _mediator;

    public InternalIPController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
