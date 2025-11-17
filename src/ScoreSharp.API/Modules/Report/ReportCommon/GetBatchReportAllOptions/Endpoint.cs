using ScoreSharp.API.Modules.Report.ReportCommon.GetBatchReportAllOptions;

namespace ScoreSharp.API.Modules.Report.ReportCommon
{
    public partial class ReportCommonController
    {
        /// <summary>
        /// 取得報表相關所有的下拉選單
        /// </summary>
        /// <remarks>
        ///
        ///     Sample Router: /ReportCommon/GetBatchReportAllOptions?IsActive=Y
        ///
        ///     對照表
        ///     報表類型 = ReportType
        /// </remarks>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetBatchReportAllOptionsResponse>))]
        [EndpointSpecificExample(
            typeof(取得全部報表相關下拉選單_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetBatchReportAllOptions")]
        public async Task<IResult> GetBatchReportAllOptions([FromQuery] string? isActive)
        {
            var result = await _mediator.Send(new Query(isActive));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Report.ReportCommon.GetBatchReportAllOptions
{
    public record Query(string? isActive) : IRequest<ResultResponse<GetBatchReportAllOptionsResponse>>;

    public class Handler : IRequestHandler<Query, ResultResponse<GetBatchReportAllOptionsResponse>>
    {
        public async Task<ResultResponse<GetBatchReportAllOptionsResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var reportTypeOptions = EnumExtenstions.GetEnumOptions<ReportType>(request.isActive);

            GetBatchReportAllOptionsResponse response = new() { ReportType = reportTypeOptions };

            return ApiResponseHelper.Success(response);
        }
    }
}
