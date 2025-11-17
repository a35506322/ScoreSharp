using ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodesByQueryString;

namespace ScoreSharp.API.Modules.SetUp.ProjectCode
{
    public partial class ProjectCodeController
    {
        /// <summary>
        /// 查詢多筆專案代號
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;ProjectName=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetProjectCodesByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得專案代號_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetProjectCodesByQueryString")]
        public async Task<IResult> GetProjectCodesByQueryString([FromQuery] GetProjectCodesByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.ProjectCode.GetProjectCodesByQueryString
{
    public record Query(GetProjectCodesByQueryStringRequest getProjectCodesByQueryStringRequest)
        : IRequest<ResultResponse<List<GetProjectCodesByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetProjectCodesByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetProjectCodesByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getProjectCodesByQueryStringRequest = request.getProjectCodesByQueryStringRequest;

            var entities = await _context
                .SetUp_ProjectCode.Where(x =>
                    string.IsNullOrEmpty(getProjectCodesByQueryStringRequest.ProjectName)
                    || x.ProjectName.Contains(getProjectCodesByQueryStringRequest.ProjectName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getProjectCodesByQueryStringRequest.IsActive)
                    || x.IsActive == getProjectCodesByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetProjectCodesByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
