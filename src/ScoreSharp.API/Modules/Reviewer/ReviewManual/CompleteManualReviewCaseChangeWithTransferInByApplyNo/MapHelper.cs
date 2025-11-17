using ScoreSharp.Common.Adapters.MW3.Models;

namespace ScoreSharp.API.Modules.Reviewer.ReviewManual.CompleteManualReviewCaseChangeWithTransferInByApplyNo;

public class MapHelper
{
    public static Reviewer_ApplyCreditCardInfoProcess MapKYCProcess(
        string applyNo,
        string process,
        DateTime startTime,
        DateTime endTime,
        string isSuccess,
        string kycCode,
        string rc2,
        UserType userType,
        string id,
        string keyMsg,
        string userId,
        string suggestCode
    )
    {
        var isSuccessStr = isSuccess == "Y" ? "成功" : "失敗";
        var userTypeStr = userType == UserType.正卡人 ? "正卡人" : "附卡人";
        var notes = $"建議核准KYC結果: {isSuccessStr}; KYC 代碼: {kycCode}; KYC Message: {keyMsg}; 建議代碼: {suggestCode} ({userTypeStr}_{id})";
        return new()
        {
            ApplyNo = applyNo,
            Process = process,
            StartTime = startTime,
            EndTime = endTime,
            Notes = notes,
            ProcessUserId = userId,
        };
    }

    public static Reviewer3rd_KYCQueryLog MapKYCQueryLog(
        string applyNo,
        string cardStatus,
        string id,
        SuggestKycMW3Info request,
        BaseMW3Response<SuggestKycResponse> response,
        string currentHandler
    )
    {
        var kycInfo = response.Data.Info;
        var kycResult = kycInfo.Result.Data;
        return new()
        {
            ApplyNo = applyNo,
            CardStatus = cardStatus,
            ID = id,
            Request = JsonHelper.序列化物件(request),
            KYCCode = kycResult.KycCode,
            KYCRank = string.Empty,
            KYCMsg = kycResult.ErrMsg,
            Response = JsonHelper.序列化物件(response),
            AddTime = DateTime.Now,
            KYCLastSendStatus = response.IsSuccess
                ? (
                    kycInfo.Rc2 == "M000" && kycResult.KycCode == MW3RtnCodeConst.建議核准KYC_成功
                        ? KYCLastSendStatus.不需發送
                        : KYCLastSendStatus.等待
                )
                : KYCLastSendStatus.等待,
            CurrentHandler = currentHandler,
            APIName = "KYC00SECREDIT",
            Source = "CompleteManualReviewCaseChangeWithTransferInByApplyNo",
        };
    }
}
