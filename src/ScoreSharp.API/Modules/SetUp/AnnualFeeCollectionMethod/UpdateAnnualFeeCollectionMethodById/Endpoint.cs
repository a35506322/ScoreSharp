using ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.UpdateAnnualFeeCollectionMethodById;

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod
{
    public partial class AnnualFeeCollectionMethodController
    {
        /// <summary>
        /// 修改單筆年費收取方式
        /// </summary>
        /// <param name="code">年費收取代碼</param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("{code}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新年費收取方式_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(
            typeof(更新年費收取方式_2000_ResEx),
            typeof(更新年費收取方式查無資料_4001_ResEx),
            typeof(更新年費收取方式路由與Req比對錯誤_4003_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("UpdateAnnualFeeCollectionMethodById")]
        public async Task<IResult> UpdateAnnualFeeCollectionMethodById(
            [FromRoute] string code,
            [FromBody] UpdateAnnualFeeCollectionMethodByIdRequest request
        )
        {
            var result = await _mediator.Send(new Command(code, request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.AnnualFeeCollectionMethod.UpdateAnnualFeeCollectionMethodById
{
    public record Command(string code, UpdateAnnualFeeCollectionMethodByIdRequest updateAnnualFeeCollectionMethodByIdRequest)
        : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext _context, IJWTProfilerHelper _jwtHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.updateAnnualFeeCollectionMethodByIdRequest;
            if (dto.AnnualFeeCollectionCode != request.code)
                return ApiResponseHelper.路由與Req比對錯誤<string>(null);

            var existingEntity = await _context.SetUp_AnnualFeeCollectionMethod.SingleOrDefaultAsync(x => x.AnnualFeeCollectionCode == request.code);

            if (existingEntity is null)
                return ApiResponseHelper.NotFound<string>(null, request.code);

            // 更新屬性
            existingEntity.AnnualFeeCollectionName = dto.AnnualFeeCollectionName;
            existingEntity.IsActive = dto.IsActive;
            existingEntity.UpdateUserId = _jwtHelper.UserId;
            existingEntity.UpdateTime = DateTime.Now;

            await _context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.UpdateByIdSuccess<string>(request.code, request.code);
        }
    }
}
