using ScoreSharp.PaperMiddleware.Modules.Test.PostTest;

namespace ScoreSharp.PaperMiddleware.Modules.Test
{
    public partial class TestController
    {
        [HttpPost]
        public async Task<IResult> PostTest([FromBody] PostTestRequest request) => Results.Ok(await _mediator.Send(new Command(request, ModelState)));
    }
}

namespace ScoreSharp.PaperMiddleware.Modules.Test.PostTest
{
    public record Command(PostTestRequest postTestRequest, ModelStateDictionary modelState) : IRequest<ResultResponse<PostTestResponse>>;

    public class Handler(ILogger<Handler> logger) : IRequestHandler<Command, ResultResponse<PostTestResponse>>
    {
        public async Task<ResultResponse<PostTestResponse>> Handle(Command request, CancellationToken cancellationToken)
        {
            if (!request.modelState.IsValid)
            {
                logger.LogError("錯誤寄信");
                var errors = request.modelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                throw new BadRequestException(errors);
            }

            var req = request.postTestRequest;

            var result = new PostTestResponse
            {
                Name = req.Name,
                Age = req.Age,
                Email = req.Email,
            };

            if (result.Name == "tes")
            {
                logger.LogError("錯誤寄信");
                throw new NotFoundException(result.Name);
            }

            if (result.Age < 18)
            {
                logger.LogError("錯誤寄信");
                var error = new Dictionary<string, string[]> { ["age"] = new string[] { "Age must be greater than 18" } };
                throw new DatabaseDefinitionException(error);
            }

            return ApiResponseHelper.Success(result);
        }
    }
}
