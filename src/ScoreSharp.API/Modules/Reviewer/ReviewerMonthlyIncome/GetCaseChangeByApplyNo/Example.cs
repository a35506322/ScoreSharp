namespace ScoreSharp.API.Modules.Reviewer.ReviewerMonthlyIncome.GetCaseChangeByApplyNo;

[ExampleAnnotation(Name = "[2000]取得月收入確認案件異動顯示資料_待月收入預審", ExampleType = ExampleType.Response)]
public class 待月收入預審_2000_ResEx : IExampleProvider<ResultResponse<List<GetCaseChangeByApplyNoResponse>>>
{
    public ResultResponse<List<GetCaseChangeByApplyNoResponse>> GetExample()
    {
        var data = new List<GetCaseChangeByApplyNoResponse>()
        {
            new GetCaseChangeByApplyNoResponse()
            {
                ApplyNo = "20250102Z7050",
                CaseChangeAction = null,
                CardStatus = CardStatus.網路件_待月收入預審,
                ApplyCardType = "JS59",
                ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                CardPromotionCode = null,
                SupplementReasonCode = null,
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                WithdrawalNote = null,
                RejectionReasonCode = null,
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = null,
                IsPrintSMSAndPaper = null,
            },
        };

        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[2000]取得月收入確認案件異動顯示資料_補件等待完成本案徵審", ExampleType = ExampleType.Response)]
public class 補件等待完成本案徵審_2000_ResEx : IExampleProvider<ResultResponse<List<GetCaseChangeByApplyNoResponse>>>
{
    public ResultResponse<List<GetCaseChangeByApplyNoResponse>> GetExample()
    {
        var data = new List<GetCaseChangeByApplyNoResponse>()
        {
            new GetCaseChangeByApplyNoResponse()
            {
                ApplyNo = "20250102Z7050",
                CaseChangeAction = null,
                CardStatus = CardStatus.補件_等待完成本案徵審,
                ApplyCardType = "JS59",
                ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                CardPromotionCode = null,
                SupplementReasonCode = null,
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                WithdrawalNote = null,
                RejectionReasonCode = null,
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = MailingAddressType.公司地址,
                IsPrintSMSAndPaper = "Y",
            },
        };

        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[2000]取得月收入確認案件異動顯示資料_退件等待完成本案徵審", ExampleType = ExampleType.Response)]
public class 退件等待完成本案徵審_2000_ResEx : IExampleProvider<ResultResponse<List<GetCaseChangeByApplyNoResponse>>>
{
    public ResultResponse<List<GetCaseChangeByApplyNoResponse>> GetExample()
    {
        var data = new List<GetCaseChangeByApplyNoResponse>()
        {
            new GetCaseChangeByApplyNoResponse()
            {
                ApplyNo = "20250102Z7050",
                CaseChangeAction = null,
                CardStatus = CardStatus.退件_等待完成本案徵審,
                ApplyCardType = "JS59",
                ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                CardPromotionCode = null,
                SupplementReasonCode = null,
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                WithdrawalNote = null,
                RejectionReasonCode = [new CodeInfo() { Code = "01", Name = "查核黑名單命中" }],
                OtherRejectionReason = null,
                RejectionNote = "此為危險人物",
                RejectionSendCardAddr = MailingAddressType.公司地址,
                IsPrintSMSAndPaper = "Y",
            },
        };

        return ApiResponseHelper.Success(data);
    }
}

[ExampleAnnotation(Name = "[2000]取得月收入確認案件異動顯示資料_撤件等待完成本案徵審", ExampleType = ExampleType.Response)]
public class 撤件等待完成本案徵審_2000_ResEx : IExampleProvider<ResultResponse<List<GetCaseChangeByApplyNoResponse>>>
{
    public ResultResponse<List<GetCaseChangeByApplyNoResponse>> GetExample()
    {
        var data = new List<GetCaseChangeByApplyNoResponse>()
        {
            new GetCaseChangeByApplyNoResponse()
            {
                ApplyNo = "20250102Z7050",
                CaseChangeAction = null,
                CardStatus = CardStatus.撤件_等待完成本案徵審,
                ApplyCardType = "JS59",
                ApplyCardTypeName = "聯邦一卡通吉鶴卡",
                CardPromotionCode = null,
                SupplementReasonCode = null,
                OtherSupplementReason = null,
                SupplementNote = null,
                SupplementSendCardAddr = null,
                WithdrawalNote = "撤件",
                RejectionReasonCode = null,
                OtherRejectionReason = null,
                RejectionNote = null,
                RejectionSendCardAddr = null,
                IsPrintSMSAndPaper = "N",
            },
        };

        return ApiResponseHelper.Success(data);
    }
}
