using ScoreSharp.API.Modules.SetUp.CreditCheckCode.InsertCreditCheckCode;

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode
{
    public partial class CreditCheckCodeController
    {
        /// <summary>
        /// 新增單筆徵信代碼
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增徵信代碼_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增徵信代碼_2000_ResEx),
            typeof(新增徵信代碼資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertCreditCheckCode")]
        public async Task<IResult> InsertCreditCheckCode([FromBody] InsertCreditCheckCodeRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.CreditCheckCode.InsertCreditCheckCode
{
    public record Command(InsertCreditCheckCodeRequest insertCreditCheckCodeRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;
        private readonly IJWTProfilerHelper _jwthelper;

        public Handler(ScoreSharpContext context, IMapper mapper, IJWTProfilerHelper jwthelper)
        {
            _context = context;
            _mapper = mapper;
            _jwthelper = jwthelper;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var insertCreditCheckCodeRequest = request.insertCreditCheckCodeRequest;

            var single = await _context
                .SetUp_CreditCheckCode.AsNoTracking()
                .SingleOrDefaultAsync(x => x.CreditCheckCode == insertCreditCheckCodeRequest.CreditCheckCode);

            if (single != null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, insertCreditCheckCodeRequest.CreditCheckCode);

            var entity = _mapper.Map<SetUp_CreditCheckCode>(insertCreditCheckCodeRequest);

            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(
                insertCreditCheckCodeRequest.CreditCheckCode,
                insertCreditCheckCodeRequest.CreditCheckCode
            );
        }
    }
}
