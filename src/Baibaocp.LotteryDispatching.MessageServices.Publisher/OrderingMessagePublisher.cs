using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
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
    public class OrderingMessagePublisher : IOrderingDispatcherMessagePublisher
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<OrderingMessagePublisher> _logger;

        public OrderingMessagePublisher(IBusClient busClient, ILogger<OrderingMessagePublisher> logger)
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
    }
}
