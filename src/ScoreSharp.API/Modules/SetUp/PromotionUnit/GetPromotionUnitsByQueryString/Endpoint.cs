using ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitsByQueryString;

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit
{
    public partial class PromotionUnitController
    {
        /// <summary>
        /// 查詢多筆推廣單位
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;PromotionUnitName=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetPromotionUnitsByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得推廣單位_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetPromotionUnitsByQueryString")]
        public async Task<IResult> GetPromotionUnitsByQueryString([FromQuery] GetPromotionUnitsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.GetPromotionUnitsByQueryString
{
    public record Query(GetPromotionUnitsByQueryStringRequest getPromotionUnitsByQueryStringRequest)
        : IRequest<ResultResponse<List<GetPromotionUnitsByQueryStringResponse>>>;

    public class Handler : IRequestHandler<Query, ResultResponse<List<GetPromotionUnitsByQueryStringResponse>>>
    {
        private readonly ScoreSharpContext _context;
        private readonly IMapper _mapper;

        public Handler(ScoreSharpContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ResultResponse<List<GetPromotionUnitsByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var getPromotionUnitsByQueryStringRequest = request.getPromotionUnitsByQueryStringRequest;

            var entities = await _context
                .SetUp_PromotionUnit.Where(x =>
                    string.IsNullOrEmpty(getPromotionUnitsByQueryStringRequest.PromotionUnitName)
                    || x.PromotionUnitName.Contains(getPromotionUnitsByQueryStringRequest.PromotionUnitName)
                )
                .Where(x =>
                    string.IsNullOrEmpty(getPromotionUnitsByQueryStringRequest.IsActive)
                    || x.IsActive == getPromotionUnitsByQueryStringRequest.IsActive
                )
                .ToListAsync();

            var result = _mapper.Map<List<GetPromotionUnitsByQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
