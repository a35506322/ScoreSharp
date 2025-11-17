namespace ScoreSharp.API.Modules.SetUp.ProjectCode;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D19-設定作業-專案代號設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ProjectCodeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProjectCodeController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
