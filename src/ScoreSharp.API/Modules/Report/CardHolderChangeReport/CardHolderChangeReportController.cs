namespace ScoreSharp.API.Modules.Report.CardHolderChangeReport;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("E50-正卡人附卡人資料變更報表_報表區")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class CardHolderChangeReportController : Controller
{
    private readonly IMediator _mediator;

    public CardHolderChangeReportController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
