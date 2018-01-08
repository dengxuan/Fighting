using Fighting.Caching.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.Caching.Redis
{
    public class RedisCacheManager : CacheManager
    {
        public RedisCacheManager(IServiceProvider sp) : base(sp)
        {
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return new RedisCache(IocResolver.GetRequiredService<IRedisCacheProvider>(), IocResolver.GetRequiredService<ICachingSerializer>(), name);
        }
    }
}
