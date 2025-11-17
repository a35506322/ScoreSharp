using Microsoft.Extensions.Caching.Memory;

namespace ScoreSharp.API.Infrastructures.Security;

public class PermissionCache
{
    public readonly string 角色與Action關聯 = "ROLE_AUTH";
    public readonly string ActionAll = "ActionAll";

    public MemoryCache Cache { get; } = new MemoryCache(new MemoryCacheOptions { SizeLimit = 2 });
}
