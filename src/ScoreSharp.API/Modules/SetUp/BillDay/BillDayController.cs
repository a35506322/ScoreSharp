namespace ScoreSharp.API.Modules.SetUp.BillDay;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D44-設定作業-帳單日設定")]
public partial class BillDayController : ControllerBase
{
    private readonly IMediator _mediator;

    public BillDayController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
