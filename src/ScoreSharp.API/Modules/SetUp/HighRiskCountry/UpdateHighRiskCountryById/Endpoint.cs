using ScoreSharp.API.Modules.SetUp.HighRiskCountry.UpdateHighRiskCountryById;

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry
{
    public partial class HighRiskCountryController
    {
        /// <summary>
        /// 單筆修改洗錢及資恐高風險國家
        /// </summary>
        /// <param name="code">國籍代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(
            typeof(單筆修改洗錢及資恐高風險國家_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(單筆修改洗錢及資恐高風險國家_2000_ResEx),
            typeof(修改洗錢及資恐高風險國家查無資料_4001_ResEx),
            typeof(修改洗錢及資恐高風險國家路由與Req比對錯誤_4003_ResEx),
            typeof(修改洗錢及資恐高風險國家查無國籍設定資料_4003_ResEx),
            typeof(修改洗錢及資恐高風險國家名稱與國籍設定不相同_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateHighRiskCountryById")]
        public async Task<IResult> UpdateHighRiskCountryById([FromRoute] string code, [FromBody] UpdateHighRiskCountryByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighRiskCountry.UpdateHighRiskCountryById
{
    public record Command(string code, UpdateHighRiskCountryByIdRequest updateCitizenshipByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateHighRiskCountryByIdRequest = request.updateCitizenshipByIdRequest;

            if (updateHighRiskCountryByIdRequest.HighRiskCountryCode != request.code)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_HighRiskCountry.SingleOrDefaultAsync(x => x.HighRiskCountryCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var citizenship = await _context.SetUp_Citizenship.AsNoTracking().SingleOrDefaultAsync(x => x.CitizenshipCode == request.code);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"查無國籍設定資料，請檢查");

            if (citizenship.CitizenshipName != updateHighRiskCountryByIdRequest.HighRiskCountryName)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "名稱與國籍設定不相同，請檢查");

            entity.HighRiskCountryName = updateHighRiskCountryByIdRequest.HighRiskCountryName;
            entity.IsActive = updateHighRiskCountryByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
