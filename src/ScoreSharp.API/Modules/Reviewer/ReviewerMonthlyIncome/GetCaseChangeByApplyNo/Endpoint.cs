using ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetCaseChangeByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome
{
    public partial class ReviewerMonthlyIncomeController
    {
        /// <summary>
        /// 取得月收入確認案件異動
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewerMonthlyIncome/GetCaseChangeByApplyNo/1234567890
        ///
        /// </remarks>
        /// <param name="applyNo">申請書編號</param>
        /// <returns></returns>
        [HttpGet("{applyNo}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<string>))]
        [EndpointSpecificExample(
            typeof(待月收入預審_2000_ResEx),
            typeof(補件等待完成本案徵審_2000_ResEx),
            typeof(退件等待完成本案徵審_2000_ResEx),
            typeof(撤件等待完成本案徵審_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetCaseChangeByApplyNo")]
        public async Task<IResult> GetCaseChangeByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetCaseChangeByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetCaseChangeByApplyNoResponse>>>;

    public class Handler(IScoreSharpDapperContext dapperContext) : IRequestHandler<Query, ResultResponse<List<GetCaseChangeByApplyNoResponse>>>
    {
        public async Task<ResultResponse<List<GetCaseChangeByApplyNoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var (caseChangeDtos, cardDictionary, rejectionReasonList, supplementReasonList) = await GetInfo(request.applyNo);

            var response = caseChangeDtos
                .Select(dto =>
                {
                    string? applyCardTypeName = null;
                    if (!string.IsNullOrEmpty(dto?.ApplyCardType))
                    {
                        var applyCardTypes = dto.ApplyCardType.Split('/');
                        var applyCardTypeNames = applyCardTypes.Select(code => cardDictionary.TryGetValue(code, out var name) ? name : "錯誤代碼");

                        applyCardTypeName = string.Join('/', applyCardTypeNames);
                    }

                    return new GetCaseChangeByApplyNoResponse()
                    {
                        SeqNo = dto.SeqNo,
                        ApplyNo = dto.ApplyNo,
                        CaseChangeAction = dto.CaseChangeAction,
                        CaseChangeActionName = dto.CaseChangeAction.ToString(),
                        CardStatus = dto.CardStatus,
                        ApplyCardTypeName = applyCardTypeName,
                        ApplyCardType = dto?.ApplyCardType,
                        CardPromotionCode = dto?.CardPromotionCode,
                        SupplementReasonCode = 切割字串轉換成資訊陣列(dto.SupplementReasonCode, supplementReasonList),
                        OtherSupplementReason = dto.OtherSupplementReason,
                        SupplementNote = dto.SupplementNote,
                        SupplementSendCardAddr = dto.SupplementSendCardAddr,
                        SupplementSendCardAddrName = dto.SupplementSendCardAddr == null ? null : dto.SupplementSendCardAddr.ToString(),
                        WithdrawalNote = dto.WithdrawalNote,
                        RejectionReasonCode = 切割字串轉換成資訊陣列(dto.RejectionReasonCode, rejectionReasonList),
                        OtherRejectionReason = dto.OtherRejectionReason,
                        RejectionNote = dto.RejectionNote,
                        RejectionSendCardAddr = dto.RejectionSendCardAddr,
                        RejectionSendCardAddrName = dto.RejectionSendCardAddr == null ? null : dto.RejectionSendCardAddr.ToString(),
                        IsPrintSMSAndPaper = dto.IsPrintSMSAndPaper,
                        UserType = dto.UserType,
                    };
                })
                .OrderBy(x => x.UserType)
                .ThenBy(x => x.ApplyCardType)
                .ToList();

            return ApiResponseHelper.Success(response);
        }

        private CodeInfo[]? 切割字串轉換成資訊陣列(string? codes, Dictionary<string, string> dic)
        {
            if (string.IsNullOrEmpty(codes))
                return null;

            string[] codeArray = codes.Split(',', StringSplitOptions.RemoveEmptyEntries);
            CodeInfo[] infos = codeArray
                .Select(x =>
                {
                    dic.TryGetValue(x, out string? name);
                    return new CodeInfo() { Code = x, Name = name ?? "錯誤代碼" };
                })
                .ToArray();

            return infos;
        }

        private async Task<(
            List<GetCaseChangeByApplyNoDto> caseChangeDtos,
            Dictionary<string, string> cardDictionary,
            Dictionary<string, string> rejectionReasonList,
            Dictionary<string, string> supplementReasonList
        )> GetInfo(string applyNo)
        {
            // 將資料庫名稱寫在settings中
            string sql =
                @"
                        SELECT
                        H.[SeqNo]
                        ,H.[ApplyNo]
                        ,H.[CaseChangeAction]
                        ,H.[CardStatus]
                        ,H.[ApplyCardType]
                        ,H.[CardPromotionCode]
                        ,H.[SupplementReasonCode]
                        ,H.[OtherSupplementReason]
                        ,H.[SupplementNote]
                        ,H.[SupplementSendCardAddr]
                        ,H.[WithdrawalNote]
                        ,H.[RejectionReasonCode]
                        ,H.[OtherRejectionReason]
                        ,H.[RejectionNote]
                        ,H.[RejectionSendCardAddr]
                        ,H.[IsPrintSMSAndPaper]
                        ,H.UserType
                    FROM [dbo].[Reviewer_ApplyCreditCardInfoHandle] H
                    WHERE H.ApplyNo = @applyNo;

                    SELECT DISTINCT CardCode,CardName FROM [dbo].[SetUp_Card];

                    SELECT  DISTINCT RejectionReasonCode,RejectionReasonName
                    FROM [dbo].[SetUp_RejectionReason];

                    SELECT  DISTINCT SupplementReasonCode,SupplementReasonName
                    FROM [dbo].[SetUp_SupplementReason];";
            using var connection = dapperContext.CreateScoreSharpConnection();
            using var multiQuery = await connection.QueryMultipleAsync(sql, new { applyNo });
            var caseChangeDtos = multiQuery.Read<GetCaseChangeByApplyNoDto>().ToList();
            var cardDictionary = multiQuery.Read<(string CardCode, string CardName)>().ToDictionary(x => x.CardCode, x => x.CardName);
            var rejectionReasonList = multiQuery.Read<(string Code, string Name)>().ToDictionary(x => x.Code, x => x.Name);
            var supplementReasonList = multiQuery.Read<(string Code, string Name)>().ToDictionary(x => x.Code, x => x.Name);

            return (caseChangeDtos, cardDictionary, rejectionReasonList, supplementReasonList);
        }
    }
}
