using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryAwardingMessageSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _iocResolver;
        private readonly ILogger<LotteryOrderingMessageSubscriber> _logger;

        public LotteryAwardingMessageSubscriber(IBusClient busClient, IServiceProvider iocResolver, ILogger<LotteryOrderingMessageSubscriber> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _iocResolver = iocResolver;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryAwarded>>(async (message) =>
            {
                try
                {
                    _logger.LogInformation("Received ticketing message: {1} {0}", message.LdpMerchanerId, message.LdpOrderId);
                    IOrderingApplicationService orderingApplicationService = _iocResolver.GetRequiredService<IOrderingApplicationService>();
                    ILotteryMerchanterApplicationService lotteryMerchanterApplicationService = _iocResolver.GetRequiredService<ILotteryMerchanterApplicationService>();

                    var order = await orderingApplicationService.WinningAsync(message.LdpOrderId, message.Content.BonusAmount, message.Content.AftertaxBonusAmount);
                    await lotteryMerchanterApplicationService.Rewarding(order.Id, order.LdpVenderId, order.LotteryId, order.InvestAmount);
                    await lotteryMerchanterApplicationService.Rewarding(order.LvpOrderId, order.LvpVenderId, order.LotteryId, order.InvestAmount);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Received ticketing message: {1} {0}", message.LdpMerchanerId, message.LdpOrderId);
                }

                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryNoticing")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("LotteryOrdering.Awards")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Awarded.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
