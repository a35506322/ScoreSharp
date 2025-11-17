namespace ScoreSharp.API.Modules.SetUp.EUSanctionCountry;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D35-設定作業-EU制裁國家設定")]
public partial class EUSanctionCountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public EUSanctionCountryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
