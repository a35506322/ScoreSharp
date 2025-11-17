namespace ScoreSharp.API.Modules.SetUp.EUCountry;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D37-設定作業-EU國家設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class EUCountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public EUCountryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
