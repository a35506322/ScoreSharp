using ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.AMLProfession
{
    public partial class AMLProfessionController
    {
        /// <summary>
        /// 查詢多筆AML職業別
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;AMLProfessionsName=&amp;Version=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetAMLProfessionsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得AML職業別_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAMLProfessionsByQueryString")]
        public async Task<IResult> GetAMLProfessionsByQueryString([FromQuery] GetAMLProfessionsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AMLProfession.GetAMLProfessionsByQueryString
{
    public record Query(GetAMLProfessionsByQueryStringRequest getAMLProfessionsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetAMLProfessionsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetAMLProfessionsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetAMLProfessionsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getAMLProfessionsByQueryStringRequest = request.getAMLProfessionsByQueryStringRequest;

            var entities = await _context
                .SetUp_AMLProfession.Where(x =>
                    string.IsNullOrEmpty(getAMLProfessionsByQueryStringRequest.AMLProfessionName)
                    || x.AMLProfessionName.Contains(getAMLProfessionsByQueryStringRequest.AMLProfessionName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getAMLProfessionsByQueryStringRequest.IsActive)
                    || x.IsActive == getAMLProfessionsByQueryStringRequest.IsActive
                )
                .Where(x =>
                    string.IsNullOrEmpty(getAMLProfessionsByQueryStringRequest.Version)
                    || x.Version == getAMLProfessionsByQueryStringRequest.Version
                )
                .OrderByDescending(x => x.Version)
                .ToListAsync();

            entities.Sort(
                (x, y) =>
                {
                    return x.Version == y.Version
                        ? int.Parse(x.AMLProfessionCode).CompareTo(int.Parse(y.AMLProfessionCode))
                        : y.Version.CompareTo(x.Version);
                }
            );

            var result = _mapper.Map<List<GetAMLProfessionsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
