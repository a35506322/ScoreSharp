namespace ScoreSharp.Batch.Jobs.EcardA02CheckNewCase.Models;

public class CheckSameWebMobile
{
    public List<Reviewer_CheckTrace> Reviewer_CheckTraces { get; set; } = [];
    public string 是否命中 => Reviewer_CheckTraces.Any() ? "Y" : "N";
}
