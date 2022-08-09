using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace IM.Common.Redis.DistributedCache;

public static class DependencyInjections
{
    private const string DefaultSection = "redis";

    public static IServiceCollection AddDistributedCacheRedis(
        this IServiceCollection services,
        IConfiguration configuration,
        string sectionName = DefaultSection)
    {
        var settings = new RedisSettings();
        configuration.GetSection(sectionName).Bind(settings);

        var endpoint = IPAddress.TryParse((string?)settings.Host, out var ipAddress)
            ? (EndPoint)new IPEndPoint(ipAddress, settings.Port)
            : new DnsEndPoint(settings.Host, settings.Port);

        var cfg = new ConfigurationOptions
        {
            AllowAdmin = true,
            AbortOnConnectFail = false,
            ConnectRetry = 5,
            ReconnectRetryPolicy = new ExponentialRetry(1000, 500),
            Password = settings.Password,
            EndPoints = { endpoint },
            ClientName = settings.ClientName,
        };

        return services
            .AddStackExchangeRedisCache(options => options.ConfigurationOptions = cfg);
    }
}
