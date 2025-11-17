using ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonByQueryString;

namespace ScoreSharp.API.Modules.SetUp.BlackListReason
{
    public partial class BlackListReasonController
    {
        /// <summary>
        /// 查詢多筆黑名單理由
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;BlackListReasonName=退件&amp;BlackListReasonCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetBlackListReasonByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得黑名單理由_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetBlackListReasonByQueryString")]
        public async Task<IResult> GetBlackListReasonByQueryString([FromQuery] GetBlackListReasonByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonByQueryString
{
    public record Query(GetBlackListReasonByQueryStringRequest getBlackListReasonByQueryStringRequest)
        : IRequest<ResultResponse<List<GetBlackListReasonByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetBlackListReasonByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetBlackListReasonByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getBlackListReasonByQueryStringRequest = request.getBlackListReasonByQueryStringRequest;

            var entities = await _context
                .SetUp_BlackListReason.Where(x =>
                    string.IsNullOrEmpty(getBlackListReasonByQueryStringRequest.BlackListReasonName)
                    || x.BlackListReasonName.Contains(getBlackListReasonByQueryStringRequest.BlackListReasonName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getBlackListReasonByQueryStringRequest.BlackListReasonCode)
                    || x.BlackListReasonCode == getBlackListReasonByQueryStringRequest.BlackListReasonCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getBlackListReasonByQueryStringRequest.IsActive)
                    || x.IsActive == getBlackListReasonByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetBlackListReasonByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
