namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForNotA02;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J3-網路件非卡友待檢核設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class WebApplyCardCheckJobForNotA02Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public WebApplyCardCheckJobForNotA02Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
}
