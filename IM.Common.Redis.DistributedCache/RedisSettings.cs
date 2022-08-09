namespace IM.Common.Redis.DistributedCache;

public class RedisSettings
{
    public int? DatabaseId { get; set; }
    public string Host { get; set; }
    public int Port { get; set; } = 6379;
    public string? Password { get; set; }
    public string? ClientName { get; set; }
}
