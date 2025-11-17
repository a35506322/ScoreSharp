namespace ScoreSharp.API.Modules.SysPersonnel.BatchSet;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J7-系統人員作業-排程設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class BatchSetController : Controller
{
    private readonly IMediator _mediator;

    public BatchSetController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
