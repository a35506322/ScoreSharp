using ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.GetWebRetryCaseByQueryString;

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase
{
    public partial class WebRetryCaseController
    {
        /// <summary>
        /// 查詢網路件重試 By QueryString
        /// </summary>
        /// <remarks>
        ///
        /// Sample QueryString:
        ///
        ///     ?applyDate=2025/02/12
        ///
        /// </remarks>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetWebRetryCaseByQueryStringResponse>>))]
        [EndpointSpecificExample(typeof(查詢網路件重試_2000_ResEx), ExampleType = ExampleType.Response, ResponseStatusCode = StatusCodes.Status200OK)]
        [OpenApiOperation("GetWebRetryCaseByQueryString")]
        public async Task<IResult> GetWebRetryCaseByQueryString([FromQuery] GetWebRetryCaseByQueryStringRequest request)
        {
            var result = await _mediator.Send(new Query(request));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.SysPersonnel.WebRetryCase.GetWebRetryCaseByQueryString
{
    public record Query(GetWebRetryCaseByQueryStringRequest getWebRetryCaseByQueryStringRequest)
        : IRequest<ResultResponse<List<GetWebRetryCaseByQueryStringResponse>>>;

    public class Handler(IScoreSharpDapperContext dappercontext, IMapper mapper)
        : IRequestHandler<Query, ResultResponse<List<GetWebRetryCaseByQueryStringResponse>>>
    {
        public async Task<ResultResponse<List<GetWebRetryCaseByQueryStringResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var req = request.getWebRetryCaseByQueryStringRequest;

            // 資料庫名稱由setting中取得
            string sql =
                @"SELECT  A.[SeqNo]
				          ,A.[ApplyNo]
				          ,A.[Request]
				          ,A.[ReturnCode]
				          ,A.[CaseErrorLog]
				          ,A.[LastSendUserId]
				          ,A.[LastSendTtime]
				          ,A.[IsSend]
                          ,A.[AddTime]
                  FROM [dbo].[ReviewerPedding_WebRetryCase] A
                  WHERE 1=1 ";

            if (!string.IsNullOrEmpty(req.ApplyDate))
            {
                sql += "AND CONVERT(VARCHAR(10), A.AddTime, 111) = @queryDate;";
            }

            var response = await dappercontext
                .CreateScoreSharpConnection()
                .QueryAsync<GetWebRetryCaseByQueryStringResponse>(sql, new { queryDate = req.ApplyDate });

            var responseList = response.ToList();

            return ApiResponseHelper.Success(responseList);
        }
    }
}
