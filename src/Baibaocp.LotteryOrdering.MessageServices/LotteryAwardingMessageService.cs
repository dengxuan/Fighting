﻿using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryAwardingMessageService : ILotteryAwardingMessageService
    {
        private readonly IBusClient _busClient;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILogger<LotteryOrderingMessageService> _logger;

        public LotteryAwardingMessageService(IBusClient busClient, IOrderingApplicationService orderingApplicationService, ILogger<LotteryOrderingMessageService> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _orderingApplicationService = orderingApplicationService;
        }

        public Task PublishAsync(LdpAwardedMessage message)
        {
            return _busClient.PublishAsync(message, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryOrdering")
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryOrdering.{message.AwardingType}.{message.LdpVenderId}");
                });
            });
        }

        public Task SubscribeAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LdpAwardedMessage>(async (message) =>
            {
                try
                {
                    await _orderingApplicationService.RewardedAsync(0, message.BonusAmount, (int)message.AwardingType);

                    _logger.LogTrace("Received ticketing message: {1} {0}", message.LdpVenderId, message.LdpOrderId);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogTrace("Received ticketing message: {1} {0}", message.LdpVenderId, message.LdpOrderId);
                }

                ///* 投注失败 */
                //await _client.PublishAsync(new LdpTicketedMessage
                //{
                //    LvpOrder = message,
                //    Status = OrderStatus.TicketFailed
                //}, context => context.UsePublishConfiguration(configuration =>
                //{
                //    configuration.OnDeclaredExchange(exchange =>
                //    {
                //        exchange.WithName("Baibaocp.LotteryVender")
                //                .WithDurability(true)
                //                .WithAutoDelete(false)
                //                .WithType(ExchangeType.Topic);
                //    });
                //    configuration.WithRoutingKey(RoutingkeyConsts.Orders.Completed.Failure);
                //}));

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
                        queue.WithName("LotteryOrdering.Orders")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Accepted.#");
                    });
                });
            }, stoppingToken);
        }
    }
}