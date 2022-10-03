namespace Fintech.ExternalService.CloudService.Caching.Attributes;

[AttributeUsage(AttributeTargets.All)]
public class RedisIsSearchable : Attribute
{
    private readonly bool IsSearchable;
    public RedisIsSearchable(bool IsSearchable)
    {
        this.IsSearchable = IsSearchable;
    }
}
