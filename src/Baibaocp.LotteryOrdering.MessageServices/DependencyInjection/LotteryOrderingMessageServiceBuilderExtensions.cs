using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryOrdering.MessageServices.DependencyInjection
{
    public static class LotteryOrderingMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryOrderingMessageServices(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<ILotteryAwardingMessageService, LotteryAwardingMessageService>();
            messageServiceBuilder.Services.AddSingleton<ILotteryOrderingMessageService, LotteryOrderingMessageService>();
            messageServiceBuilder.Services.AddSingleton<ILotteryTicketingMessageService, LotteryTicketingMessageService>();
            return messageServiceBuilder;
        }
    }
}
