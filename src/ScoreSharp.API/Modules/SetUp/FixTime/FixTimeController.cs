namespace ScoreSharp.API.Modules.SetUp.FixTime;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D55-設定作業-維護時段設定")]
public partial class FixTimeController : ControllerBase
{
    private readonly IMediator _mediator;

    public FixTimeController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
