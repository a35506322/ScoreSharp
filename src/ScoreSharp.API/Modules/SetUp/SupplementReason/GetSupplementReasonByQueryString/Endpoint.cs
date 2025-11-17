using ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonByQueryString;

namespace ScoreSharp.API.Modules.SetUp.SupplementReason
{
    public partial class SupplementReasonController
    {
        /// <summary>
        ///  查詢多筆補件原因
        /// </summary>
        /// <remarks>
        ///
        ///  Sample QueryString :
        ///
        ///     ?IsActive=Y&amp;supplementReasonName=證明&amp;supplementReasonCode
        ///
        /// </remarks>
        /// <params name="request"></params>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetSupplementReasonByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得補件原因_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSupplementReasonByQueryString")]
        public async Task<IResult> GetSupplementReasonByQueryString([FromQuery] GetSupplementReasonByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonByQueryString
{
    public record Query(GetSupplementReasonByQueryStringRequest getSupplementReasonByQueryStringRequest)
        : IRequest<ResultResponse<List<GetSupplementReasonByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetSupplementReasonByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetSupplementReasonByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var dto = request.getSupplementReasonByQueryStringRequest;

            var entity = await _context
                .SetUp_SupplementReason.Where(x => string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive)
                .Where(x => string.IsNullOrEmpty(dto.SupplementReasonName) || x.SupplementReasonName.Contains(dto.SupplementReasonName))
                .Where(x => string.IsNullOrEmpty(dto.SupplementReasonCode) || x.SupplementReasonCode == dto.SupplementReasonCode)
                .ToListAsync();

            var result = _mapper.Map<List<GetSupplementReasonByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
