namespace ScoreSharp.API.Modules.SysParamManage.SysParam;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("H5-系統參數管理作業-系統參數設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class SysParamController : ControllerBase
{
    private readonly IMediator _mediator;

    public SysParamController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
