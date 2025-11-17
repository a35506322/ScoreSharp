using ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodeById;

namespace ScoreSharp.API.Modules.SetUp.ProjectCode
{
    public partial class ProjectCodeController
    {
        /// <summary>
        /// 查詢單筆專案代號
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /ProjectCode/GetProjectCodeById/201
        ///
        /// </remarks>
        /// <param name="code">專案代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetProjectCodeByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得專案代號_2000_ResEx),
            typeof(取得專案代號查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetProjectCodeById")]
        public async Task<IResult> GetProjectCodeById([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodeById
{
    public record Query(string code) : IRequest<ResultResponse<GetProjectCodeByIdResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetProjectCodeByIdResponse>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<GetProjectCodeByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context.SetUp_ProjectCode.AsNoTracking().SingleOrDefaultAsync(x => x.ProjectCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetProjectCodeByIdResponse>(null, request.code.ToString());

            var result = _mapper.Map<GetProjectCodeByIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
