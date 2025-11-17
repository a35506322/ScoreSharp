using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonByQueryString;

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason
{
    public partial class MakeCardFailedReasonController
    {
        /// <summary>
        /// 查詢多筆製卡失敗原因
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;MakeCardFailedReasonName=卡號&amp;MakeCardFailedReasonCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetMakeCardFailedReasonByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得製卡失敗原因_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetMakeCardFailedReasonByQueryString")]
        public async Task<IResult> GetMakeCardFailedReasonByQueryString([FromQuery] GetMakeCardFailedReasonByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonByQueryString
{
    public record Query(GetMakeCardFailedReasonByQueryStringRequest getMakeCardFailedReasonByQueryStringRequest)
        : IRequest<ResultResponse<List<GetMakeCardFailedReasonByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetMakeCardFailedReasonByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetMakeCardFailedReasonByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getMakeCardFailedReasonByQueryStringRequest = request.getMakeCardFailedReasonByQueryStringRequest;

            var entities = await _context
                .SetUp_MakeCardFailedReason.Where(x =>
                    string.IsNullOrEmpty(getMakeCardFailedReasonByQueryStringRequest.MakeCardFailedReasonName)
                    || x.MakeCardFailedReasonName.Contains(getMakeCardFailedReasonByQueryStringRequest.MakeCardFailedReasonName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getMakeCardFailedReasonByQueryStringRequest.MakeCardFailedReasonCode)
                    || x.MakeCardFailedReasonCode == getMakeCardFailedReasonByQueryStringRequest.MakeCardFailedReasonCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getMakeCardFailedReasonByQueryStringRequest.IsActive)
                    || x.IsActive == getMakeCardFailedReasonByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetMakeCardFailedReasonByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
