using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ScoreSharp.Middleware.Infrastructures.HealthCheck;

public class SeqHealthyCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public SeqHealthyCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            using (var httpClient = _httpClientFactory.CreateClient())
            {
                var url = _configuration.GetValue<string>("SerilLogConfig:SeqUrl");
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                httpClient.BaseAddress = new Uri(url);

                var response = await httpClient.GetAsync($"/health");
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy($"Seq is healthy.");
                }

                return HealthCheckResult.Unhealthy("Seq is unhealthy, status code: " + response.StatusCode);
            }
        }
        catch (HttpRequestException ex)
        {
            return HealthCheckResult.Unhealthy($"Seq connection failed: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return HealthCheckResult.Unhealthy("Seq health check timed out", ex);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"Seq health check failed: {ex.Message}", ex);
        }
    }
}
