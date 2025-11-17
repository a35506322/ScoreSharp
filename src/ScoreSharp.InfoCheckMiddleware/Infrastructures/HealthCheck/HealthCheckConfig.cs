namespace ScoreSharp.InfoCheckMiddleware.Infrastructures.HealthCheck;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var securityHelper = services.BuildServiceProvider().GetRequiredService<IUITCSecurityHelper>();
        var scoreSharpConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ScoreSharp")!);

        services
            .AddHealthChecks()
            .AddSqlServer(scoreSharpConnectionString, name: "ScoreSharpDB", tags: new[] { "database" })
            .AddCheck<SeqHealthyCheck>("Seq", tags: new[] { "seq" });

        return services;
    }
}
