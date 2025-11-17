using Microsoft.Extensions.Options;

namespace ScoreSharp.API.Infrastructures.Security;

public class PermissionAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

    public PermissionAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

    public Task<AuthorizationPolicy> GetFallbackPolicyAsync() => FallbackPolicyProvider.GetFallbackPolicyAsync();

    /// <summary>
    /// 繼承實作，動態註冊政策
    /// </summary>
    /// <param name="policyName"></param>
    /// <returns></returns>
    public async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("Policy", StringComparison.OrdinalIgnoreCase))
        {
            var policy = new AuthorizationPolicyBuilder();
            policy.AddRequirements(new PermissionAuthorizationRequirement() { PolicyName = policyName });
            return await Task.FromResult(policy.Build());
        }

        return await FallbackPolicyProvider.GetPolicyAsync(policyName);
    }
}
