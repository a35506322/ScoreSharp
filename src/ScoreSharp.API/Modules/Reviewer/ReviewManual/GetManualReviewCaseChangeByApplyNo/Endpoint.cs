using ScoreSharp.API.Modules.Reviewer.ReviewManual.GetManualReviewCaseChangeByApplyNo;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual
{
    public partial class ReviewManualController
    {
        ///<summary>
        /// 取得人工徵審案件異動 By申請書編號
        /// </summary>
        /// <remarks>
        /// Sample Router:
        ///
        ///     /ReviewManual/GetManualReviewCaseChangeByApplyNo/20240601A0001
        ///
        /// </remarks>
        ///<returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ResultResponse<List<GetManualReviewCaseChangeByApplyNoResponse>>))]
        [EndpointSpecificExample(
            typeof(取得人工徵審案件異動_2000_ResEx),
            ExampleType = ExampleType.Response,
            ResponseStatusCode = StatusCodes.Status200OK
        )]
        [OpenApiOperation("GetManualReviewCaseChangeByApplyNo")]
        [HttpGet("{applyNo}")]
        public async Task<IResult> GetManualReviewCaseChangeByApplyNo([FromRoute] string applyNo)
        {
            var result = await _mediator.Send(new Query(applyNo));
            return Results.Ok(result);
        }
    }
}

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.GetManualReviewCaseChangeByApplyNo
{
    public record Query(string applyNo) : IRequest<ResultResponse<List<GetManualReviewCaseChangeByApplyNoResponse>>>;

    public class Handler(ScoreSharpContext context, IScoreSharpDapperContext dapperContext)
        : IRequestHandler<Query, ResultResponse<List<GetManualReviewCaseChangeByApplyNoResponse>>>
    {
        public async Task<ResultResponse<List<GetManualReviewCaseChangeByApplyNoResponse>>> Handle(Query request, CancellationToken cancellationToken)
        {
            var applyCardType = await context.SetUp_Card.AsNoTracking().ToDictionaryAsync(x => x.CardCode, x => x.CardName);

            var supplementReasonDict = await context
                .SetUp_SupplementReason.AsNoTracking()
                .ToDictionaryAsync(x => x.SupplementReasonCode, x => x.SupplementReasonName);

            var rejectionReasonDict = await context
                .SetUp_RejectionReason.AsNoTracking()
                .ToDictionaryAsync(x => x.RejectionReasonCode, x => x.RejectionReasonName);

            var caseChange = await GetManualReviewCaseChangeByApplyNo(request.applyNo);

            var caseChangeResponse = caseChange
                .Select(x => new GetManualReviewCaseChangeByApplyNoResponse()
                {
                    SeqNo = x.SeqNo,
                    ApplyNo = x.ApplyNo,
                    CaseChangeAction = MapToManualReviewAction(x.CaseChangeAction),
                    CardStatus = x.CardStatus,
                    ApplyCardType = x.ApplyCardType,
                    ApplyCardTypeName = applyCardType[x.ApplyCardType],
                    UserType = x.UserType,
                    CreditCheckCode = x.CreditCheckCode,
                    CardPromotionCode = x.CardPromotionCode,
                    HandleNote = x.HandleNote,
                    SupplementReasonCode = MapToSupplementReasonCode(x.SupplementReasonCode, supplementReasonDict),
                    OtherSupplementReason = x.OtherSupplementReason,
                    SupplementNote = x.SupplementNote,
                    SupplementSendCardAddr = x.SupplementSendCardAddr,
                    WithdrawalNote = x.WithdrawalNote,
                    RejectionReasonCode = MapToRejectionReasonCode(x.RejectionReasonCode, rejectionReasonDict),
                    OtherRejectionReason = x.OtherRejectionReason,
                    RejectionNote = x.RejectionNote,
                    RejectionSendCardAddr = x.RejectionSendCardAddr,
                    IsPrintSMSAndPaper = x.IsPrintSMSAndPaper,
                    IsOriginCardholderSameCardLimit = x.IsOriginCardholderSameCardLimit,
                    CardLimit = x.CardLimit,
                    IsForceCard = x.IsForceCard,
                    NuclearCardNote = x.NuclearCardNote,
                    IsOriginalCardholder = x.IsOriginalCardholder,
                })
                .OrderBy(x => x.UserType)
                .ThenBy(x => x.ApplyCardType)
                .ToList();

            return ApiResponseHelper.Success(caseChangeResponse);
        }

        private ManualReviewAction? MapToManualReviewAction(CaseChangeAction? caseChangeAction) =>
            caseChangeAction switch
            {
                CaseChangeAction.權限內_撤件作業 => ManualReviewAction.撤件作業,
                CaseChangeAction.權限內_退件作業 => ManualReviewAction.退件作業,
                CaseChangeAction.權限內_補件作業 => ManualReviewAction.補件作業,
                CaseChangeAction.權限內_核卡作業 => ManualReviewAction.核卡作業,
                CaseChangeAction.權限外_排入核卡 => ManualReviewAction.排入核卡,
                CaseChangeAction.權限外_排入退件 => ManualReviewAction.排入退件,
                CaseChangeAction.權限外_排入補件 => ManualReviewAction.排入補件,
                CaseChangeAction.權限外_排入撤件 => ManualReviewAction.排入撤件,
                _ => null,
            };

        private CodeInfo[] MapToSupplementReasonCode(string? supplementReasonCode, Dictionary<string, string> supplementReasonDict)
        {
            if (String.IsNullOrEmpty(supplementReasonCode))
            {
                return Array.Empty<CodeInfo>();
            }

            return supplementReasonCode.Split(",").Select(x => new CodeInfo() { Code = x, Name = supplementReasonDict[x] }).ToArray();
        }

        private CodeInfo[] MapToRejectionReasonCode(string? rejectionReasonCode, Dictionary<string, string> rejectionReasonDict)
        {
            if (String.IsNullOrEmpty(rejectionReasonCode))
            {
                return Array.Empty<CodeInfo>();
            }

            return rejectionReasonCode.Split(",").Select(x => new CodeInfo() { Code = x, Name = rejectionReasonDict[x] }).ToArray();
        }

        private async Task<List<GetManualReviewCaseChangeByApplyNoDto>> GetManualReviewCaseChangeByApplyNo(string applyNo)
        {
            // TODO : 2025.07.25 將資料庫名稱寫在settings中
            var sql = """
                WITH ApplyInfo AS (
                    SELECT ApplyNo, ID, IsOriginalCardholder, 'Main' AS SourceType
                    FROM [dbo].[Reviewer_ApplyCreditCardInfoMain]
                    WHERE ApplyNo = @ApplyNo
                    UNION ALL
                    SELECT ApplyNo, ID, IsOriginalCardholder, 'Supplementary' AS SourceType
                    FROM [dbo].[Reviewer_ApplyCreditCardInfoSupplementary]
                    WHERE ApplyNo = @ApplyNo
                )
                SELECT H.[SeqNo],
                        H.[ApplyNo],
                        H.[UserType],
                        H.[CardStatus],
                        H.[CreditCheckCode],
                        H.[ApplyCardType],
                        H.[CaseChangeAction],
                        H.[SupplementReasonCode],
                        H.[OtherSupplementReason],
                        H.[SupplementNote],
                        H.[SupplementSendCardAddr],
                        H.[WithdrawalNote],
                        H.[RejectionReasonCode],
                        H.[OtherRejectionReason],
                        H.[RejectionNote],
                        H.[RejectionSendCardAddr],
                        H.[CardPromotionCode],
                        H.[IsPrintSMSAndPaper],
                        H.[CardLimit],
                        H.[OriginCardholderJCICNotes],
                        H.[IsOriginCardholderSameCardLimit],
                        H.[IsForceCard],
                        H.[NuclearCardNote],
                        H.[HandleNote],
                        C.[IsOriginalCardholder]
                FROM [dbo].[Reviewer_ApplyCreditCardInfoHandle] H
                INNER JOIN ApplyInfo C ON H.ApplyNo = C.ApplyNo AND H.ID = C.ID
                WHERE H.ApplyNo = @ApplyNo
                """;

            var parameters = new { ApplyNo = applyNo };
            using var connection = dapperContext.CreateScoreSharpConnection();
            var result = await connection.QueryAsync<GetManualReviewCaseChangeByApplyNoDto>(sql, parameters);
            return result.ToList();
        }
    }
}
