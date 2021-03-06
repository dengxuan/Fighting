﻿using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Extensions.UnitOfWork.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryTrading.TradeLogging.Subscribers
{
    internal class LotteryAwardingMessageSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _iocResolver;
        private readonly ILogger<LotteryAwardingMessageSubscriber> _logger;

        public LotteryAwardingMessageSubscriber(IBusClient busClient, IServiceProvider iocResolver, ILogger<LotteryAwardingMessageSubscriber> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _iocResolver = iocResolver;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryAwarded>>(async (message) =>
            {
                try
                {
                    _logger.LogTrace("Received awarding message: {0} {1} {2}", message.LdpOrderId, message.LdpMerchanerId, message.Content.AwardingType);
                    if (message.Content.AwardingType == LotteryAwardingTypes.Winning)
                    {
                        IUnitOfWorkManager unitOfWorkManager = _iocResolver.GetRequiredService<IUnitOfWorkManager>();
                        using (var uow = unitOfWorkManager.Begin())
                        {
                            IOrderingApplicationService orderingApplicationService = _iocResolver.GetRequiredService<IOrderingApplicationService>();
                            var order = await orderingApplicationService.FindOrderAsync(message.LdpOrderId);
                            ILotteryMerchanterApplicationService lotteryMerchanterApplicationService = _iocResolver.GetRequiredService<ILotteryMerchanterApplicationService>();
                            await lotteryMerchanterApplicationService.Rewarding(message.LdpMerchanerId, order.Id, order.LotteryId, message.Content.BonusAmount);
                            await lotteryMerchanterApplicationService.Rewarding(order.LvpVenderId, order.Id, order.LotteryId, message.Content.BonusAmount);
                            uow.Complete();
                        }
                    }
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex, "Received awarding message: {0} {1} {2}", message.LdpOrderId, message.LdpMerchanerId, message.Content.AwardingType);
                }
                return new Nack();
            }, context =>
            {
                context.UseSubscribeConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryNoticing")
                                .WithDurability(true)
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.FromDeclaredQueue(queue =>
                    {
                        queue.WithName("LotteryTrading.TradeLogging.Awards")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Awarded.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
