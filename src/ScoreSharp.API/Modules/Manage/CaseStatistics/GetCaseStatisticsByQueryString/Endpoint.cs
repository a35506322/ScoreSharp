using ScoreSharp.API.Modules.Manage.CaseStatistics.GetCaseStatisticsByQueryString;

namespace ScoreSharp.API.Modules.Manage.CaseStatistics
{
    public partial class CaseStatisticsController
    {
        /// <summary>
        /// 查詢派案統計並匯出
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /CaseStatistics/GetCaseStatisticsByQueryString
        ///
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<GetCaseStatisticsByQueryStringResponse>))]
        [EndpointSpecificExample(
            typeof(取得派案統計並匯出_查詢資料_2000_ReqEx),
            typeof(取得派案統計並匯出_匯出資料_2000_ReqEx),
            ParameterName = "request",
            ExampleType = ExampleType.Request
        )]
        [EndpointSpecificExample(
            typeof(取得派案統計並匯出_查詢成功_2000_ResEx),
            typeof(取得派案統計並匯出_匯出成功_2000_ResEx),
            typeof(取得派案統計並匯出_查無資料_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCaseStatisticsByQueryString")]
        public async Task<IResult> GetCaseStatisticsByQueryString([FromQuery] GetCaseStatisticsByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Manage.CaseStatistics.GetCaseStatisticsByQueryString
{
    public record Query(GetCaseStatisticsByQueryStringRequest getCaseStatisticsByQueryStringRequest)
        : IRequest<ResultResponse<GetCaseStatisticsByQueryStringResponse>>;

    public class Handler(ScoreSharpContext context, IMiniExcelHelper miniExcelHelper)
        : IRequestHandler<Query, ResultResponse<GetCaseStatisticsByQueryStringResponse>>
    {
        public async Task<ResultResponse<GetCaseStatisticsByQueryStringResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var userId = request.getCaseStatisticsByQueryStringRequest.UserId;
            var addTime = request.getCaseStatisticsByQueryStringRequest.Addtime;
            var type = request.getCaseStatisticsByQueryStringRequest.Type;
            var caseType = request.getCaseStatisticsByQueryStringRequest.CaseType;

            List<SqlParameter> sqlParameters = [];
            GetCaseStatisticsByQueryStringResponse response = new() { Type = type };

            string sql = """
                    SELECT
                        A.UserId,
                        B.UserName,
                        A.AddTime,
                        CASE A.CaseType
                            WHEN 1 THEN '系統派案'
                            WHEN 2 THEN '整批派案'
                            WHEN 3 THEN '調撥案件'
                            WHEN 4 THEN '強制派案'
                            WHEN 5 THEN '待派案_人工'
                            WHEN 6 THEN '調撥_人工'
                            ELSE '錯誤的描述'
                        END AS CaseType,
                        A.ApplyNo
                    FROM [ScoreSharp].[dbo].[Reviewer_CaseStatistics] A
                    LEFT JOIN [ScoreSharp].[dbo].[OrgSetUp_User] B ON A.UserId = B.UserId
                    WHERE 1 = 1
                """;

            if (!string.IsNullOrEmpty(userId))
            {
                sql += " AND A.UserId = @UserId ";
                sqlParameters.Add(new SqlParameter("@UserId", userId));
            }

            if (addTime.HasValue)
            {
                sql += " AND @StartTime <= A.AddTime AND A.AddTime < @EndTime ";
                sqlParameters.Add(new SqlParameter("@StartTime", addTime.Value.Date));
                sqlParameters.Add(new SqlParameter("@EndTime", addTime.Value.Date.AddDays(1)));
            }

            if (caseType.HasValue)
            {
                sql += " AND A.CaseType = @CaseType ";
                sqlParameters.Add(new SqlParameter("@CaseType", caseType.Value));
            }

            var queryDtos = await context.Database.SqlQueryRaw<GetCaseStatisticsDto>(sql, sqlParameters.ToArray()).ToListAsync();

            if (type == "Query")
            {
                response.QueryData = queryDtos;
            }
            else if (type == "Export")
            {
                var excelDtos = queryDtos
                    .GroupBy(k => k.UserId)
                    .ToDictionary(
                        g => $"{g.Key}_{g.First().UserName}_調撥結果",
                        g =>
                            g.Select(c => new CaseStatisticsToExcelDto
                                {
                                    ApplyNo = c.ApplyNo,
                                    AddTime = c.AddTime,
                                    CaseType = c.CaseType,
                                    UserId = c.UserId,
                                    UserName = c.UserName,
                                })
                                .ToList()
                    );
                MemoryStream stream = await miniExcelHelper.匯出多個工作表ExcelToStream(excelDtos);
                response.ExportData = new ExportGetCaseStatisticsToExcelDto
                {
                    FileContent = stream.ToArray(),
                    FileName = $"查詢派案統計_{DateTime.Now:yyyyMMddHHmmss}xlsx",
                };
            }

            return ApiResponseHelper.Success(response);
        }
    }
}
