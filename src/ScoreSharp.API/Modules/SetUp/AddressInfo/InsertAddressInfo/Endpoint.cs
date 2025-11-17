using ScoreSharp.API.Modules.SetUp.AddressInfo.InsertAddressInfo;

namespace ScoreSharp.API.Modules.SetUp.AddressInfo
{
    public partial class AddressInfoController
    {
        /// <summary>
        /// 新增單筆地址資訊
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     {
        ///       "zipCode": "247",
        ///       "city": "新北市",
        ///       "area": "蘆洲區",
        ///       "road": "長安街",
        ///       "scope": "1-99 號單號",
        ///       "isActive": "Y"
        ///     }
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">新增成功返回流水號</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(新增地址資訊_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(新增地址資訊_2000_ResEx),
            typeof(新增地址資訊資料已存在_4002_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("InsertAddressInfo")]
        public async Task<IResult> InsertAddressInfo([FromBody] InsertAddressInfoRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AddressInfo.InsertAddressInfo
{
    public record Command(InsertAddressInfoRequest insertAddressInfoRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.insertAddressInfoRequest;

            // 檢查是否已存在相同的地址資訊
            var existingEntity = await context.SetUp_AddressInfo.SingleOrDefaultAsync(x =>
                x.ZIPCode == dto.ZIPCode && x.City == dto.City && x.Area == dto.Area && x.Road == dto.Road
            );

            if (existingEntity is not null)
                return ApiResponseHelper.DataAlreadyExists<string>(null, $"{dto.ZIPCode}-{dto.City}-{dto.Area}-{dto.Road}");

            // 產生流水號 SeqNo
            var seqNo = await GenerateSeqNo();

            var newEntity = new SetUp_AddressInfo
            {
                SeqNo = seqNo,
                ZIPCode = dto.ZIPCode,
                City = dto.City,
                Area = dto.Area,
                Road = dto.Road,
                Scope = dto.Scope,
                IsActive = dto.IsActive,
            };

            await context.SetUp_AddressInfo.AddAsync(newEntity);
            await context.SaveChangesAsync();

            return ApiResponseHelper.InsertSuccess(seqNo, seqNo);
        }

        private async Task<string> GenerateSeqNo()
        {
            // 取得最後一個序號並 +1
            var lastSeqNo = await context.SetUp_AddressInfo.OrderByDescending(x => x.SeqNo).Select(x => x.SeqNo).FirstOrDefaultAsync();
            if (String.IsNullOrEmpty(lastSeqNo))
                throw new Exception("沒有資料");

            // 將字串轉換為數字後 +1
            if (int.TryParse(lastSeqNo, out int currentSeqNo))
            {
                return (currentSeqNo + 1).ToString();
            }

            throw new Exception($"轉換失敗: {lastSeqNo}");
        }
    }
}
