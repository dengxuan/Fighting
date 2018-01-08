using Baibaocp.LotteryOrdering.ApplicationServices;
using Baibaocp.LotteryOrdering.Messages;
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
        private readonly ILogger<LotteryTicketingService> _logger;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly IOrderingApplicationService _ticketingService;

        public LotteryTicketingService(IBusClient client, IIdentityGenerater identityGenerater, ILogger<LotteryTicketingService> logger, IOrderingApplicationService ticketingService)
        {
            _client = client;
            _logger = logger;
            _identityGenerater = identityGenerater;
            _ticketingService = ticketingService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<TicketedMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Ticketing received message:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                    await _ticketingService.UpdateAsync(message);
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
            } , stoppingToken);
        }
    }
}
