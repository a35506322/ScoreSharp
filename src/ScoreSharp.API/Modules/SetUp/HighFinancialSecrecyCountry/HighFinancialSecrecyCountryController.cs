namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D34-設定作業-高金融保密國家")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class HighFinancialSecrecyCountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public HighFinancialSecrecyCountryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
