namespace AvesdoSystemDesign.Api.Configurations;

public class CacheSettings
{
    // Memory || Redis    
    public string Provider { get; set; } = "Memory";
    public string? RedisConnection { get; set; }
}