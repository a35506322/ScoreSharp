using ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipById;

namespace ScoreSharp.API.Modules.SetUp.Citizenship
{
    public partial class CitizenshipController
    {
        /// <summary>
        /// 查詢單筆國籍
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /Citizenship/GetCitizenshipById/TW
        ///
        /// </remarks>
        /// <param name="code">國籍代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(取得國籍_2000_ResEx),
            typeof(取得國籍查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCitizenshipById")]
        public async Task<IResult> GetCitizenshipById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipById
{
    public record Query(string code) : IRequest<ResultResponse<GetCitizenshipByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetCitizenshipByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetCitizenshipByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_Citizenship.AsNoTracking().SingleOrDefaultAsync(x => x.CitizenshipCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetCitizenshipByIdResponse>(null, request.code);

            var result = _mapper.Map<GetCitizenshipByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
