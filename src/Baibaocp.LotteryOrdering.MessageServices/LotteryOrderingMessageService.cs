using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Abstractions;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Common;
using RawRabbit.Configuration.Exchange;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryOrderingMessageService : ILotteryOrderingMessageService
    {
        private readonly IBusClient _busClient;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly ILogger<LotteryOrderingMessageService> _logger;
        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILotteryMerchanterApplicationService _lotteryMerchanterApplicationService;

        public LotteryOrderingMessageService(IBusClient busClient, IIdentityGenerater identityGenerater, IOrderingApplicationService orderingApplicationService, ILotteryMerchanterApplicationService lotteryMerchanterApplicationService, ILogger<LotteryOrderingMessageService> logger, IDispatchOrderingMessageService dispatchOrderingMessageService)
        {
            _logger = logger;
            _busClient = busClient;
            _identityGenerater = identityGenerater;
            _orderingApplicationService = orderingApplicationService;
            _lotteryMerchanterApplicationService = lotteryMerchanterApplicationService;
            _dispatchOrderingMessageService = dispatchOrderingMessageService;
        }

        public Task PublishAsync(LvpOrderedMessage orderingMessage)
        {
            return _busClient.PublishAsync(orderingMessage, context =>
            {
                context.UsePublishConfiguration(configuration =>
                {
                    configuration.OnDeclaredExchange(exchange =>
                    {
                        exchange.WithName("Baibaocp.LotteryOrdering")
                                .WithAutoDelete(false)
                                .WithType(ExchangeType.Topic);
                    });
                    configuration.WithRoutingKey($"LotteryOrdering.Accepted.{orderingMessage.LvpVenderId}");
                });
            });
        }

        public Task SubscribeAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LvpOrderedMessage>(async (message) =>
            {
                try
                {
                    /* 此处必须保证投注渠道已经开通相应的彩种和出票渠道*/
                    string ldpVenderId = await _lotteryMerchanterApplicationService.FindLdpVenderId(message.LvpVenderId, message.LotteryId);
                    if (string.IsNullOrEmpty(ldpVenderId))
                    {
                        _logger.LogError("当前投注渠道{0}不支持该彩种{1}", message.LvpVenderId, message.LotteryId);
                        return new Nack();
                    }
                    LotteryMerchanteOrder lotteryMerchanteOrder = await _orderingApplicationService.CreateAsync(message.LvpOrderId, message.LvpUserId, message.LvpVenderId, message.LotteryId, message.LotteryPlayId, message.IssueNumber, message.InvestCode, message.InvestType, message.InvestCount, message.InvestTimes, message.InvestAmount);
                    await _dispatchOrderingMessageService.PublishAsync(ldpVenderId, lotteryMerchanteOrder.Id, message);
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
                    });
                });
            }, stoppingToken);
        }
    }
}
