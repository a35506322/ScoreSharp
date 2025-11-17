using ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.Citizenship
{
    public partial class CitizenshipController
    {
        /// <summary>
        /// 查詢多筆國籍
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;CitizenshipName=國&amp;CitizenshipCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetCitizenshipsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得國籍_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCitizenshipsByQueryString")]
        public async Task<IResult> GetCitizenshipsByQueryString([FromQuery] GetCitizenshipsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.Citizenship.GetCitizenshipsByQueryString
{
    public record Query(GetCitizenshipsByQueryStringRequest getCitizenshipsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetCitizenshipsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetCitizenshipsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetCitizenshipsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getCitizenshipsByQueryStringRequest = request.getCitizenshipsByQueryStringRequest;

            var entites = await _context
                .SetUp_Citizenship.Where(x =>
                    string.IsNullOrEmpty(getCitizenshipsByQueryStringRequest.CitizenshipCode)
                    || x.CitizenshipCode == getCitizenshipsByQueryStringRequest.CitizenshipCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getCitizenshipsByQueryStringRequest.CitizenshipName)
                    || x.CitizenshipName.Contains(getCitizenshipsByQueryStringRequest.CitizenshipName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getCitizenshipsByQueryStringRequest.IsActive)
                    || x.IsActive == getCitizenshipsByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetCitizenshipsByQueryStringResponse>>(entites);

            return ApiResponseHelper.Success(result);
        }
    }
}
