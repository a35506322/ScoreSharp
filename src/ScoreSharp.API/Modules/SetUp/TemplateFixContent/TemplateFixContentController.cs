namespace ScoreSharp.API.Modules.SetUp.TemplateFixContent;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D53-設定作業-樣板固定值設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class TemplateFixContentController : ControllerBase
{
    private readonly IMediator _mediator;

    public TemplateFixContentController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
