namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class CheckInternalMobileSameResult
{
    public List<Reviewer_BankInternalSameLog> BankInternalSameLogs { get; set; } = [];
    public string 是否命中 => BankInternalSameLogs.Any() ? "Y" : "N";
}
