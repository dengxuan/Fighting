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
    public class LotteryAwardedService : BackgroundService
    {
        private readonly IBusClient _client;

        private readonly INoticeDispatcher _dispatcher;

        private readonly ILogger<LotteryAwardedService> _logger;

        public LotteryAwardedService(IBusClient client, INoticeDispatcher dispatcher, ILogger<LotteryAwardedService> logger)
        {
            _client = client;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<AwardedMessage>((message) =>
            {
                _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1} LdpOrderId:{2} LdpVenderId:{3}", message.LvpOrder.LvpOrderId, message.LvpOrder.LvpVenderId, message.LdpOrderId, message.LdpVenderId);
                _dispatcher.DispatchAsync(new Notifier<Awarded>(message.LvpOrder.LvpVenderId)
                {
                    Notice = new Awarded
                    {
                        OrderId = message.LvpOrder.LvpOrderId,
                        Status = message.Status == OrderStatus.TicketWinning ? 10400 : 10401,
                        Amount = message.BonusAmount
                    }
                });
                return Task.CompletedTask;
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
                        queue.WithName("Awards.Noticing")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Awards.Completed.#");
                    });
                });
            });
        }
    }
}
