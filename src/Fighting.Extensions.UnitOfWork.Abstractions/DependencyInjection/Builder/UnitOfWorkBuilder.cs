using Fighting.DependencyInjection.Builder;
using Fighting.Extensions.UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Fighting.Extensions.UnitOfWork.DependencyInjection.Builder
{
    public class UnitOfWorkBuilder
    {
        public IServiceCollection Services { get; }

        internal UnitOfWorkBuilder(IServiceCollection services) => Services = services;



        internal void AddUowServices()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<UnitOfWorkOptions>, UnitOfWorkOptionsSetup>());
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<UnitOfWorkOptions>>().Value);

            Services.AddSingleton<IUnitOfWorkDefaultOptions, UnitOfWorkDefaultOptions>();
        }

        internal void Build()
        {
            AddUowServices();
        }
    }
}
