namespace ScoreSharp.Middleware.Infrastructures.Data;

public interface IScoreSharpDapperContext
{
    public SqlConnection CreateScoreSharpConnection();
    public SqlConnection CreateECardFileConnection();
}
