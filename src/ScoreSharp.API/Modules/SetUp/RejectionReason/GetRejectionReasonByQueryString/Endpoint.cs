using ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonByQueryString;

namespace ScoreSharp.API.Modules.SetUp.RejectionReason
{
    public partial class RejectionReasonController
    {
        /// <summary>
        ///  查詢多筆退件原因
        /// </summary>
        /// <remarks>
        ///     Sample QueryString:
        ///
        ///         ?IsActive=Y&amp;RejectionReasonName=卡&amp;RejectionReasonCode
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetRejectionReasonByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得退件原因_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRejectionReasonByQueryString")]
        public async Task<IResult> GetRejectionReasonByQueryString([FromQuery] GetRejectionReasonByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonByQueryString
{
    public record Query(GetRejectionReasonByQueryStringRequest getRejectionReasonByQueryStringRequest)
        : IRequest<ResultResponse<List<GetRejectionReasonByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetRejectionReasonByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetRejectionReasonByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var dto = request.getRejectionReasonByQueryStringRequest;

            var entities = await _context
                .SetUp_RejectionReason.Where(x => string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive)
                .Where(x => string.IsNullOrEmpty(dto.RejectionReasonName) || x.RejectionReasonName.Contains(dto.RejectionReasonName))
                .Where(x => string.IsNullOrEmpty(dto.RejectionReasonCode) || x.RejectionReasonCode == dto.RejectionReasonCode)
                .ToListAsync();

            var result = _mapper.Map<List<GetRejectionReasonByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
