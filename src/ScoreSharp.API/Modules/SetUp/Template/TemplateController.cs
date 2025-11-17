namespace ScoreSharp.API.Modules.SetUp.Template;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D52-設定作業-樣板設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class TemplateController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplateController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
