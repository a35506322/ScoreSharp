namespace ScoreSharp.API.Modules.Reviewer.Common;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("A4-徵審通用")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class ReviewerCommonController : ControllerBase
{
    private readonly IMediator _mediator;

    public ReviewerCommonController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
