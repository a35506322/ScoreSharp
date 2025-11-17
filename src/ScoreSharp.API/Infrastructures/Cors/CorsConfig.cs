namespace ScoreSharp.API.Infrastructures.Cors;

public static class CorsConfig
{
    public static void ConfigureCors(this IServiceCollection services, IWebHostEnvironment env)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                if (env.IsDevelopment())
                {
                    builder.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
                }
                else if (env.IsEnvironment("Testing"))
                {
                    builder
                        .SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost" || new Uri(origin).Host == "192.168.233.40")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                }
            });
        });
    }
}
