using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryOrdering.MessageServices.DependencyInjection
{
    public static class LotteryOrderingMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryOrderingMessagePublisher(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddTransient<ILotteryOrderingMessageService, LotteryOrderingMessageService>();
            return messageServiceBuilder;
        }
    }
}
