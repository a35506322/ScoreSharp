namespace ScoreSharp.Batch.Infrastructures.Email;

public static class EmailConfig
{
    public static void AddEmail(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddFluentEmail(configuration.GetValue<string>("Email:Sender:Email"))
            .AddSmtpSender(configuration.GetValue<string>("Email:Smtp:Host"), configuration.GetValue<int>("Email:Smtp:Port"));
    }
}
