namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D36-設定作業-UN制裁國家設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class UNSanctionCountryController : ControllerBase
{
    private readonly IMediator _mediator;

    public UNSanctionCountryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
