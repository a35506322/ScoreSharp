using ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonById;

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason
{
    public partial class MakeCardFailedReasonController
    {
        /// <summary>
        /// 查詢單筆製卡失敗原因
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /MakeCardFailedReason/GetMakeCardFailedReasonById/11
        ///
        /// </remarks>
        /// <param name="makeCardFailedReasonCode">PK</param>
        /// <returns></returns>
        [HttpGet("{makeCardFailedReasonCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetMakeCardFailedReasonByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得製卡失敗原因_2000_ResEx),
            typeof(取得製卡失敗原因查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetMakeCardFailedReasonById")]
        public async Task<IResult> GetMakeCardFailedReasonById([FromRoute] string makeCardFailedReasonCode)
        {
            var result = await _mediator.Send(new Query(makeCardFailedReasonCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MakeCardFailedReason.GetMakeCardFailedReasonById
{
    public record Query(string makeCardFailedReasonCode) : IRequest<ResultResponse<GetMakeCardFailedReasonByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetMakeCardFailedReasonByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetMakeCardFailedReasonByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_MakeCardFailedReason.AsNoTracking()
                .SingleOrDefaultAsync(x => x.MakeCardFailedReasonCode == request.makeCardFailedReasonCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetMakeCardFailedReasonByIdResponse>(null, request.makeCardFailedReasonCode);

            var result = _mapper.Map<GetMakeCardFailedReasonByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
