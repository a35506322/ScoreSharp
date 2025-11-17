using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ScoreSharp.Batch.Infrastructures.HealthCheck;

public class MW3HealthyCheck : IHealthCheck
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public MW3HealthyCheck(IHttpClientFactory httpClientFactory, IConfiguration configuration)
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
                var url = _configuration.GetValue<string>("MW3AdapterConfiguration:BaseUrl");
                var apiVersion = _configuration.GetValue<string>("MW3AdapterConfiguration:APIVersion");

                // 設定較短的超時時間，避免健康檢查阻塞太久
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                httpClient.DefaultRequestHeaders.Add("api-version", apiVersion);
                httpClient.BaseAddress = new Uri(url);

                var response = await httpClient.GetAsync($"/MW3/api/PostMan/Get", cancellationToken);
                if (response.IsSuccessStatusCode)
                {
                    return HealthCheckResult.Healthy($"MW3 is healthy.");
                }

                return HealthCheckResult.Unhealthy($"MW3 returned status code: {response.StatusCode}");
            }
        }
        catch (HttpRequestException ex)
        {
            return HealthCheckResult.Unhealthy($"MW3 connection failed: {ex.Message}", ex);
        }
        catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
        {
            return HealthCheckResult.Unhealthy("MW3 health check timed out", ex);
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"MW3 health check failed: {ex.Message}", ex);
        }
    }
}
