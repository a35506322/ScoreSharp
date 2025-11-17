namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D17-設定作業-製卡失敗原因設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class MakeCardFailedReasonController : ControllerBase
{
    private readonly IMediator _mediator;

    public MakeCardFailedReasonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
