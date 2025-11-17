namespace ScoreSharp.API.Infrastructures.Data;

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

    public SqlConnection CreateScoreSharpFileConnection() =>
        new SqlConnection(_uitcSecurityHelper.DecryptConn(_configuration.GetConnectionString("ScoreSharpFile")!));

    public SqlConnection CreateScoreSharpFileHisConnection(string dbName) =>
        new SqlConnection(_uitcSecurityHelper.DecryptConn(_configuration.GetConnectionString("ScoreSharpFileHis")!) + $"Database={dbName};");
}
