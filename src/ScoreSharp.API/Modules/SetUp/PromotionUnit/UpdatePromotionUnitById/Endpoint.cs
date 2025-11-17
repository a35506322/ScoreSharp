using ScoreSharp.API.Modules.SetUp.PromotionUnit.UpdatePromotionUnitById;

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit
{
    public partial class PromotionUnitController
    {
        /// <summary>
        /// 修改單筆推廣單位
        /// </summary>
        /// <param name="code">PK</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(修改推廣單位_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(修改推廣單位_2000_ResEx),
            typeof(修改推廣單位查無此資料_4001_ResEx),
            typeof(修改推廣單位路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdatePromotionUnitById")]
        public async Task<IResult> UpdatePromotionUnitById([FromRoute] string code, [FromBody] UpdatePromotionUnitByIdRequest request)
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.PromotionUnit.UpdatePromotionUnitById
{
    public record Command(string code, UpdatePromotionUnitByIdRequest updatePromotionUnitByIdRequest) : IRequest<ResultResponse<string>>;

    public class Hanlder : IRequestHandler<Command, ResultResponse<string>>
    {
        private readonly ScoreSharpContext _context;

        public Hanlder(ScoreSharpContext context)
        {
            _context = context;
        }

        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var updatePromotionUnitByIdRequest = request.updatePromotionUnitByIdRequest;

            if (request.code != updatePromotionUnitByIdRequest.PromotionUnitCode)
                return ApiResponseHelper.路由與Req比對錯誤<string>(request.code);

            var entity = await _context.SetUp_PromotionUnit.SingleOrDefaultAsync(x => x.PromotionUnitCode == request.code);

            if (entity is null)
                return ApiResponseHelper.NotFound<string>(request.code, request.code.ToString());

            entity.IsActive = updatePromotionUnitByIdRequest.IsActive;
            entity.PromotionUnitName = updatePromotionUnitByIdRequest.PromotionUnitName;

            await _context.SaveChangesAsync();

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code.ToString());
        }
    }
}
