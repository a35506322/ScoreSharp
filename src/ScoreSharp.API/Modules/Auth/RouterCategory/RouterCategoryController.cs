namespace ScoreSharp.API.Modules.Auth.RouterCategory;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("I1-權限作業-路由類別設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class RouterCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public RouterCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
