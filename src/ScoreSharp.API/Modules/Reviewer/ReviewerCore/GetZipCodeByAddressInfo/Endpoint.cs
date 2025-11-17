using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetZipCodeByAddressInfo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得郵遞區號 By 地址資訊 (還有多種狀況未考慮先將就用)
        /// </summary>
        /// <remarks>
        /// 1. 這 Request 是從 Body 帶資訊
        /// 2. 這隻只要查找不到就是帶空值且狀態為 4001
        /// 3. 觸發條件可以設定為縣市、鄉鎮市區、路名、巷、門牌號碼、門牌號碼2
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [OpenApiOperation("GetZipCodeByAddressInfo")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(查詢郵遞區號_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(查詢郵遞區號_2000_ResEx),
            typeof(查詢郵遞區號查無資料_4001_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        public async Task<IResult> GetZipCodeByAddressInfo([FromBody] GetZipCodeByAddressInfoRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetZipCodeByAddressInfo
{
    public record Query(GetZipCodeByAddressInfoRequest getZipCodeByAddressInfoRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, ILogger<Handler> _logger) : IRequestHandler<Query, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Query request, CancellationToken cancellationToken)
        {
            var _request = request.getZipCodeByAddressInfoRequest;

            try
            {
                var addressInfos = await _context
                    .SetUp_AddressInfo.AsNoTracking()
                    .Where(x => x.City == _request.City && x.Area == _request.District && x.Road == _request.Road)
                    .ToListAsync(cancellationToken);

                var addressInfoDtos = addressInfos
                    .Select(x => new AddressInfoDto()
                    {
                        City = x.City,
                        Area = x.Area,
                        Road = x.Road,
                        Scope = x.Scope,
                        ZipCode = x.ZIPCode,
                    })
                    .ToList();

                var searchAddressInfo = new SearchAddressInfoDto()
                {
                    City = _request.City,
                    District = _request.District,
                    Road = _request.Road,
                    Number = _request.Number,
                    SubNumber = _request.SubNumber,
                    Lane = _request.Lane,
                };

                var zipCode = AddressHelper.FindZipCode(addressInfoDtos, searchAddressInfo);

                if (string.IsNullOrEmpty(zipCode))
                {
                    _logger.LogError("查詢郵遞區號失敗，查無資料，request: {@request}", _request);
                    return ApiResponseHelper.NotFound<string>(
                        String.Empty,
                        $"{_request.City}_{_request.District}_{_request.Road}_{_request.Number}_{_request.SubNumber}_{_request.Lane}"
                    );
                }

                return ApiResponseHelper.Success(zipCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "查詢郵遞區號失敗，request: {@request}，error: {@error}", _request, ex.ToString());
                return ApiResponseHelper.NotFound<string>(
                    String.Empty,
                    $"{_request.City}_{_request.District}_{_request.Road}_{_request.Number}_{_request.SubNumber}_{_request.Lane}"
                );
            }
        }
    }
}
