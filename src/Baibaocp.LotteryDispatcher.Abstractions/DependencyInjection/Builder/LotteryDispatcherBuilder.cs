using Baibaocp.LotteryDispatcher.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;

namespace Baibaocp.LotteryDispatcher.DependencyInjection.Builder
{
    public class LotteryDispatcherBuilder
    {
        public IServiceCollection Services { get; }

        internal LotteryDispatcherBuilder(IServiceCollection services)
        {
            Services = services;
        }

        private void AddHandlerDiscovery()
        {
            Services.Scan(scanner => 
            {
                scanner.FromApplicationDependencies()
                       .AddClasses(filter => filter.AssignableTo(typeof(IExecuteHandler<>)))
                       .AsSelf()
                       .WithTransientLifetime();
            });
        }

        public LotteryDispatcherBuilder ConfigureOptions(Action<LotteryDispatcherOptions> options)
        {
            Services.Configure(options);
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<LotteryDispatcherOptions>, LotteryDispatcherOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<LotteryDispatcherOptions>>().Value);
            AddHandlerDiscovery();
        }
    }
}
