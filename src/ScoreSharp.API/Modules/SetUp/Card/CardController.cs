namespace ScoreSharp.API.Modules.SetUp.Card;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D13-設定作業-信用卡卡片種類設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class CardController : ControllerBase
{
    private readonly IMediator _mediator;

    public CardController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
