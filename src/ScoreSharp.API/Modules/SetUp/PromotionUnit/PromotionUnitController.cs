namespace ScoreSharp.API.Modules.SetUp.PromotionUnit;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D18-設定作業-推廣單位設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class PromotionUnitController : ControllerBase
{
    private readonly IMediator _mediator;

    public PromotionUnitController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
