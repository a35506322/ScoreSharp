using ScoreSharp.API.Modules.SetUp.FixTime.UpdateFixTime;

namespace ScoreSharp.API.Modules.SetUp.FixTime
{
    public partial class FixTimeController
    {
        /// <summary>
        /// 更新維護時段設定
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     {
        ///       "KYC_IsFix": "Y",
        ///       "KYC_StartTime": "2025-01-01T00:00:00",
        ///       "KYC_EndTime": "2025-01-01T06:00:00"
        ///     }
        ///
        /// 驗證規則：
        /// <ul>
        ///     <li>當KYC維護設為Y時，開始時間與結束時間為必填</li>
        ///     <li>開始時間不能大於結束時間</li>
        /// </ul>
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        /// <response code="200">更新成功</response>
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(typeof(更新維護時段_2000_ReqEx), ParameterName = "request", ExampleType = ExampleType.Request)]
        [EndpointSpecificExample(typeof(更新維護時段_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("UpdateFixTime")]
        public async Task<IResult> UpdateFixTime([FromBody] UpdateFixTimeRequest request)
        {
            var result = await _mediator.Send(new Command(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SetUp.FixTime.UpdateFixTime
{
    public record Command(UpdateFixTimeRequest updateFixTimeRequest) : IRequest<ResultResponse<string>>;

    public class Handler(ScoreSharpContext context, IJWTProfilerHelper jwtProfilerHelper) : IRequestHandler<Command, ResultResponse<string>>
    {
        public async Task<ResultResponse<string>> Handle(Command request, CancellationToken cancellationToken)
        {
            var dto = request.updateFixTimeRequest;

            var existingEntity = await context.SetUp_FixTime.FirstOrDefaultAsync(cancellationToken);

            if (existingEntity is null)
            {
                return ApiResponseHelper.NotFound<string>(null, "查無資料");
            }

            existingEntity.KYC_IsFix = dto.KYC_IsFix;
            existingEntity.KYC_StartTime = dto.KYC_StartTime;
            existingEntity.KYC_EndTime = dto.KYC_EndTime;
            existingEntity.UpdateUserId = jwtProfilerHelper.UserId;
            existingEntity.UpdateTime = DateTime.Now;
            await context.SaveChangesAsync(cancellationToken);

            return ApiResponseHelper.Success<string>(null, "維護時段設定已更新");
        }
    }
}
