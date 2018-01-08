using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.Core.Executers;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal
{
    public class LotteryOrderingService : BackgroundService
    {
        private readonly IBusClient _client;
        private readonly ILogger<LotteryOrderingService> _logger;
        private readonly IExecuterDispatcher<OrderingExecuter> _dispatcher;

        public LotteryOrderingService(IBusClient client, IExecuterDispatcher<OrderingExecuter> dispatcher, ILogger<LotteryOrderingService> logger)
        {
            _client = client;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _client.SubscribeAsync<OrderingExecuter>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                    await _dispatcher.DispatchAsync(message);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering executer:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
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
                         queue.WithName("Orders.Dispatching")
                              .WithAutoDelete(false)
                              .WithDurability(true);
                     });
                     configuration.Consume(consume =>
                     {
                         consume.WithRoutingKey("Orders.Storaged.#");
                     });
                 });
             }, stoppingToken);
        }
    }
}
