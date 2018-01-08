using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Caching.Abstractions
{
    public abstract class CacheManager : ICacheManager
    {

        private readonly ConcurrentDictionary<string, ICache> _caches = new ConcurrentDictionary<string, ICache>();

        protected IServiceProvider IocResolver { get; }

        public CacheManager(IServiceProvider sp)
        {
            IocResolver = sp;
        }

        public ICache GetCache(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            return _caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);

                //var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);

                //foreach (var configurator in configurators)
                //{
                //    configurator.InitAction?.Invoke(cache);
                //}

                return cache;
            });
        }

        /// <summary>
        /// Used to create actual cache implementation.
        /// </summary>
        /// <param name="name">Name of the cache</param>
        /// <returns>Cache object</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}
