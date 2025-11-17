using Microsoft.Data.SqlClient;

namespace ScoreSharp.Batch.Infrastructures.Data;

public interface IScoreSharpDapperContext
{
    public SqlConnection CreateScoreSharpConnection();

    public SqlConnection CreateECardFileConnection();
}
