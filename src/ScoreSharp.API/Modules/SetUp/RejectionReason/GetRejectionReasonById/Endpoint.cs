using ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonById;

namespace ScoreSharp.API.Modules.SetUp.RejectionReason
{
    public partial class RejectionReasonController
    {
        /// <summary>
        /// 查詢單筆退件原因
        /// </summary>
        /// <remarks>
        ///     Sample Router :
        ///
        ///         /RejectionReason/GetRejectionReasonById/01
        ///
        /// </remarks>
        /// <param name="rejectionReasonCode">PK</param>
        /// <returns></returns>
        [HttpGet("{rejectionReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetRejectionReasonByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得退件原因_2000_ResEx),
            typeof(取得退件原因查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetRejectionReasonById")]
        public async Task<IResult> GetRejectionReasonById([FromRoute] string rejectionReasonCode)
        {
            var result = await _mediator.Send(new Query(rejectionReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.RejectionReason.GetRejectionReasonById
{
    public record Query(string rejectionReasonCode) : IRequest<ResultResponse<GetRejectionReasonByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetRejectionReasonByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetRejectionReasonByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_RejectionReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.RejectionReasonCode == request.rejectionReasonCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetRejectionReasonByIdResponse>(null, request.rejectionReasonCode);

            var result = _mapper.Map<GetRejectionReasonByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
