namespace ScoreSharp.API.Modules.SysPersonnel.Common;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J6-系統人員作業通用")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class SysPersonnelCommonController : ControllerBase
{
    private readonly IMediator _mediator;

    public SysPersonnelCommonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
