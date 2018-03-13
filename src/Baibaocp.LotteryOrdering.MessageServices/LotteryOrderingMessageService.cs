using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
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
    public class LotteryOrderingMessageService : ILotteryOrderingMessageService
    {
        private readonly IBusClient _busClient;
        private readonly ISchedulerManager _schedulerManager;
        private readonly IIdentityGenerater _identityGenerater;
        private readonly IOrderingApplicationService _orderingApplicationService;
        private readonly ILogger<LotteryOrderingMessageService> _logger;
        private readonly ILotteryDispatcherMessageService<OrderingExecuteMessage> _lotteryDispatcherMessageService;

        public LotteryOrderingMessageService(IBusClient busClient, ISchedulerManager schedulerManager, IIdentityGenerater identityGenerater, IOrderingApplicationService orderingApplicationService, ILogger<LotteryOrderingMessageService> logger, ILotteryDispatcherMessageService<OrderingExecuteMessage> lotteryDispatcherMessageService)
        {
            _logger = logger;
            _busClient = busClient;
            _schedulerManager = schedulerManager;
            _identityGenerater = identityGenerater;
            _orderingApplicationService = orderingApplicationService;
            _lotteryDispatcherMessageService = lotteryDispatcherMessageService;
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
                    configuration.WithRoutingKey("Orders.Accepted.#");
                });
            });
        }

        public Task SubscribeAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LvpOrderedMessage>(async (message) =>
            {
                long ldpOrderId = _identityGenerater.Generate();
                string ldpVenderId = "450022";
                try
                {
                    OrderingExecuteMessage executeMessage = new OrderingExecuteMessage(ldpOrderId.ToString(), ldpVenderId, message);
                    await _orderingApplicationService.CreateAsync(message.LvpOrderId, message.LvpUserId, message.LvpVenderId, message.LotteryId, message.LotteryPlayId, message.IssueNumber, message.InvestCode, message.InvestType, message.InvestCount, message.InvestTimes, message.InvestAmount);
                    await _lotteryDispatcherMessageService.PublishAsync(ldpVenderId, executeMessage);

                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", ldpOrderId, ldpVenderId);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering executer:{0} VenderId:{1}", ldpOrderId, ldpVenderId);
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
                        queue.WithName("Orders.Ordering")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("Orders.Accepted.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
