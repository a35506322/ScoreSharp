using ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.UpdateHighFinancialSecrecyCountryById;

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry
{
    public partial class HighFinancialSecrecyCountryController
    {
        /// <summary>
        /// 單筆修改高金融保密國家
        /// </summary>
        /// <param name="code">國籍代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse))]
        [EndpointSpecificExample(typeof(單筆修改高金融保密國家_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(單筆修改高金融保密國家_2000_ResEx),
            typeof(單筆修改高金融保密國家查無資料_4001_ResEx),
            typeof(單筆修改高金融保密國家路由與Req比對錯誤_4003_ResEx),
            typeof(單筆修改高金融保密國家查無國籍設定資料_2000_ResEx),
            typeof(單筆修改高金融保密國家名稱與國籍設定不相同_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateHighFinancialSecrecyCountryById")]
        public async Task<IResult> UpdateHighFinancialSecrecyCountryById(
            [FromRoute] string code,
            [FromBody] UpdateHighFinancialSecrecyCountryByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.HighFinancialSecrecyCountry.UpdateHighFinancialSecrecyCountryById
{
    public record Command(string code, UpdateHighFinancialSecrecyCountryByIdRequest updateHighFinancialSecrecyCountryByIdRequest)
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
            var updateHighFinancialSecrecyCountryByIdRequest = request.updateHighFinancialSecrecyCountryByIdRequest;

            if (updateHighFinancialSecrecyCountryByIdRequest.HighFinancialSecrecyCountryCode != request.code)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_HighFinancialSecrecyCountry.SingleOrDefaultAsync(x =>
                x.HighFinancialSecrecyCountryCode == request.code
            );

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var citizenship = await _context.SetUp_Citizenship.AsNoTracking().SingleOrDefaultAsync(x => x.CitizenshipCode == request.code);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, $"查無國籍設定資料，請檢查");

            if (citizenship.CitizenshipName != updateHighFinancialSecrecyCountryByIdRequest.HighFinancialSecrecyCountryName)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "名稱與國籍設定不相同，請檢查");

            entity.HighFinancialSecrecyCountryName = updateHighFinancialSecrecyCountryByIdRequest.HighFinancialSecrecyCountryName;
            entity.IsActive = updateHighFinancialSecrecyCountryByIdRequest.IsActive;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
