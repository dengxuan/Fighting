using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class LotteryOrderingMessageSubscriber : ILotteryDispatcherMessageSubscriber
    {
        private readonly IBusClient _busClient;

        private readonly IOrderingExecuteDispatcher _dispatcher;

        private readonly ILogger<LotteryOrderingMessageSubscriber> _logger;

        public LotteryOrderingMessageSubscriber(IBusClient busClient, IOrderingExecuteDispatcher dispatcher, ILogger<LotteryOrderingMessageSubscriber> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _dispatcher = dispatcher;
        }

        public Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<OrderingExecuteMessage>(async (executer) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                    IExecuteHandle handle = await _dispatcher.DispatchAsync(executer);
                    bool result = await handle.HandleAsync();
                    if (result == true)
                    {
                        return new Ack();
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                }
                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatcher")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName($"{_dispatcher.Name}.Ordering")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"LotteryDispatcher.Ordering.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
