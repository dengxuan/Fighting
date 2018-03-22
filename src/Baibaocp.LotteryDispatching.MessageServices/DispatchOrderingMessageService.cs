﻿using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
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
    public class DispatchOrderingMessageService : IDispatchOrderingMessageService
    {
        private readonly IBusClient _busClient;

        private readonly ILogger<DispatchOrderingMessageService> _logger;

        public DispatchOrderingMessageService(IBusClient busClient, ILogger<DispatchOrderingMessageService> logger)
        {
            _logger = logger;
            _busClient = busClient;
        }

        public Task PublishAsync(string merchanerId, string ldpOrderId, LvpOrderedMessage message)
        {
            OrderingExecuteMessage orderingMessage = new OrderingExecuteMessage(ldpOrderId, merchanerId, message);

            return _busClient.PublishAsync(orderingMessage, context =>
            {
                context.UsePublishAcknowledge(false);
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryDispatching")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryDispatching.Ordering.{merchanerId}");
                });
            });
        }

        public Task SubscribeAsync(string merchanerName, Func<OrderingExecuteMessage, Task<bool>> subscriber, CancellationToken stoppingToken)
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
                        exchange.WithName("Baibaocp.LotteryDispatching")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName($"LotteryDispatching.{merchanerName}.Orders")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"LotteryDispatching.Ordering.#");
                    });
                });
            }, stoppingToken);
        }
    }
}