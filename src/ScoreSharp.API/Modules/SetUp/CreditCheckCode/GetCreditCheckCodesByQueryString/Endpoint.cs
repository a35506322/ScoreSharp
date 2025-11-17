using ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode
{
    public partial class CreditCheckCodeController
    {
        /// <summary>
        /// 查詢多筆徵信代碼
        /// </summary>
        /// <remarks>
        ///
        ///  Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;IsManualReview=Y&amp;IsSystemReview=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetCreditCheckCodesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得徵信代碼_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCreditCheckCodesByQueryString")]
        public async Task<IResult> GetCreditCheckCodesByQueryString([FromQuery] GetCreditCheckCodesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.GetCreditCheckCodesByQueryString
{
    public record Query(GetCreditCheckCodesByQueryStringRequest getCreditCheckCodesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetCreditCheckCodesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetCreditCheckCodesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetCreditCheckCodesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getCreditCheckCodesByQueryStringRequest = request.getCreditCheckCodesByQueryStringRequest;

            var entities = await _context
                .SetUp_CreditCheckCode.Where(x =>
                    string.IsNullOrEmpty(getCreditCheckCodesByQueryStringRequest.IsActive)
                    || x.IsActive == getCreditCheckCodesByQueryStringRequest.IsActive
                )
                .Where(x =>
                    string.IsNullOrEmpty(getCreditCheckCodesByQueryStringRequest.CreditCheckCodeName)
                    || x.CreditCheckCodeName.Contains(getCreditCheckCodesByQueryStringRequest.CreditCheckCodeName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getCreditCheckCodesByQueryStringRequest.CreditCheckCode)
                    || x.CreditCheckCode == getCreditCheckCodesByQueryStringRequest.CreditCheckCode
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetCreditCheckCodesByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
