using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodQueryString;

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod
{
    public partial class AnnualFeeCollectionMethodController
    {
        /// <summary>
        /// 查詢年費收取方式
        /// </summary>
        /// <param name="request"></param>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?IsActive=Y&amp;AnnualFeeCollectionName=
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetAnnualFeeCollectionMethodQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(取得年費收取方式_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAnnualFeeCollectionMethodQueryString")]
        public async Task<IResult> GetAnnualFeeCollectionMethodQueryString([FromQuery] GetAnnualFeeCollectionMethodQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodQueryString
{
    public record Query(GetAnnualFeeCollectionMethodQueryStringRequest getAnnualFeeCollectionMethodQueryStringRequest)
        : IRequest<ResultResponse<List<GetAnnualFeeCollectionMethodQueryStringResponse>>>;

    public class Handler(ScoreSharpContext _context, IMapper _mapper)
        : IRequestHandler<Query, ResultResponse<List<GetAnnualFeeCollectionMethodQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetAnnualFeeCollectionMethodQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var dto = request.getAnnualFeeCollectionMethodQueryStringRequest;

            var entities = await _context
                .SetUp_AnnualFeeCollectionMethod.Where(x => (string.IsNullOrEmpty(dto.IsActive) || x.IsActive == dto.IsActive))
                .Where(x => (string.IsNullOrEmpty(dto.AnnualFeeCollectionName) || x.AnnualFeeCollectionName.Contains(dto.AnnualFeeCollectionName)))
                .OrderBy(x => x.AnnualFeeCollectionCode)
                .ToListAsync();

            var result = _mapper.Map<List<GetAnnualFeeCollectionMethodQueryStringResponse>>(entities);

            return ApiResponseHelper.Success(result);
        }
    }
}
