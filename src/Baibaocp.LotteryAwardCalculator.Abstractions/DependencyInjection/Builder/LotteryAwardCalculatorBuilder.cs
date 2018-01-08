using Baibaocp.LotteryAwardCalculator;
using Baibaocp.LotteryAwardCalculator.Abstractions;
using Baibaocp.LotteryAwardCalculator.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Baibaocp.DependencyInjection.Builder
{
    public class LotteryAwardCalculatorBuilder
    {
        public IServiceCollection Services { get; set; }

        internal LotteryAwardCalculatorBuilder(IServiceCollection services) => Services = services;


        internal void Build()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<LotteryAwardCalculatorOptions>, LotteryAwardCalculatorOptionsSetup>());
            Services.AddSingleton(c => c.GetRequiredService<IOptions<LotteryAwardCalculatorOptions>>().Value);

            Services.AddSingleton<CalculateHandler>();
            Services.AddSingleton<ICalculator, Calculator>();
            Services.AddSingleton<IHostedService, LotteryAwarderService>();
        }
    }
}
