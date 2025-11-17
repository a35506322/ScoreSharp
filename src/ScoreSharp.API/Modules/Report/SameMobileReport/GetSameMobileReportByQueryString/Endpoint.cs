using System.Data;
using ScoreSharp.API.Modules.Report.SameMobileReport.GetSameMobileReportByQueryString;

namespace ScoreSharp.API.Modules.Report.SameMobileReport
{
    public partial class SameMobileReportController
    {
        ///<summary>
        /// 查詢線上辦卡手機號碼比對相同報表 ByQueryString
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString :
        ///
        ///     ?StartDate=2025/03/06&amp;EndDate=2025/03/07&amp;ComparisonResult=ALL
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetSameMobileReportByQueryStringResponse>>))]
        [EndpointSpecificExample(
            typeof(查詢線上辦卡手機號碼比對相同報表_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetSameMobileReportByQueryString")]
        public async Task<IResult> GetSameMobileReportByQueryString([FromQuery] GetSameMobileReportByQueryStringRequest request) =>
            Results.Ok(await _mediator.Send(new Query(request)));
    }
}

namespace ScoreSharp.API.Modules.Report.SameMobileReport.GetSameMobileReportByQueryString
{
    public record Query(GetSameMobileReportByQueryStringRequest getSameMobileReportByQueryStringRequest)
        : IRequest<ResultResponse<List<GetSameMobileReportByQueryStringResponse>>>;

    public class Handler(IScoreSharpDapperContext scoreSharpDapperContext, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetSameMobileReportByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetSameMobileReportByQueryStringResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        ) => ApiResponseHelper.Success(await GetSameMobileReportByQueryString(request.getSameMobileReportByQueryStringRequest));

        private async Task<List<GetSameMobileReportByQueryStringResponse>> GetSameMobileReportByQueryString(
            GetSameMobileReportByQueryStringRequest request
        )
        {
            List<GetSameMobileReportByQueryStringResponse> responses = new();

            using (var conn = scoreSharpDapperContext.CreateScoreSharpConnection())
            {
                SqlMapper.GridReader results = await conn.QueryMultipleAsync(
                    sql: "Usp_GetSameMobileReport",
                    param: new
                    {
                        startDate = request.StartDate,
                        endDate = request.EndDate,
                        comparisonResult = request.ComparisonResult,
                    },
                    commandType: CommandType.StoredProcedure
                );

                responses = results.Read<GetSameMobileReportByQueryStringResponse>().ToList();
            }

            return responses;
        }
    }
}
