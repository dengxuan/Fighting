using Baibaocp.Core;
using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Baibaocp.LotteryOrdering.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.Services
{
    public class LotteryTicketedService : BackgroundService
    {
        private readonly IBusClient _client;

        private readonly INoticeDispatcher _dispatcher;

        private readonly ILogger<LotteryTicketedService> _logger;

        public LotteryTicketedService(IBusClient client, INoticeDispatcher dispatcher, ILogger<LotteryTicketedService> logger)
        {
            _client = client;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<TicketedMessage>((message) =>
            {
                _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1} LdpOrderId:{2} LdpVenderId:{3}", message.LvpOrder.LvpOrderId, message.LvpOrder.LvpVenderId, message.LdpOrderId, message.LdpVenderId);
                _dispatcher.DispatchAsync(new Notifier<Ticketed>(message.LvpOrder.LvpVenderId)
                {
                    Notice = new Ticketed
                    {
                        OrderId = message.LvpOrder.LvpOrderId,
                        TicketOdds = message.TicketOdds,
                        Status = message.Status == Core.OrderStatus.TicketDrawing ? 10300 : 10301
                    }
                });
                return Task.CompletedTask;
            },context => 
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
                        queue.WithName("Tickets.Noticing")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Tickets.Completed.#");
                    });
                });
            });
        }
    }
}
