using Fighting.Caching;
using Microsoft.Extensions.Options;

namespace Fighting.DependencyInjection.Builder
{
    internal class CachingOptionsSetup : ConfigureOptions<CachingOptions>
    {
        public CachingOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(CachingOptions options)
        {
        }
    }
}
