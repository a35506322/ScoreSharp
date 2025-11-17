using ScoreSharp.API.Common.Helpers.LDAP;

namespace ScoreSharp.API.Infrastructures.Options;

public static class OptionsConfig
{
    public static void AddOptionsConfig(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddOptions<LDAPOptions>().Bind(configuration.GetSection("LDAPOptionConfig")).ValidateDataAnnotations().ValidateOnStart();
    }
}
