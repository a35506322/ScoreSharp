using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodId;

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod
{
    public partial class AnnualFeeCollectionMethodController
    {
        /// <summary>
        /// 查詢單筆年費收取方式
        /// </summary>
        /// <remarks>
        ///
        /// Sample Routers:
        ///
        ///     /AnnualFeeCollectionMethod/GetAnnualFeeCollectionMethodId/01
        ///
        /// </remarks>
        /// <param name="code">年費收取代碼</param>
        /// <returns></returns>
        [HttpGet("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetAnnualFeeCollectionMethodIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得年費收取方式_2000_ResEx),
            typeof(取得年費收取方式查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAnnualFeeCollectionMethodId")]
        public async Task<IResult> GetAnnualFeeCollectionMethodId([FromRoute] string code)
        {
            var result = await _mediator.Send(new Query(code));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.GetAnnualFeeCollectionMethodId
{
    public record Query(string code) : IRequest<ResultResponse<GetAnnualFeeCollectionMethodIdResponse>>;

    public class Handler(ScoreSharpContext _context, IMapper _mapper) : IRequestHandler<Query, ResultResponse<GetAnnualFeeCollectionMethodIdResponse>>
    {
        public async Task<ResultResponse<GetAnnualFeeCollectionMethodIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var single = await _context
                .SetUp_AnnualFeeCollectionMethod.AsNoTracking()
                .SingleOrDefaultAsync(x => x.AnnualFeeCollectionCode == request.code);

            if (single is null)
                return ApiResponseHelper.NotFound<GetAnnualFeeCollectionMethodIdResponse>(null, request.code.ToString());

            var result = _mapper.Map<GetAnnualFeeCollectionMethodIdResponse>(single);

            return ApiResponseHelper.Success(result);
        }
    }
}
