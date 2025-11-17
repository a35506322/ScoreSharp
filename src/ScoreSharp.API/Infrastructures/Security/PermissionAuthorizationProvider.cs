namespace ScoreSharp.API.Infrastructures.Security;

/// <summary>
/// 提供政策所需要的相關資料
/// </summary>
public class PermissionAuthorizationProvider : IPermissionAuthorizationProvider
{
    private readonly IServiceProvider _serviceProvider;

    public PermissionAuthorizationProvider(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task<ActionDto?> GetActionByActionId(string actionId)
    {
        var cacheOptions = new FusionCacheEntryOptions { Duration = TimeSpan.FromHours(4), FailSafeMaxDuration = TimeSpan.FromHours(6) };

        using var scope = _serviceProvider.CreateScope();
        var fusionCache = scope.ServiceProvider.GetRequiredService<IFusionCache>();
        var cacheKey = $"{SecurityConstants.PolicyRedisKey.Action}:{actionId}";

        var redisAction = await fusionCache.GetOrSetAsync(
            cacheKey,
            async (cacheEntry) =>
            {
                var context = scope.ServiceProvider.GetRequiredService<ScoreSharpContext>();
                var authAction = await context
                    .Auth_Action.AsNoTracking()
                    .Select(x => new ActionDto
                    {
                        ActionId = x.ActionId,
                        ActionName = x.ActionName,
                        IsActive = x.IsActive,
                        IsCommon = x.IsCommon,
                    })
                    .FirstOrDefaultAsync(x => x.ActionId == actionId);

                return authAction;
            },
            cacheOptions,
            tags: new[] { SecurityConstants.PolicyRedisTag.Action }
        );

        if (redisAction is null)
        {
            await fusionCache.RemoveAsync(cacheKey);
        }

        return redisAction;
    }

    public async Task<IEnumerable<string>> GetRoleWithActoinByRoleIds(string[] roleIds)
    {
        var cacheOptions = new FusionCacheEntryOptions { Duration = TimeSpan.FromHours(4), FailSafeMaxDuration = TimeSpan.FromHours(6) };

        using var scope = _serviceProvider.CreateScope();
        var fusionCache = scope.ServiceProvider.GetRequiredService<IFusionCache>();
        var cacheKey = $"{SecurityConstants.PolicyRedisKey.RoleAction}:{string.Join("_", roleIds)}";

        var redisRoleAction = await fusionCache.GetOrSetAsync(
            cacheKey,
            async (cacheEntry) =>
            {
                var context = scope.ServiceProvider.GetRequiredService<ScoreSharpContext>();
                var roleAction = await context
                    .Auth_Role_Router_Action.AsNoTracking()
                    .Where(x => roleIds.Contains(x.RoleId))
                    .Select(x => x.ActionId)
                    .Distinct()
                    .ToListAsync();

                return roleAction;
            },
            cacheOptions,
            tags: new[] { SecurityConstants.PolicyRedisTag.RoleAction }
        );

        if (redisRoleAction is null || !redisRoleAction.Any())
        {
            await fusionCache.RemoveAsync(cacheKey);
        }

        return redisRoleAction!;
    }
}
