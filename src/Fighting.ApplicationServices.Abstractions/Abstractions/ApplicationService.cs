using Fighting.Caching.Abstractions;
using System;

namespace Fighting.ApplicationServices.Abstractions
{
    public abstract class ApplicationService : IApplicationService
    {
        protected ICacheManager CacheManager { get; }

        public ApplicationService(ICacheManager cacheManager)
        {
            CacheManager = cacheManager ?? throw new ArgumentNullException(nameof(cacheManager));
        }
    }
}
