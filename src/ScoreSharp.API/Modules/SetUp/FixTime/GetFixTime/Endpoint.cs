using ScoreSharp.API.Modules.SetUp.FixTime.GetFixTime;

namespace ScoreSharp.API.Modules.SetUp.FixTime
{
    public partial class FixTimeController
    {
        /// <summary>
        /// 查詢維護時段設定
        /// </summary>
        /// <returns></returns>
        /// <response code="200">查詢成功</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetFixTimeResponse>))]
        [EndpointSpecificExample(
            typeof(取得維護時段設定_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetFixTime")]
        public async Task<IResult> GetFixTime()
        {
            var result = await _mediator.Send(new Query());
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.FixTime.GetFixTime
{
    public record Query() : IRequest<ResultResponse<GetFixTimeResponse?>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetFixTimeResponse?>>
    {
        public async Task<ResultResponse<GetFixTimeResponse?>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await context.SetUp_FixTime.AsNoTracking().FirstOrDefaultAsync(cancellationToken);

            if (entity is null)
                return ApiResponseHelper.NotFound<GetFixTimeResponse?>(null, "查無資料");

            return ApiResponseHelper.Success(mapper.Map<GetFixTimeResponse>(entity));
        }
    }
}
