using ScoreSharp.API.Modules.Reviewer.Common.GetAddressInfoOptions;
using ScoreSharp.API.Modules.Reviewer.Helpers;

namespace ScoreSharp.API.Modules.Reviewer.Common
{
    public partial class ReviewerCommonController
    {
        /// <summary>
        /// 取得地址相關參數，目前此資料由資訊科匯入，並無CRUD
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /ReviewerCommon/GetAddressInfoOptions?IsActive=Y
        ///
        ///     對照表
        ///     縣市 = City
        ///     區 = Area
        ///     街道 = Road
        /// </remarks>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetAddressInfoOptionsResponse>))]
        [EndpointSpecificExample(
            typeof(取得地址相關下拉選單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetAddressInfoOptions")]
        public async Task<IResult> GetAddressInfoOptions([FromQuery] string? isActive)
        {
            var result = await _mediator.Send(new Query(isActive));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.Common.GetAddressInfoOptions
{
    public record Query(string? isActive) : IRequest<ResultResponse<GetAddressInfoOptionsResponse>>;

    public class Handler(IReviewerHelper reviewerHelper, IFusionCache fusionCache)
        : IRequestHandler<Query, ResultResponse<GetAddressInfoOptionsResponse>>
    {
        public async Task<ResultResponse<GetAddressInfoOptionsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var cacheKey = RedisKeyConst.GetAddressInfoOptions;
            var cacheData = await fusionCache.GetOrSetAsync(
                cacheKey,
                async (cacheEntry) =>
                {
                    var dto = await reviewerHelper.GetAddressInfoParams(request.isActive);
                    return new GetAddressInfoOptionsResponse
                    {
                        City = dto.City,
                        Area = dto.Area,
                        Road = dto.Road,
                    };
                },
                new FusionCacheEntryOptions { Duration = TimeSpan.FromDays(1), IsFailSafeEnabled = false },
                cancellationToken
            );

            return ApiResponseHelper.Success(cacheData);
        }
    }
}
