namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D38-設定作業-AML職級別設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class AMLJobLevelController : ControllerBase
{
    private readonly IMediator _mediator;

    public AMLJobLevelController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
