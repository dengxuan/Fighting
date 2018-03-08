using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Fighting.MessageServices.DependencyInjection.Builder
{
    public class MessageServiceBuilder
    {
        public IServiceCollection Services { get; }

        internal MessageServiceBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<MessageServiceOptions>, MessageServiceOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<MessageServiceOptions>>().Value);
        }
    }
}
