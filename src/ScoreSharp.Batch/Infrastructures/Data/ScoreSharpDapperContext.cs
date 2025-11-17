using Microsoft.Data.SqlClient;

namespace ScoreSharp.Batch.Infrastructures.Data;

public class ScoreSharpDapperContext : IScoreSharpDapperContext
{
    private readonly IConfiguration _configuration;
    private readonly IUITCSecurityHelper _uitcSecurityHelper;

    public ScoreSharpDapperContext(IConfiguration configuration, IUITCSecurityHelper uitcSecurityHelper)
    {
        _configuration = configuration;
        _uitcSecurityHelper = uitcSecurityHelper;
    }

    public SqlConnection CreateScoreSharpConnection() =>
        new SqlConnection(_uitcSecurityHelper.DecryptConn(_configuration.GetConnectionString("ScoreSharp")!));

    public SqlConnection CreateECardFileConnection() =>
        new SqlConnection(_uitcSecurityHelper.DecryptConn(_configuration.GetConnectionString("ECardFile")!));
}
