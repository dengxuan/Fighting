using Fighting.ApplicationServices.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Orleans;
using System;

namespace Fighting.ApplicationServices.DependencyInjection.Builder
{
    public class ApplicationServiceBuilder
    {
        public IServiceCollection Services { get; }

        internal ApplicationServiceBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<ApplicationServiceOptions>, ApplicationServiceOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<ApplicationServiceOptions>>().Value);
            Services.AddSingleton<IApplicationServiceCluster, ApplicationServiceCluster>();
        }
    }
}
