namespace ScoreSharp.API.Modules.SetUp.RejectionReason;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D20-設定作業-退件原因設定")]
public partial class RejectionReasonController : ControllerBase
{
    private readonly IMediator _mediator;

    public RejectionReasonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
