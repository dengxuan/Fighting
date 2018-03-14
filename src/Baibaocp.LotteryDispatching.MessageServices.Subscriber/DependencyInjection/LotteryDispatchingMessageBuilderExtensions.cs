using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Subscriber.Internal;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Baibaocp.LotteryDispatching.MessageServices.DependencyInjection
{
    public static class LotteryDispatchingMessageBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessageSubscriber(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.Scan(scanner =>
            {
                scanner.FromApplicationDependencies()
                       .AddClasses(filter => filter.AssignableTo<ILotteryDispatcherMessageSubscriber>())
                       .AsImplementedInterfaces()
                       .WithSingletonLifetime();
            });
            messageServiceBuilder.Services.AddSingleton<IHostedService, LotteryDispatcherMessageService>();
            return messageServiceBuilder;
        }
    }
}
