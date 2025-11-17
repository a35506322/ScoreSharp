namespace ScoreSharp.API.Modules.SetUp.CardPromotion;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D01-設定作業-信用卡優惠辦法設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class CardPromotionController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardPromotionController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
