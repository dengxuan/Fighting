using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryOrdering.MessageServices.DependencyInjection
{
    public static class LotteryOrderingMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryOrderingMessageServices(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddTransient<ILotteryAwardingMessageService, LotteryAwardingMessageService>();
            messageServiceBuilder.Services.AddTransient<ILotteryOrderingMessageService, LotteryOrderingMessageService>();
            messageServiceBuilder.Services.AddTransient<ILotteryTicketingMessageService, LotteryTicketingMessageService>();
            return messageServiceBuilder;
        }
    }
}
