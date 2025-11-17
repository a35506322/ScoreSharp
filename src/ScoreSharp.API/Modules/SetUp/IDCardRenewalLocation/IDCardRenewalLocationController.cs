namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D29-參數設定-身分證換發地點設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class IDCardRenewalLocationController : ControllerBase
{
    private readonly IMediator _mediator;

    public IDCardRenewalLocationController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
