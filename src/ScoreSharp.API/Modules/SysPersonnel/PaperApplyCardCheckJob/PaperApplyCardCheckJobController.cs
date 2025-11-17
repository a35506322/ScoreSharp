namespace ScoreSharp.API.Modules.SysPersonnel.PaperApplyCardCheckJob;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J5-紙本件待檢核設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class PaperApplyCardCheckJobController : ControllerBase
{
    private readonly IMediator _mediator;

    public PaperApplyCardCheckJobController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
