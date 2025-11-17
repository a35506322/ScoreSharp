namespace ScoreSharp.API.Modules.SysPersonnel.MailSet;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("J2-系統人員作業-郵件設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class MailSetController : ControllerBase
{
    private readonly IMediator _mediator;

    public MailSetController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
