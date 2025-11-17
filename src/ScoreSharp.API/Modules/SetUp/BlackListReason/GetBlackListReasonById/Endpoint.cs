using ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonById;

namespace ScoreSharp.API.Modules.SetUp.BlackListReason
{
    public partial class BlackListReasonController
    {
        /// <summary>
        /// 查詢單筆黑名單理由
        /// </summary>
        /// <remarks>
        ///     Sample Router:
        ///
        ///         /BlackListReason/GetBlackListReasonById/01
        ///
        /// </remarks>
        /// <param name="blackListReasonCode">PK</param>
        /// <returns></returns>
        [HttpGet("{blackListReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetBlackListReasonByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得黑名單理由_2000_ResEx),
            typeof(取得黑名單理由查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetBlackListReasonById")]
        public async Task<IResult> GetBlackListReasonById([FromRoute] string blackListReasonCode)
        {
            var result = await _mediator.Send(new Query(blackListReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.BlackListReason.GetBlackListReasonById
{
    public record Query(string blackListReasonCode) : IRequest<ResultResponse<GetBlackListReasonByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetBlackListReasonByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetBlackListReasonByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_BlackListReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.BlackListReasonCode == request.blackListReasonCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetBlackListReasonByIdResponse>(null, request.blackListReasonCode);

            var result = _mapper.Map<GetBlackListReasonByIdResponse>(single);

            return ApiResponseHelper.Success<GetBlackListReasonByIdResponse>(result);
        }
    }
}
