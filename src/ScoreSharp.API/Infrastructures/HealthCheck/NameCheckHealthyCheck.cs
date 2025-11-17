using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ScoreSharp.API.Infrastructures.HealthCheck;

public class NameCheckHealthyCheck : IHealthCheck
{
    private IMW3APAPIAdapter _mW3APAPIAdapter;

    public NameCheckHealthyCheck(IMW3APAPIAdapter mW3APAPIAdapter)
    {
        _mW3APAPIAdapter = mW3APAPIAdapter;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        try
        {
            string ulid = Ulid.NewUlid().ToString();
            var response = await _mW3APAPIAdapter.QueryNameCheck("周杰倫", UserIdConst.ScoreSharpBatch, ulid);

            if (response.IsSuccess)
            {
                return HealthCheckResult.Healthy($"NameCheck is healthy.");
            }

            return HealthCheckResult.Unhealthy($"NameCheck returned status code: {response.ErrorMessage}");
        }
        catch (Exception ex)
        {
            return HealthCheckResult.Unhealthy($"NameCheck health check failed: {ex.Message}", ex);
        }
    }
}
