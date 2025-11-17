namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J1-系統人員作業-網路件重試")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class WebRetryCaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public WebRetryCaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
