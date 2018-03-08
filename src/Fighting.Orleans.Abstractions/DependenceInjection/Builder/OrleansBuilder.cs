using Fighting.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Fighting.Orleans.DependencyInjection.Builder
{
    public class OrleansBuilder
    {
        public IServiceCollection Services { get; }

        internal OrleansBuilder(IServiceCollection services) => Services = services;

        internal void Build()
        {

            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<OrleansOptions>, OrleansOptionsSetup>());
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<OrleansOptions>>().Value);
        }
    }
}
