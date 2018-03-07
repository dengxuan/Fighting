using Fighting.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Scrutor;

namespace Fighting.DependencyInjection.Builder
{
    public class FightBuilder
    {
        public IServiceCollection Services { get; }

        internal FightBuilder(IServiceCollection services) => Services = services;

        internal void Build()
        {
            Services.Scan(scanner =>
            {
                scanner.FromApplicationDependencies()
                       .AddClasses(fiter => fiter.WithAttribute<ISingletonDependencyAttribute>())
                       .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                       .AsImplementedInterfaces()
                       .WithSingletonLifetime();
            });
            Services.Scan(scanner =>
            {
                scanner.FromApplicationDependencies()
                       .AddClasses(fiter => fiter.WithAttribute<TransientDependencyAttribute>())
                       .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                       .AsImplementedInterfaces()
                       .WithTransientLifetime();
            });

            Services.AddSingleton<IIdentityGenerater>(sp =>
            {
                FightOptions options = sp.GetRequiredService<FightOptions>();
                return new IdentityGenerater(options.MachineId, options.ProcessId);
            });

            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<FightOptions>, FightOptionsSetup>());
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<FightOptions>>().Value);
        }
    }
}
