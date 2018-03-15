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
    public class DispatchQueryingMessageService : IDispatchQueryingMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<DispatchQueryingMessageService> _logger;

        public DispatchQueryingMessageService(IBusClient busClient, ILogger<DispatchQueryingMessageService> logger)
        {
            _logger = logger;
            _busClient = busClient;
        }

        public Task PublishAsync(string merchanerId, string ldpOrderId, QueryingTypes queryingType)
        {
            QueryingExecuteMessage queryingMessage = new QueryingExecuteMessage(ldpOrderId, merchanerId, queryingType);
            return _busClient.PublishAsync(queryingMessage, context =>
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
                    configuration.WithRoutingKey($"LotteryDispatcher.Querying.{merchanerId}");
                });
            });
        }

        public Task SubscribeAsync(string merchanerName, QueryingTypes queryingType, Func<QueryingExecuteMessage, Task<bool>> subscriber, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<QueryingExecuteMessage>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", message.LdpOrderId, message.LdpVenderId);
                    var result = await subscriber?.Invoke(message);
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
                        queue.WithName($"LotteryDispatcher.{merchanerName}.{queryingType}")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"LotteryDispatcher.Querying.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
