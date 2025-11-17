using ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodeById;

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode
{
    public partial class CreditCheckCodeController
    {
        /// <summary>
        /// 查詢單筆徵信代碼
        /// </summary>
        /// <remarks>
        ///
        /// Sample Router:
        ///
        ///     /CreditCheckCode/GetCreditCheckCodeById/A05
        ///
        /// </remarks>
        /// <param name="creditCheckCode">PK</param>
        /// <returns></returns>
        [HttpGet("{creditCheckCode}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetCreditCheckCodeByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得徵信代碼_2000_ResEx),
            typeof(取得徵信代碼查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCreditCheckCodeById")]
        public async Task<IResult> GetCreditCheckCodeById([FromRoute] string creditCheckCode)
        {
            var result = await _mediator.Send(new Query(creditCheckCode));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodeById
{
    public record Query(string creditCheckCode) : IRequest<ResultResponse<GetCreditCheckCodeByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetCreditCheckCodeByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetCreditCheckCodeByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_CreditCheckCode.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CreditCheckCode == request.creditCheckCode);

            if (single is null)
                return ApiResponseHelper.NotFound<GetCreditCheckCodeByIdResponse>(null, request.creditCheckCode);

            var result = _mapper.Map<GetCreditCheckCodeByIdResponse>(single);

            return ApiResponseHelper.Success<GetCreditCheckCodeByIdResponse>(result);
        }
    }
}
