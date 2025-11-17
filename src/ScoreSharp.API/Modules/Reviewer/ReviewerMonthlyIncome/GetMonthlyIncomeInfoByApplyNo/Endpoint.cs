using ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetMonthlyIncomeInfoByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome
{
    public partial class ReviewerMonthlyIncomeController
    {
        /// <summary>
        /// 取得月收入簽核顯示資料
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerMonthlyIncome/GetMonthlyIncomeInfoByApplyNo/1234567890
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(查詢月收入簽核顯示資料_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetMonthlyIncomeInfoByApplyNo")]
        public async Task<IResult> GetMonthlyIncomeInfoByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetMonthlyIncomeInfoByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<GetMonthlyIncomeInfoByApplyNoResponse>>;

    public class Handler(IScoreSharpDapperContext dapperContext) : IRequestHandler<Query, ResultResponse<GetMonthlyIncomeInfoByApplyNoResponse>>
    {
        public async Task<ResultResponse<GetMonthlyIncomeInfoByApplyNoResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (queryResult, cardDictionary) = await GetInfo(request.applyNo);

            var response = queryResult
                .GroupBy(x => x.ApplyNo)
                .Select(x =>
                {
                    var first = x.FirstOrDefault();

                    return new GetMonthlyIncomeInfoByApplyNoResponse
                    {
                        ApplyNo = first.ApplyNo,
                        CreditCheckCode = first.CreditCheckCode,
                        CurrentMonthIncome = first.CurrentMonthIncome,
                        CardOwner = first.CardOwner,
                        CardInfoList = x.Select(y => new CardInfo
                            {
                                UserType = y.UserType,
                                CardStatus = y.CardStatus,
                                ApplyCardType = y.ApplyCardType,
                                ApplyCardTypeName = cardDictionary.TryGetValue(y.ApplyCardType ?? string.Empty, out var name) ? name : null,
                            })
                            .OrderBy(y => y.UserType)
                            .ThenBy(y => y.ApplyCardType)
                            .ToList(),
                    };
                })
                .FirstOrDefault();
            return ApiResponseHelper.Success(response);
        }

        private async Task<(IEnumerable<GetMonthlyIncomeInfoByApplyNoDto> queryResult, Dictionary<string, string> cardDictionary)> GetInfo(
            string applyNo
        )
        {
            // 將資料庫名稱寫在settings中
            string sql =
                @"SELECT     H.[ApplyNo]
                            ,H.[CardStatus]
                            ,H.[CreditCheckCode]
                            ,M.[CardOwner]
                            ,H.[ApplyCardType]
                            ,M.CurrentMonthIncome
                            ,H.UserType
                    FROM [dbo].[Reviewer_ApplyCreditCardInfoMain] M
                    JOIN [dbo].[Reviewer_ApplyCreditCardInfoHandle] H On H.ApplyNo = M.ApplyNo
                    WHERE H.ApplyNo = @applyNo;

                SELECT CardCode,CardName FROM [dbo].[SetUp_Card];";

            using var connection = dapperContext.CreateScoreSharpConnection();
            using var multiQuery = await connection.QueryMultipleAsync(sql, new { applyNo });
            var queryResult = multiQuery.Read<GetMonthlyIncomeInfoByApplyNoDto>();
            var cardDictionary = multiQuery.Read<(string CardCode, string CardName)>().ToDictionary(x => x.CardCode, x => x.CardName);

            return (queryResult, cardDictionary);
        }
    }
}
