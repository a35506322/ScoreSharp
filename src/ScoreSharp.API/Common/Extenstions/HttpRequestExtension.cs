using Microsoft.AspNetCore.Http;

namespace ScoreSharp.API.Common.Extenstions;

public static class HttpRequestExtension
{
    private const string ForwardedProtoHeader = "X-Forwarded-Proto";
    private const string ForwardedPrefixHeader = "X-Forwarded-Prefix";
    private const string ForwardedHostHeader = "X-Forwarded-Host";

    public static string GetServerUrl(this HttpRequest request)
    {
        var host = TryGetFirstHeader(request.Headers, ForwardedHostHeader);
        var baseUrl = new Uri($"{request.GetHttpScheme()}://{host ?? request.Host.ToString()}").ToString().TrimEnd('/');

        return $"{baseUrl}{request.GetBasePath()}".TrimEnd('/');
    }

    public static string GetBasePath(this HttpRequest request)
    {
        var prefix = TryGetFirstHeader(request.Headers, ForwardedPrefixHeader);

        if (prefix != null)
        {
            return "/" + prefix.Trim('/');
        }

        var host = TryGetFirstHeader(request.Headers, ForwardedHostHeader);
        string basePath;

        if (host != null)
        {
            var proto = TryGetFirstHeader(request.Headers, ForwardedProtoHeader) ?? "http";
            basePath = new Uri($"{proto}://{host}").AbsolutePath.TrimEnd('/');
        }
        else
        {
            basePath = "";
        }

        if (request.PathBase.HasValue)
        {
            basePath += "/" + request.PathBase.Value;
        }

        return ("/" + basePath.Trim('/')).TrimEnd('/');
    }

    private static string GetHttpScheme(this HttpRequest request)
    {
        return request.Headers.TryGetFirstHeader("X-Forwarded-Proto") ?? request.Scheme;
    }

    private static string TryGetFirstHeader(this IHeaderDictionary headers, string name)
    {
        name = headers.Keys.FirstOrDefault(n => string.Equals(n, name, StringComparison.OrdinalIgnoreCase));

        if (name == null)
        {
            return null;
        }

        return headers[name].First().Split(',').Select(s => s.Trim()).First();
    }
}
