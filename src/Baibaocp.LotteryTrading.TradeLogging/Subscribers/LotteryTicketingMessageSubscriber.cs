using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
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
using System.Transactions;

namespace Baibaocp.LotteryTrading.TradeLogging.Subscribers
{
    public class LotteryTicketingMessageSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _iocResolver;
        private readonly ILogger<LotteryTicketingMessageSubscriber> _logger;

        public LotteryTicketingMessageSubscriber(IBusClient busClient, IServiceProvider iocResolver, ILogger<LotteryTicketingMessageSubscriber> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _iocResolver = iocResolver;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryTicketed>>(async (message) =>
            {
                try
                {
                    if (message.Content.TicketingType == LotteryTicketingTypes.Success)
                    {
                        IOrderingApplicationService orderingApplicationService = _iocResolver.GetRequiredService<IOrderingApplicationService>();
                        var order = await orderingApplicationService.FindOrderAsync(message.LdpOrderId);
                        ILotteryMerchanterApplicationService lotteryMerchanterApplicationService = _iocResolver.GetRequiredService<ILotteryMerchanterApplicationService>();
                        using(TransactionScope transaction = new TransactionScope())
                        {
                            await lotteryMerchanterApplicationService.Ticketing(order.LdpVenderId, order.Id, order.LotteryId, order.InvestAmount);
                            await lotteryMerchanterApplicationService.Ticketing(order.LvpVenderId, order.LvpOrderId, order.LotteryId, order.InvestAmount);
                        }
                    }
                    _logger.LogTrace("Received ticketing message: {1} {0}", message.LdpMerchanerId, message.LdpOrderId);
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
                        queue.WithName("LotteryOrdering.Tickets")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Ticketed.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
