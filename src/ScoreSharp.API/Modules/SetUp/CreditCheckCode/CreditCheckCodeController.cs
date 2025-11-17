namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D22-設定作業-徵信代碼設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class CreditCheckCodeController : ControllerBase
{
    private readonly IMediator _mediator;

    public CreditCheckCodeController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
