using ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.HealthyCheck;

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 健康檢查
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [OpenApiOperation("HealthyCheck")]
        public async Task<IResult> HealthyCheck()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Reviewer.ReviewerCore.HealthyCheck
{
    public record Query() : IRequest<ResultResponse<string>>;

    public class Handler() : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            return ApiResponseHelper.Success<string>(data: null, message: "API 正常");
        }
    }
}
