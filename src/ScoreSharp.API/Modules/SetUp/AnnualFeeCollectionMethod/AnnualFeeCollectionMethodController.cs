namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod;

[Route("[controller]/[action]")]
[ApiController]
[OpenApiTags("D57-設定作業-年費收取方式設定")]
[Authorize(Policy = SecurityConstants.Policy.RBAC)]
public partial class AnnualFeeCollectionMethodController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnnualFeeCollectionMethodController(IMediator mediator)
    {
        _mediator = mediator;
    }
}
