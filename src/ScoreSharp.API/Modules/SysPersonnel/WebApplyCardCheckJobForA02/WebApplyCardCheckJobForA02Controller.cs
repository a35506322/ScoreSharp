namespace ScoreSharp.API.Modules.SysPersonnel.WebApplyCardCheckJobForA02;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J4-網路件卡友待檢核設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class WebApplyCardCheckJobForA02Controller : ControllerBase
{
    private readonly IMediator _mediator;

    public WebApplyCardCheckJobForA02Controller(IMediator mediator)
    {
        _mediator = mediator;
    }
}
