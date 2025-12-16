using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server;

/// <summary>
/// Implements an Internet Control Message Protocol (ICMP) health check.
/// </summary>
public class PingHealthCheck(string host, int healthyRoundtripTime) : IHealthCheck
{
    private readonly string _host = host;
    private readonly int _healthyRoundtripTime = healthyRoundtripTime;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
		try
		{
			using var ping = new Ping();
			var reply = await ping.SendPingAsync(_host);

            switch (reply.Status)
            {
                case IPStatus.Success:
                    string message = $"Ping to {_host} took {reply.RoundtripTime} ms.";
                    return (reply.RoundtripTime > _healthyRoundtripTime)
                        ? HealthCheckResult.Degraded(message) : HealthCheckResult.Healthy(message);
                default:
                    return HealthCheckResult.Unhealthy($"Ping to {_host} failed: {reply.Status}");
            }
        }
		catch (Exception ex)
		{
			return HealthCheckResult.Unhealthy($"Ping to {_host} failed: {ex.Message}");
		}
    }
}
