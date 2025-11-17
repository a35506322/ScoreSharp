namespace ScoreSharp.API.Modules.SetUp.Citizenship;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D40-設定作業-國籍設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class CitizenshipController : ControllerBase
{
    private readonly IMediator _mediator;

    public CitizenshipController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
