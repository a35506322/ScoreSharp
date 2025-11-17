namespace ScoreSharp.Batch.Jobs.EcardNotA02CheckNewCase.Models;

public class CheckShortTimeID
{
    public List<Reviewer_CheckTrace> Reviewer_CheckTraces { get; set; } = [];
    public string 是否命中 => Reviewer_CheckTraces.Any() ? "Y" : "N";
}
