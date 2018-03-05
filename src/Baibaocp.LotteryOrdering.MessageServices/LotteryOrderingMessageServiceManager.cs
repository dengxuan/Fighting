﻿using Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Abstractions;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryOrderingMessageServiceManager : ILotteryOrderingMessageServiceManager
    {
        private readonly IBusClient _busClient;
        private readonly ISchedulerManager _schedulerManager;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILogger<LotteryOrderingMessageServiceManager> _logger;
        private readonly ILotteryDispatchingMessageServiceManager _lotteryDispatchingMessageServiceManager;

        public Task PublishAsync(LvpOrderedMessage orderingMessage)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LvpOrderedMessage>(async (lvpOrderedMessage) =>
            {
                long ldpOrderId = _identityGenerater.Generate();
                string ldpVenderId = "";
                try
                {
                    OrderingExecuteMessage orderingExecuteMessage = new OrderingExecuteMessage(ldpOrderId.ToString(), ldpVenderId, lvpOrderedMessage);
                    LotteryMerchanteOrder lotteryMerchanteOrder = new LotteryMerchanteOrder();

                    await _orderingApplicationService.CreateAsync(lotteryMerchanteOrder);
                    await _lotteryDispatchingMessageServiceManager.PublishAsync(ldpVenderId,orderingExecuteMessage);

                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", ldpOrderId, ldpVenderId);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering executer:{0} VenderId:{1}", ldpOrderId, ldpVenderId);
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
