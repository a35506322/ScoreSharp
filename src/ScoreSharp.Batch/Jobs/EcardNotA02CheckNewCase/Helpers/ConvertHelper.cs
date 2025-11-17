using ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Helpers;

public static class ConvertHelper
{
    public static CardStatus ConvertCardStatus(CardStatus status, bool jobIsSuccess) =>
        (status, jobIsSuccess) switch
        {
            (CardStatus.網路件_非卡友_待檢核, true) => CardStatus.網路件_待月收入預審,
            (CardStatus.網路件_非卡友_待檢核, false) => CardStatus.網路件_待月收入預審_檢核異常,
            (CardStatus.網路件_待月收入預審_檢核異常, true) => CardStatus.網路件_待月收入預審,
            (CardStatus.網路件_待月收入預審_檢核異常, false) => CardStatus.網路件_待月收入預審_檢核異常,
            _ => throw new Exception($"CardStatus is not valid, status: {status}, jobIsSuccess: {jobIsSuccess}"),
        };
}
