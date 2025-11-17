namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D39-設定作業-主要所得及資金來源設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class MainIncomeAndFundController : ControllerBase
{
    private readonly IMediator _mediator;

    public MainIncomeAndFundController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
