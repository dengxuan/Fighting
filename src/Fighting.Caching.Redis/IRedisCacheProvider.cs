using StackExchange.Redis;

namespace Fighting.Caching.Redis
{
    public interface IRedisCacheProvider
    {
        /// <summary>
        /// Gets the database connection.
        /// </summary>
        IDatabase GetDatabase();
    }
}
