﻿using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryOrderingMessageSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _iocResolver;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly ILogger<LotteryOrderingMessageSubscriber> _logger;
        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;

        public LotteryOrderingMessageSubscriber(IBusClient busClient, IServiceProvider iocResolver, IIdentityGenerater identityGenerater, ILogger<LotteryOrderingMessageSubscriber> logger, IDispatchOrderingMessageService dispatchOrderingMessageService)
        {
            _logger = logger;
            _busClient = busClient;
            _iocResolver = iocResolver;
            _identityGenerater = identityGenerater;
            _dispatchOrderingMessageService = dispatchOrderingMessageService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LvpOrderMessage>(async (message) =>
            {
                try
                {
                    ILotteryMerchanterApplicationService lotteryMerchanterApplicationService = _iocResolver.GetRequiredService<ILotteryMerchanterApplicationService>();
                    /* 此处必须保证投注渠道已经开通相应的彩种和出票渠道*/
                    string ldpVenderId = await lotteryMerchanterApplicationService.FindLdpMerchanterIdAsync(message.LvpVenderId, message.LotteryId);
                    if (string.IsNullOrEmpty(ldpVenderId))
                    {
                        _logger.LogError("当前投注渠道{0}不支持该彩种{1}", message.LvpVenderId, message.LotteryId);
                        return new Nack();
                    }
                    IOrderingApplicationService orderingApplicationService = _iocResolver.GetRequiredService<IOrderingApplicationService>();
                    LotteryMerchanteOrder lotteryMerchanteOrder = await orderingApplicationService.CreateAsync(message.LvpOrderId, message.LvpUserId, message.LvpVenderId, message.LotteryId, message.LotteryPlayId, message.IssueNumber, message.InvestCode, message.InvestType, message.InvestCount, message.InvestTimes, message.InvestAmount);
                    await _dispatchOrderingMessageService.PublishAsync(lotteryMerchanteOrder.Id, ldpVenderId, message);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering :{0}", message.LvpOrderId);
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
                        queue.WithName("LotteryOrdering.Orders")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Accepted.#");
#if DEBUG
                        consume.WithPrefetchCount(1);
#endif
                    });
                });
            }, stoppingToken);
        }
    }
}
