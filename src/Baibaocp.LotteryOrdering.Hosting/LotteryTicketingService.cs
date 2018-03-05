using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Hosting
{
    public class LotteryTicketingService : BackgroundService
    {
        private readonly IBusClient _client;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly ILogger<LotteryTicketingService> _logger;
        private readonly IOrderingApplicationService _orderingApplicationService;

        public LotteryTicketingService(IBusClient client, IIdentityGenerater identityGenerater, ILogger<LotteryTicketingService> logger, IOrderingApplicationService orderingApplicationService, ILotteryMerchanterApplicationService lotteryMerchanterApplicationService)
        {
            _client = client;
            _logger = logger;
            _identityGenerater = identityGenerater;
            _orderingApplicationService = orderingApplicationService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<LdpTicketedMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Ticketing received message:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                    var order = await _orderingApplicationService.FindOrderAsync(message.LdpOrderId);
                    if (message.Status == Storaging.Entities.OrderStatus.TicketDrawing)
                    {
                        order.LdpVenderId = message.LdpVenderId;
                        order.Status = (int)message.Status;
                        order.TicketOdds = message.TicketOdds;
                    }
                    else if (message.Status == Storaging.Entities.OrderStatus.TicketFailed)
                    {

                    }
                    await _orderingApplicationService.TicketedAsync(Convert.ToInt64(message.LvpOrder.LvpOrderId), message.LdpOrderId, message.TicketOdds, (int)message.Status);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex, "Ticketing received message error:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                }
                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryVender")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("Tickets.Storaging")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Tickets.Completed.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
