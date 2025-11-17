using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoQueryLogByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得申請信用卡資料查詢紀錄 By 申請編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetApplyCreditCardInfoQueryLogByApplyNo/20241014B0701
        ///
        /// </remarks>
        ///<returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(
            StatusCodes.Status200OK,
            Type = typeof(ResultResponse<List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>>)
        )]
        [EndpointSpecificExample(
            typeof(取得申請信用卡資料查詢紀錄_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyCreditCardInfoQueryLogByApplyNo")]
        public async Task<IResult> GetApplyCreditCardInfoQueryLogByApplyNo([FromRoute] string applyNo) =>
            Results.Ok(await _mediator.Send(new Query(applyNo)));
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCreditCardInfoQueryLogByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>>>;

    public class Handler(IScoreSharpDapperContext scoreSharpDapperContext)
        : IRequestHandler<Query, ResultResponse<List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>>>
    {
        public async Task<ResultResponse<List<GetApplyCreditCardInfoQueryLogByApplyNoResponse>>> Handle(
            Query request,
            CancellationToken cancellationToken
        )
        {
            var applyNo = request.applyNo;

            using var conn = scoreSharpDapperContext.CreateScoreSharpConnection();
            var sql =
                @"
                SELECT A.*, B.UserName
                FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoQueryLog] A
                JOIN [ScoreSharp].[dbo].[OrgSetUp_User] B ON A.UserId = B.UserId
                WHERE A.ApplyNo = @ApplyNo
                ORDER BY AddTime DESC";

            var result = await conn.QueryAsync<GetApplyCreditCardInfoQueryLogByApplyNoResponse>(sql, new { ApplyNo = applyNo });

            return ApiResponseHelper.Success(result.ToList());
        }
    }
}
