namespace ScoreSharp.API.Modules.SetUp.BlackListReason;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D14-設定作業-黑名單理由設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class BlackListReasonController : ControllerBase
{
    private readonly IMediator _mediator;

    public BlackListReasonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
