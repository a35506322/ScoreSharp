using ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCardStatus;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore
{
    public partial class ReviewerCoreController
    {
        /// <summary>
        /// 取得卡別狀態
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerCore/GetApplyCardStatus/1234567890
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetApplyCardStatusResponse>>))]
        [EndpointSpecificExample(
            typeof(取得卡別狀態_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetApplyCardStatus")]
        public async Task<IResult> GetApplyCardStatus([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerCore.GetApplyCardStatus
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetApplyCardStatusResponse>>>;

    public class Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Query, ResultResponse<List<GetApplyCardStatusResponse>>>
    {
        public async Task<ResultResponse<List<GetApplyCardStatusResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            List<GetApplyCardStatusDto> cardStatusList;
            Dictionary<string, string> cardDictionary;
            Dictionary<string, string> creditCheckCodeDictionary;
            Dictionary<string, string> cardPromotionDictionary;

            string sql =
                @"SELECT  H.SeqNo,
                            H.ApplyCardType,
                            H.CreditCheckCode,
                            H.CardPromotionCode,
                            H.CardStatus,
                            H.CardLimit,
                            H.UserType,
                            COUNT(D.SeqNo) AS SupplementCount
                    FROM [ScoreSharp].[dbo].[Reviewer_ApplyCreditCardInfoHandle] H
                    LEFT JOIN [ScoreSharp].[dbo].[Reviewer_CardRecord] D ON H.SeqNo = D.HandleSeqNo  AND D.CardStatus = 10231
                    WHERE H.ApplyNo = @applyNo
                    GROUP BY H.SeqNo,H.ApplyCardType, H.CreditCheckCode, H.CardPromotionCode, H.CardStatus, H.CardLimit,H.UserType;

                SELECT DISTINCT CardCode, CardName FROM [ScoreSharp].[dbo].[SetUp_Card];

                SELECT DISTINCT CreditCheckCode, CreditCheckCodeName FROM  [ScoreSharp].[dbo].[SetUp_CreditCheckCode];

                SELECT DISTINCT CardPromotionCode, CardPromotionName FROM [ScoreSharp].[dbo].[SetUp_CardPromotion];";

            using (var connection = dapperContext.CreateScoreSharpConnection())
            {
                using (var multiQuery = await connection.QueryMultipleAsync(sql, new { request.applyNo }))
                {
                    cardStatusList = multiQuery.Read<GetApplyCardStatusDto>().ToList();
                    cardDictionary = multiQuery.Read<(string CardCode, string CardName)>().ToDictionary(x => x.CardCode, x => x.CardName);
                    creditCheckCodeDictionary = multiQuery
                        .Read<(string CreditCheckCode, string CreditCheckCodeName)>()
                        .ToDictionary(x => x.CreditCheckCode, x => x.CreditCheckCodeName);
                    cardPromotionDictionary = multiQuery
                        .Read<(string CardPromotionCode, string CardPromotionName)>()
                        .ToDictionary(x => x.CardPromotionCode, x => x.CardPromotionName);
                }
            }

            List<GetApplyCardStatusResponse> response = cardStatusList
                .OrderBy(dto => dto.UserType)
                .Select(dto => new GetApplyCardStatusResponse
                {
                    SeqNo = dto.SeqNo,
                    ApplyCardType = dto.ApplyCardType,
                    ApplyCardTypeName = 取得代碼名稱(dto.ApplyCardType, cardDictionary),
                    CreditCheckCode = dto.CreditCheckCode,
                    CreditCheckCodeName = 取得代碼名稱(dto.CreditCheckCode, creditCheckCodeDictionary),
                    CardPromotion = dto.CardPromotionCode,
                    CardPromotionName = 取得代碼名稱(dto.CardPromotionCode, cardPromotionDictionary),
                    CardStatus = dto.CardStatus,
                    UserType = dto.UserType,
                    SupplementCount = dto.SupplementCount,
                    CardLimit = dto.CardLimit,
                })
                .ToList();

            return ApiResponseHelper.Success(response);
        }

        private string? 取得代碼名稱(string code, Dictionary<string, string> dic)
        {
            if (string.IsNullOrEmpty(code))
                return string.Empty;

            return dic.TryGetValue(code, out var name) ? name : "錯誤代碼";
        }
    }
}
