namespace ScoreSharp.API.Infrastructures.Data;

public interface IScoreSharpDapperContext
{
    public SqlConnection CreateScoreSharpConnection();
    public SqlConnection CreateECardFileConnection();
    public SqlConnection CreateScoreSharpFileConnection();
    public SqlConnection CreateScoreSharpFileHisConnection(string dbName);
}
