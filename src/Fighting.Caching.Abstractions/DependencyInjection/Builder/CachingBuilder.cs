using Fighting.Caching;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Fighting.DependencyInjection.Builder
{
    public class CachingBuilder
    {
        public IServiceCollection Services { get; }

        internal CachingBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<CachingOptions>, CachingOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<CachingOptions>>().Value);
        }
    }
}
