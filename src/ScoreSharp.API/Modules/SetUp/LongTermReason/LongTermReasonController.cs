namespace ScoreSharp.API.Modules.SetUp.LongTermReason;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D28-設定作業-長循分期戶理由碼設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class LongTermReasonController : ControllerBase
{
    private readonly IMediator _mediator;

    public LongTermReasonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
