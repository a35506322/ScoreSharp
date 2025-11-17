using ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationByQueryString;

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation
{
    public partial class IDCardRenewalLocationController
    {
        /// <summary>
        /// 查詢多筆身分證換發地點
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///       ?IsActive=Y&amp;IDCardRenewalLocationName=連&amp;IDCardRenewalLocationCode=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetIDCardRenewalLocationByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得身分證換發地點_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetIDCardRenewalLocationByQueryString")]
        public async Task<IResult> GetIDCardRenewalLocationByQueryString([FromQuery] GetIDCardRenewalLocationByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.IDCardRenewalLocation.GetIDCardRenewalLocationByQueryString
{
    public record Query(GetIDCardRenewalLocationByQueryStringRequest getIDCardRenewalLocationByQueryStringRequest)
        : IRequest<ResultResponse<List<GetIDCardRenewalLocationByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetIDCardRenewalLocationByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetIDCardRenewalLocationByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getIDCardRenewalLocationByQueryStringRequest = request.getIDCardRenewalLocationByQueryStringRequest;

            var entities = await _context
                .SetUp_IDCardRenewalLocation.Where(x =>
                    string.IsNullOrEmpty(getIDCardRenewalLocationByQueryStringRequest.IDCardRenewalLocationName)
                    || x.IDCardRenewalLocationName.Contains(getIDCardRenewalLocationByQueryStringRequest.IDCardRenewalLocationName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getIDCardRenewalLocationByQueryStringRequest.IDCardRenewalLocationCode)
                    || x.IDCardRenewalLocationCode == getIDCardRenewalLocationByQueryStringRequest.IDCardRenewalLocationCode
                )
                .Where(x =>
                    string.IsNullOrEmpty(getIDCardRenewalLocationByQueryStringRequest.IsActive)
                    || x.IsActive == getIDCardRenewalLocationByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var response = _mapper.Map<List<GetIDCardRenewalLocationByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(response);
        }
    }
}
