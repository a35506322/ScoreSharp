namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.GetManualReviewCaseChangeByApplyNo;

[ExampleAnnotation(Name = "[2000]取得人工徵審案件異動", ExampleType = ExampleType.Response)]
public class 取得人工徵審案件異動_2000_ResEx : IExampleProvider<ResultResponse<List<GetManualReviewCaseChangeByApplyNoResponse>>>
{
    public ResultResponse<List<GetManualReviewCaseChangeByApplyNoResponse>> GetExample()
    {
        var data = new List<GetManualReviewCaseChangeByApplyNoResponse>()
        {
            new GetManualReviewCaseChangeByApplyNoResponse()
            {
                SeqNo = "01JQMZ8FR8AGA4PF28BVT7EDBK",
                ApplyNo = "20250331H5860",
                CaseChangeAction = null,
                CardStatus = CardStatus.人工徵信中,
                ApplyCardType = "JS59",
                ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                UserType = UserType.正卡人,
                CreditCheckCode = "",
                CardPromotionCode = null,
                HandleNote = null,
                SupplementReasonCode = Array.Empty<CodeInfo>(),
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                SupplementSendCardAddrName = null,
                WithdrawalNote = null,
                RejectionReasonCode = Array.Empty<CodeInfo>(),
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = null,
                RejectionSendCardAddrName = null,
                IsPrintSMSAndPaper = null,
                IsOriginCardholderSameCardLimit = null,
                CardLimit = null,
                IsForceCard = null,
                NuclearCardNote = null,
            },
            new GetManualReviewCaseChangeByApplyNoResponse()
            {
                SeqNo = "01JQMZ8FR8AGA4PF28BVT8EDBK",
                ApplyNo = "20250331H5860",
                CaseChangeAction = ManualReviewAction.撤件作業,
                CardStatus = CardStatus.人工徵信中,
                ApplyCardType = "JST59",
                ApplyCardTypeName = "國民現金普卡A",
                UserType = UserType.正卡人,
                CreditCheckCode = "",
                CardPromotionCode = null,
                HandleNote = null,
                SupplementReasonCode = Array.Empty<CodeInfo>(),
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                SupplementSendCardAddrName = null,
                WithdrawalNote = null,
                RejectionReasonCode = Array.Empty<CodeInfo>(),
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = null,
                RejectionSendCardAddrName = null,
                IsPrintSMSAndPaper = null,
                IsOriginCardholderSameCardLimit = null,
                CardLimit = null,
                IsForceCard = null,
                NuclearCardNote = null,
                IsOriginalCardholder = "Y",
            },
        };
        return ApiResponseHelper.Success(data);
    }
}
