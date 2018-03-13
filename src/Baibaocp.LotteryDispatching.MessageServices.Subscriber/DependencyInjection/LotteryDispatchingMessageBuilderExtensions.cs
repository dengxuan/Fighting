using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

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
            return messageServiceBuilder;
        }
    }
}
