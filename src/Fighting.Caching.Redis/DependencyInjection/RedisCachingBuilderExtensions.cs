using Fighting.Caching;
using Fighting.Caching.Abstractions;
using Fighting.Caching.Redis;
using Fighting.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.DependencyInjection
{
    public static class RedisCachingBuilderExtensions
    {
        /// <summary>
        /// Adds both Redis publisher and subscriber
        /// </summary>
        /// <remarks>
        /// These services expect <see cref="IConnectionMultiplexer"/> to be registered
        /// </remarks>
        /// <param name="builder">Messaging builder</param>
        /// <returns></returns>
        public static CachingBuilder UseRedisCache(this CachingBuilder builder, Action<CachingOptions> options)
        {
            builder.Services.Configure(options);
            builder.Services.AddSingleton<IRedisCacheProvider, RedisCacheDatabaseProvider>();
            builder.Services.AddSingleton<ICacheManager, RedisCacheManager>();
            builder.Services.AddSingleton<ICachingSerializer, JsonCachingSerializer>();
            return builder;
        }
    }
}
