namespace ScoreSharp.Common.Helpers.Address;

public class MatchScopeDto
{
    public string[] Scope { get; set; }
    public int Number { get; set; }
    public int Lane { get; set; }
    public int SubNumber { get; set; }

    public void Deconstruct(out string[] scope, out int number, out int lane, out int subNumber)
    {
        scope = Scope;
        number = Number;
        lane = Lane;
        subNumber = SubNumber;
    }
}
