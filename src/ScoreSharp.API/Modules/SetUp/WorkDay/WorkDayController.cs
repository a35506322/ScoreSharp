namespace ScoreSharp.API.Modules.SetUp.WorkDay;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D54-設定作業-工作日設定")]
public partial class WorkDayController : ControllerBase
{
    private readonly IMediator _mediator;

    public WorkDayController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
