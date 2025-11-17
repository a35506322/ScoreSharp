using ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfosByQueryString;

namespace ScoreSharp.API.Modules.SetUp.AddressInfo
{
    public partial class AddressInfoController
    {
        /// <summary>
        /// 查詢多筆地址資訊 ByQueryString
        /// </summary>
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     ?City=新北市&amp;IsActive=Y
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetAddressInfosByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得多筆地址資訊_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAddressInfosByQueryString")]
        public async Task<IResult> GetAddressInfosByQueryString([FromQuery] GetAddressInfosByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfosByQueryString
{
    public record Query(GetAddressInfosByQueryStringRequest getAddressInfosByQueryStringRequest)
        : IRequest<ResultResponse<List<GetAddressInfosByQueryStringResponse>>>;

    public class Handler(ScoreSharpContext context, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetAddressInfosByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetAddressInfosByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var queryRequest = request.getAddressInfosByQueryStringRequest;

            var entity = await context
                .SetUp_AddressInfo.AsNoTracking()
                .Where(x =>
                    (String.IsNullOrWhiteSpace(queryRequest.ZIPCode) || x.ZIPCode == queryRequest.ZIPCode)
                    && (String.IsNullOrWhiteSpace(queryRequest.City) || x.City == queryRequest.City)
                    && (String.IsNullOrWhiteSpace(queryRequest.Area) || x.Area == queryRequest.Area)
                    && (String.IsNullOrWhiteSpace(queryRequest.Road) || x.Road == queryRequest.Road)
                    && (String.IsNullOrWhiteSpace(queryRequest.IsActive) || x.IsActive == queryRequest.IsActive)
                )
                .ToListAsync();

            var result = mapper.Map<List<GetAddressInfosByQueryStringResponse>>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
