using Baibaocp.ApplicationServices.Abstractions;
using Fighting.ApplicationServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.ApplicationServices.DependencyInjection
{
    public static class BaibaocpApplicationServiceBuilderExtensions
    {
        public static ApplicationServiceBuilder UseBaibaocpApplicationService(this ApplicationServiceBuilder applicationServiceBuilder)
        {
            applicationServiceBuilder.Services.AddSingleton<ILotteryMerchanterApplicationService, LotteryMerchanterApplicationService>();
            return applicationServiceBuilder;
        }
    }
}
