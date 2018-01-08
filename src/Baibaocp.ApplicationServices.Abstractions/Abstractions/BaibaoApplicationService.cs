using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;

namespace Baibaocp.ApplicationServices
{
    public abstract class BaibaoApplicationService : ApplicationService
    {
        public BaibaoApplicationService(ICacheManager cacheManager) : base(cacheManager)
        {
        }
    }
}
