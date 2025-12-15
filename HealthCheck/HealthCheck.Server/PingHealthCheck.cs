using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net.NetworkInformation;

namespace HealthCheck.Server;

/// <summary>
/// Implements an Internet Control Message Protocol (ICMP) health check.
/// </summary>
public class PingHealthCheck : IHealthCheck
{
    // Set host to a non-routable IP address to simulate an "unhealthy" scenario
    private readonly string _host = "10.0.0.0";
    private readonly int _healthyRoundTripTime = 300;

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
		try
		{
			using var ping = new Ping();
			var reply = await ping.SendPingAsync(_host);
            return reply.Status switch
            {
                IPStatus.Success => (reply.RoundtripTime > _healthyRoundTripTime) 
                    ? HealthCheckResult.Degraded() : HealthCheckResult.Healthy(),
                _ => HealthCheckResult.Unhealthy(),
            };
        }
		catch (Exception)
		{
			return HealthCheckResult.Unhealthy();
		}
    }
}
