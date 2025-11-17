namespace ScoreSharp.API.Modules.Manage.TransferCase;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("B01-調撥案件")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class TransferCaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public TransferCaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
