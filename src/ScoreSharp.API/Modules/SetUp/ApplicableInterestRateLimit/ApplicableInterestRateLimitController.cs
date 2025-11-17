namespace ScoreSharp.API.Modules.SetUp.ApplicableInterestRateLimit;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D30-設定作業-判斷適用利率額度")]
public partial class ApplicableInterestRateLimitController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicableInterestRateLimitController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
