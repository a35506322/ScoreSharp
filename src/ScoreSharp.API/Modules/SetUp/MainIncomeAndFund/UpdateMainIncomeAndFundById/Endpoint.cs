using ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.UpdateMainIncomeAndFundById;

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund
{
    public partial class MainIncomeAndFundController
    {
        /// <summary>
        /// 單筆修改主要所得及資金來源
        /// </summary>
        /// <param name="code">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改主要所得及資金來源_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改主要所得及資金來源_2000_ResEx),
            typeof(修改主要所得及資金來源查無此資料_4001_ResEx),
            typeof(修改主要所得及資金來源路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateMainIncomeAndFundById")]
        public async Task<IResult> UpdateMainIncomeAndFundById(
            [FromRoute] string code,
            [FromBody] UpdateMainIncomeAndFundByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.MainIncomeAndFund.UpdateMainIncomeAndFundById
{
    public record Command(string code, UpdateMainIncomeAndFundByIdRequest updateMainIncomeAndFundByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Hanlder : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Hanlder(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updateMainIncomeAndFundByIdRequest = request.updateMainIncomeAndFundByIdRequest;

            if (request.code != updateMainIncomeAndFundByIdRequest.MainIncomeAndFundCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var entity = await _context.SetUp_MainIncomeAndFund.SingleOrDefaultAsync(x => x.MainIncomeAndFundCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            entity.IsActive = updateMainIncomeAndFundByIdRequest.IsActive;
            entity.MainIncomeAndFundName = updateMainIncomeAndFundByIdRequest.MainIncomeAndFundName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess(request.code, request.code);
        }
    }
}
