namespace ScoreSharp.Batch.Infrastructures.HealthCheck;

public static class HealthCheckConfig
{
    public static IServiceCollection AddHealthCheck(this IServiceCollection services, IConfiguration configuration)
    {
        var securityHelper = services.BuildServiceProvider().GetRequiredService<IUITCSecurityHelper>();
        var scoreSharpConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ScoreSharp")!);
        var eCardFileConnectionString = securityHelper.DecryptConn(configuration.GetConnectionString("ECardFile")!);
        var smtpHost = configuration.GetValue<string>("Email:Smtp:Host");
        var smtpPort = configuration.GetValue<int>("Email:Smtp:Port");

        services
            .AddHealthChecks()
            .AddSqlServer(scoreSharpConnectionString, name: "ScoreSharpDB", tags: new[] { "database" })
            .AddSqlServer(eCardFileConnectionString, name: "ECardFileDB", tags: new[] { "database" })
            .AddSmtpHealthCheck(
                setup =>
                {
                    setup.Host = smtpHost;
                    setup.Port = smtpPort;
                },
                name: "email",
                tags: new[] { "email" }
            )
            .AddCheck<MW3HealthyCheck>("MW3HealthyCheck", tags: new[] { "mw3" })
            .AddCheck<NameCheckHealthyCheck>("NameCheckHealthyCheck", tags: new[] { "mw3" })
            .AddHangfire(
                setup =>
                {
                    setup.MaximumJobsFailed = 1;
                    setup.MinimumAvailableServers = 1;
                },
                name: "hangfire",
                tags: new[] { "hangfire" }
            );
        return services;
    }
}
