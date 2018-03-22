using Baibaocp.LotteryOrdering.Hosting;
using Fighting.DependencyInjection.Builder;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;

namespace Fighting.Scheduling.DependencyInjection.Builder
{
    public class SchedulingBuilder
    {
        public IServiceCollection Services { get; }

        public FightBuilder FightBuilder { get; }

        internal SchedulingBuilder(IServiceCollection services, FightBuilder fightBuilder)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            FightBuilder = fightBuilder ?? throw new ArgumentNullException(nameof(fightBuilder));
        }

        public SchedulingBuilder AddScheduleServer()
        {
            Services.TryAddSingleton<IHostedService, ScheduleHostingService>();
            return this;
        }

        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<SchedulingOptions>, SchedulingOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<SchedulingConfiguration>>().Value);
            Services.TryAddSingleton<IScheduleStore, InMemoryScheduleStore>();
            Services.TryAddSingleton<ISchedulerManager, SchedulerManager>();
        }
    }
}
