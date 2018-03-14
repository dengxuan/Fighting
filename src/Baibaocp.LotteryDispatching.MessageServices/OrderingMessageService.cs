using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class OrderingMessageService : IOrderingMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<OrderingMessageService> _logger;

        public OrderingMessageService(IBusClient busClient, ILogger<OrderingMessageService> logger)
        {
            _logger = logger;
            _busClient = busClient;
        }

        public Task PublishAsync(string merchanerId, string ldpOrderId, LvpOrderedMessage message)
        {
            OrderingExecuteMessage orderingMessage = new OrderingExecuteMessage(ldpOrderId, merchanerId, message);

            return _busClient.PublishAsync(orderingMessage, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatcher")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryDispatcher.Ordering.{merchanerId}");
                });
            });
        }

        public Task SubscribeAsync(string merchanerId, Func<OrderingExecuteMessage, Task<bool>> subscriber, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<OrderingExecuteMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering message:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                    bool? result = await subscriber?.Invoke(message);
                    if (result == true)
                    {
                        return new Ack();
                    }
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
                        exchange.WithName("Baibaocp.LotteryDispatcher")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName($"LotteryDispatcher.Ordering")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"LotteryDispatcher.Ordering.{merchanerId}");
                    });
                });
            }, stoppingToken);
        }
    }
}
