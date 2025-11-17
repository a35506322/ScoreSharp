using ScoreSharp.API.Modules.Report.GetBatchReportDownloadByQueryString;

namespace ScoreSharp.API.Modules.Report.BatchReportDownload
{
    public partial class BatchReportDownloadController
    {
        /// <summary>
        /// 查詢批次報表下載 By QueryString
        ///
        /// <remarks>
        /// Sample QueryString:
        ///
        ///     /BatchReportDownload/GetBatchReportDownloadByQueryString?StartTime=2024-03-21&EndTime=2024-03-22&ReportType=1
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetBatchReportDownloadByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢批次報表下載_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetBatchReportDownloadByQueryString")]
        public async Task<IResult> GetBatchReportDownloadByQueryString([FromQuery] GetBatchReportDownloadByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Report.GetBatchReportDownloadByQueryString
{
    public record Query(GetBatchReportDownloadByQueryStringRequest Request)
        : IRequest<ResultResponse<List<GetBatchReportDownloadByQueryStringResponse>>>;

    public class Handler(IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Query, ResultResponse<List<GetBatchReportDownloadByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetBatchReportDownloadByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            using var connection = dapperContext.CreateScoreSharpConnection();

            var sql =
                @"
            SELECT r.*, u.UserName AS LastDownloadUserName
            FROM Report_BatchReportDownload r
            LEFT JOIN OrgSetUp_User u ON r.LastDownloadUserId = u.UserId
            WHERE 
            CAST(r.AddTime AS DATE) BETWEEN CAST(@StartTime AS DATE) AND CAST(@EndTime AS DATE)
            AND r.ReportType = @ReportType";

            var result = await connection.QueryAsync<GetBatchReportDownloadByQueryStringResponse>(sql, request.Request);

            return ApiResponseHelper.Success(result.ToList());
        }
    }
}
