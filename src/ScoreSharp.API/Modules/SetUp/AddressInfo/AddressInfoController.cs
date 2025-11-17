namespace ScoreSharp.API.Modules.SetUp.AddressInfo;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D56-設定作業-地址資訊設定")]
public partial class AddressInfoController : ControllerBase
{
    private readonly IMediator _mediator;

    public AddressInfoController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
