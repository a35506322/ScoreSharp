using ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonById;

namespace ScoreSharp.API.Modules.SetUp.LongTermReason
{
    public partial class LongTermReasonController
    {
        /// <summary>
        /// 查詢單筆長循分期戶理由碼
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///       /LongTermReason/GetLongTermReasonById/02
        ///
        /// </remarks>
        /// <param name="code">長循分期戶理由碼代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetLongTermReasonByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得長循分期戶理由碼_2000_ResEx),
            typeof(取得長循分期戶理由碼查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetLongTermReasonById")]
        public async Task<IResult> GetLongTermReasonById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.LongTermReason.GetLongTermReasonById
{
    public record Query(string code) : IRequest<ResultResponse<GetLongTermReasonByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetLongTermReasonByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetLongTermReasonByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_LongTermReason.AsNoTracking().SingleOrDefaultAsync(x => x.LongTermReasonCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetLongTermReasonByIdResponse>(null, request.code);

            var result = _mapper.Map<GetLongTermReasonByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
