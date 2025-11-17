using ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfoById;

namespace ScoreSharp.API.Modules.SetUp.AddressInfo
{
    public partial class AddressInfoController
    {
        /// <summary>
        /// 查詢單筆地址資訊
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /AddressInfo/GetAddressInfoById/72424
        ///
        /// </remarks>
        /// <param name="seqNo">流水號</param>
        /// <returns></returns>
        /// <response code="200">查詢成功返回地址資訊</response>
        [HttpGet("{seqNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetAddressInfoByIdResponse>))]
        [EndpointSpecificExample(
            typeof(取得單筆地址資訊_2000_ResEx),
            typeof(取得單筆地址資訊查無此資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAddressInfoById")]
        public async Task<IResult> GetAddressInfoById([FromRoute] string seqNo)
        {
            var result = await _mediator.Send(new Query(seqNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AddressInfo.GetAddressInfoById
{
    public record Query(string seqNo) : IRequest<ResultResponse<GetAddressInfoByIdResponse>>;

    public class Handler(ScoreSharpContext context, IMapper mapper) : IRequestHandler<Query, ResultResponse<GetAddressInfoByIdResponse>>
    {
        public async Task<ResultResponse<GetAddressInfoByIdResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var entity = await context.SetUp_AddressInfo.AsNoTracking().SingleOrDefaultAsync(x => x.SeqNo == request.seqNo);

            if (entity is null)
                return ApiResponseHelper.NotFound<GetAddressInfoByIdResponse>(null, request.seqNo);

            var result = mapper.Map<GetAddressInfoByIdResponse>(entity);

            return ApiResponseHelper.Success(result);
        }
    }
}
