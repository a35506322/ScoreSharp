using ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionById;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession
{
    public partial class AMLProfessionController
    {
        /// <summary>
        /// 查詢單筆AML職業別
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /AMLProfession/GetAMLProfessionById/01J57D7NVXFHHD4QSZQDABTH3A
        ///
        /// </remarks>
        /// <param name="code">AML職業別代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetAMLProfessionByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得AML職業別_2000_ResEx),
            typeof(取得AML職業別查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAMLProfessionById")]
        public async Task<IResult> GetAMLProfessionById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionById
{
    public record Query(string code) : IRequest<ResultResponse<GetAMLProfessionByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetAMLProfessionByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetAMLProfessionByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_AMLProfession.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetAMLProfessionByIdResponse>(null, request.code.ToString());

            var result = _mapper.Map<GetAMLProfessionByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
