﻿using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public class LotteryDispatcherMessageService<TExecuteMessage> : ILotteryDispatcherMessageService<TExecuteMessage> where TExecuteMessage : IExecuteMessage
    {
        private readonly IBusClient _busClient;

        private readonly ISchedulerManager _schedulerManager;

        private readonly ILogger<LotteryDispatcherMessageService<TExecuteMessage>> _logger;

        private readonly IExecuteDispatcher<TExecuteMessage> _dispatcher;

        public LotteryDispatcherMessageService(IBusClient busClient, ISchedulerManager schedulerManager, IExecuteDispatcher<TExecuteMessage> dispatcher, ILogger<LotteryDispatcherMessageService<TExecuteMessage>> logger)
        {
            _busClient = busClient;
            _schedulerManager = schedulerManager;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        public Task PublishAsync(string merchanerId, TExecuteMessage message)
        {
            return _busClient.PublishAsync(message, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryOrdering")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"Orders.Storaged.{merchanerId}");
                });
            });
        }

        public Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<TExecuteMessage>(async (executer) =>
            {
                try
                {
                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                    bool result = await _dispatcher.DispatchAsync(executer);
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
                        exchange.WithName("Baibaocp.LotteryOrdering")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName($"Orders.Dispatcher.{merchanerId}")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey($"Orders.Storaged.{merchanerId}");
                    });
                });
            }, stoppingToken);
        }
    }
}
