namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("A2-月收入確認")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ReviewerMonthlyIncomeController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewerMonthlyIncomeController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
