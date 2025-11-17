namespace ScoreSharp.API.Modules.SetUp.AMLProfession;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D33-設定作業-AML職業別設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class AMLProfessionController : ControllerBase
{
    private readonly IMediator _mediator;

    public AMLProfessionController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
