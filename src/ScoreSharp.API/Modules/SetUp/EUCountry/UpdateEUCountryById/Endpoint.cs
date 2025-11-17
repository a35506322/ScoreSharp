using ScoreSharp.API.Modules.SetUp.EUCountry.UpdateEUCountryById;

namespace ScoreSharp.API.Modules.SetUp.EUCountry
{
    public partial class EUCountryController
    {
        /// <summary>
        /// 修改單筆EU國家
        /// </summary>
        /// <param name="code">EU國家代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改EU國家_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改EU國家_2000_ResEx),
            typeof(修改EU國家查無資料_4001_ResEx),
            typeof(修改EU國家路由與Req比對錯誤_4003_ResEx),
            typeof(修改EU國家查無國籍設定資料_4003_ResEx),
            typeof(修改EU國家名稱與國籍設定不相同_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateEUCountryById")]
        public async Task<IResult> UpdateEUCountryById([FromRoute] string code, [FromBody] UpdateEUCountryByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.EUCountry.UpdateEUCountryById
{
    public record Command(string code, UpdateEUCountryByIdRequest updateEUCountryByIdRequest) : IRequest<ResultResponse<string>>;

    public class Handler : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Handler(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateEUCountryByIdRequest = request.updateEUCountryByIdRequest;

            if (request.code != updateEUCountryByIdRequest.EUCountryCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_EUCountry.SingleOrDefaultAsync(x => x.EUCountryCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            var citizenship = await _context.SetUp_Citizenship.AsNoTracking().SingleOrDefaultAsync(x => x.CitizenshipCode == request.code);

            if (citizenship is null)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "查無國籍設定資料，請檢查");

            if (citizenship.CitizenshipName != updateEUCountryByIdRequest.EUCountryName)
                return ApiResponseHelper.BusinessLogicFailed<string>(null, "名稱與國籍設定不相同，請檢查");

            entity.IsActive = updateEUCountryByIdRequest.IsActive;
            entity.EUCountryName = updateEUCountryByIdRequest.EUCountryName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
