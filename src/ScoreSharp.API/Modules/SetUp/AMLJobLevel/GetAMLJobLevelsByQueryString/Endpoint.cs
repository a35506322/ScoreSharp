using ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel
{
    public partial class AMLJobLevelController
    {
        /// <summary>
        /// 查詢多筆AML職級別
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;AMLJobLevelName=&amp;IsSeniorManagers=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetAMLJobLevelsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得AML職級別_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAMLJobLevelsByQueryString")]
        public async Task<IResult> GetAMLJobLevelsByQueryString([FromQuery] GetAMLJobLevelsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLJobLevel.GetAMLJobLevelsByQueryString
{
    public record Query(GetAMLJobLevelsByQueryStringRequest getAMLJobLevelsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetAMLJobLevelsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetAMLJobLevelsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetAMLJobLevelsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getAMLJobLevelsByQueryStringRequest = request.getAMLJobLevelsByQueryStringRequest;

            var entities = await _context
                .SetUp_AMLJobLevel.Where(x =>
                    string.IsNullOrEmpty(getAMLJobLevelsByQueryStringRequest.AMLJobLevelName)
                    || x.AMLJobLevelName.Contains(getAMLJobLevelsByQueryStringRequest.AMLJobLevelName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getAMLJobLevelsByQueryStringRequest.IsActive)
                    || x.IsActive == getAMLJobLevelsByQueryStringRequest.IsActive
                )
                .Where(x =>
                    string.IsNullOrEmpty(getAMLJobLevelsByQueryStringRequest.IsSeniorManagers)
                    || x.IsSeniorManagers == getAMLJobLevelsByQueryStringRequest.IsSeniorManagers
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetAMLJobLevelsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
