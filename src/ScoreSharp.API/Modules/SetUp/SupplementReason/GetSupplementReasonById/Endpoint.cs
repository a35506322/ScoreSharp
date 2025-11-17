using ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonById;

namespace ScoreSharp.API.Modules.SetUp.SupplementReason
{
    public partial class SupplementReasonController
    {
        /// <summary>
        ///  查詢單筆補件原因
        /// </summary>
        /// <remarks>
        ///  Sample Router :
        ///
        ///         /SupplementReason/GetSupplementReasonById/01
        ///
        /// </remarks>
        /// <params name="supplementReasonCode">PK</params>
        /// <returns></returns>
        [HttpGet("{supplementReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetSupplementReasonByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得補件原因_2000_ResEx),
            typeof(取得補件原因查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSupplementReasonById")]
        public async Task<IResult> GetSupplementReasonById([FromRoute] string supplementReasonCode)
        {
            var result = await _mediator.Send(new Query(supplementReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.SupplementReason.GetSupplementReasonById
{
    public record Query(string supplementReasonCode) : IRequest<ResultResponse<GetSupplementReasonByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetSupplementReasonByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetSupplementReasonByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_SupplementReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.SupplementReasonCode == request.supplementReasonCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetSupplementReasonByIdResponse>(null, request.supplementReasonCode);

            var result = _mapper.Map<GetSupplementReasonByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
