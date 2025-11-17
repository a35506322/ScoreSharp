namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D32-設定作業-洗錢及資恐高風險國家")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class HighRiskCountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public HighRiskCountryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
