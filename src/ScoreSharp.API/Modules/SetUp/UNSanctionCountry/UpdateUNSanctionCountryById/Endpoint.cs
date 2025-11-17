using ScoreSharp.API.Modules.SetUp.UNSanctionCountry.UpdateUNSanctionCountryById;

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry
{
    public partial class UNSanctionCountryController
    {
        /// <summary>
        /// 單筆修改UN制裁國家
        /// </summary>
        /// <param name="code">UN制裁國家代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改UN制裁國家_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改UN制裁國家_2000_ResEx),
            typeof(修改UN制裁國家查無資料_4001_ResEx),
            typeof(修改UN制裁國家路由與Req比對錯誤_4003_ResEx),
            typeof(修改UN制裁國家查無國籍設定資料_4003_ResEx),
            typeof(修改UN制裁國家名稱與國籍設定不相同_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateUNSanctionCountryById")]
        public async Task<IResult> UpdateUNSanctionCountryById(
            [FromRoute] string code,
            [FromBody] UpdateUNSanctionCountryByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.UNSanctionCountry.UpdateUNSanctionCountryById
{
    public record Command(string code, UpdateUNSanctionCountryByIdRequest updateUNSanctionCountryByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateUNSanctionCountryByIdRequest = request.updateUNSanctionCountryByIdRequest;

            if (request.code != updateUNSanctionCountryByIdRequest.UNSanctionCountryCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_UNSanctionCountry.SingleOrDefaultAsync(x => x.UNSanctionCountryCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var citizenship = await _context.SetUp_Citizenship.AsNoTracking().SingleOrDefaultAsync(x => x.CitizenshipCode == request.code);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            if (citizenship.CitizenshipName != updateUNSanctionCountryByIdRequest.UNSanctionCountryName)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "名稱與國籍設定不相同，請檢查");

            entity.IsActive = updateUNSanctionCountryByIdRequest.IsActive;
            entity.UNSanctionCountryName = updateUNSanctionCountryByIdRequest.UNSanctionCountryName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
