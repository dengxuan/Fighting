namespace Fighting.Caching.Abstractions
{
    public interface ICacheManager
    {
        ICache GetCache(string name);
    }
}
