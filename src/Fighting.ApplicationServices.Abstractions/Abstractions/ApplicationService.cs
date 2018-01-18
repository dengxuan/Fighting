using Fighting.Caching.Abstractions;
using Orleans;
using System;

namespace Fighting.ApplicationServices.Abstractions
{
    public abstract class ApplicationService : Grain, IApplicationService
    {
        protected ICacheManager CacheManager { get; }

        public ApplicationService(ICacheManager cacheManager)
        {
            CacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }
    }
}
