using System.Net;

namespace ScoreSharp.LDAP;

public class IPWhitelistMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<IPWhitelistMiddleware> _logger;
    private readonly IPWhitelistOptions _options;

    public IPWhitelistMiddleware(RequestDelegate next, ILogger<IPWhitelistMiddleware> logger, IOptions<IPWhitelistOptions> options)
    {
        _next = next;
        _logger = logger;
        _options = options.Value;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var remoteIp = context.Connection.RemoteIpAddress?.MapToIPv4();

        Console.WriteLine($"remoteIp: {remoteIp}");

        if (!IsIpAllowed(remoteIp))
        {
            _logger.LogWarning($"禁止來自 {remoteIp} 的請求訪問");
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }

        await _next(context);
    }

    private bool IsIpAllowed(IPAddress? remoteIp)
    {
        if (remoteIp == null)
            return false;

        if (_options.AllowedIPs == null || !_options.AllowedIPs.Any())
            return true;

        return _options.AllowedIPs.Contains(remoteIp.ToString());
    }
}
