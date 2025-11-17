namespace ScoreSharp.API.Modules.SetUp.SupplementReason;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D02-設定作業-補件原因設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class SupplementReasonController : ControllerBase
{
    private readonly IMediator _mediator;

    public SupplementReasonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
