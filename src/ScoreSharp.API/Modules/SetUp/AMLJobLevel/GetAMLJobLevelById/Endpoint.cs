using ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelById;

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel
{
    public partial class AMLJobLevelController
    {
        /// <summary>
        /// 查詢單筆AML職級別
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /AMLJobLevel/GetAMLJobLevelById/2
        ///
        /// </remarks>
        /// <param name="code">AML職級別代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetAMLJobLevelByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得AML職級別_2000_ResEx),
            typeof(取得AML職級別查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAMLJobLevelById")]
        public async Task<IResult> GetAMLJobLevelById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelById
{
    public record Query(string code) : IRequest<ResultResponse<GetAMLJobLevelByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetAMLJobLevelByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetAMLJobLevelByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_AMLJobLevel.AsNoTracking().SingleOrDefaultAsync(x => x.AMLJobLevelCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetAMLJobLevelByIdResponse>(null, request.code.ToString());

            var result = _mapper.Map<GetAMLJobLevelByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
