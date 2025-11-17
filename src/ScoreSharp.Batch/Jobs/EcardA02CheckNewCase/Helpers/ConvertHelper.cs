using ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Helpers;

public static class ConvertHelper
{
    public static CardStatus ConvertCardStatus(CardStatus status, bool jobIsError, VerifyResultContext verifyResultContext) =>
        (status, jobIsError, verifyResultContext.郵遞區號計算成功) switch
        {
            (CardStatus.網路件_卡友_待檢核, true, true) => CardStatus.KYC入檔作業_完成卡友檢核,
            (CardStatus.網路件_卡友_待檢核, true, false) => CardStatus.網路件_待月收入預審,
            (CardStatus.網路件_卡友_待檢核, false, false) => CardStatus.網路件_卡友_檢核異常,
            (CardStatus.網路件_卡友_待檢核, false, true) => CardStatus.網路件_卡友_檢核異常,
            (CardStatus.網路件_卡友_檢核異常, true, true) => CardStatus.KYC入檔作業_完成卡友檢核,
            (CardStatus.網路件_卡友_檢核異常, true, false) => CardStatus.網路件_待月收入預審,
            (CardStatus.網路件_卡友_檢核異常, false, false) => CardStatus.網路件_卡友_檢核異常,
            (CardStatus.網路件_卡友_檢核異常, false, true) => CardStatus.網路件_卡友_檢核異常,
            _ => throw new Exception(
                $"CardStatus is not valid, status: {status}, jobIsError: {jobIsError}, 郵遞區號計算成功: {verifyResultContext.郵遞區號計算成功}"
            ),
        };
}
