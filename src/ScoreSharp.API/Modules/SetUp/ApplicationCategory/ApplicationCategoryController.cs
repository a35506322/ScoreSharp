namespace ScoreSharp.API.Modules.SetUp.ApplicationCategory;

[Route("[controller]/[action]")]
[ApiController]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
[OpenApiTags("D16-設定作業-申請書類別設定")]
public partial class ApplicationCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public ApplicationCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
