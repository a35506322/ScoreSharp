using ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.LongTermReason
{
    public partial class LongTermReasonController
    {
        /// <summary>
        /// 查詢多筆長循分期戶理由碼
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;LongTermReasonName=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetLongTermReasonsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得長循分期戶理由碼_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetLongTermReasonsByQueryString")]
        public async Task<IResult> GetLongTermReasonsByQueryString([FromQuery] GetLongTermReasonsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonsByQueryString
{
    public record Query(GetLongTermReasonsByQueryStringRequest getLongTermReasonsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetLongTermReasonsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetLongTermReasonsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetLongTermReasonsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getLongTermReasonsByQueryStringRequest = request.getLongTermReasonsByQueryStringRequest;

            var entites = await _context
                .SetUp_LongTermReason.Where(x =>
                    string.IsNullOrEmpty(getLongTermReasonsByQueryStringRequest.LongTermReasonName)
                    || x.LongTermReasonName.Contains(getLongTermReasonsByQueryStringRequest.LongTermReasonName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getLongTermReasonsByQueryStringRequest.IsActive)
                    || x.IsActive == getLongTermReasonsByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetLongTermReasonsByQueryStringResponse>>(entites);

            return ApiResponseHelper.Success(result);
        }
    }
}
