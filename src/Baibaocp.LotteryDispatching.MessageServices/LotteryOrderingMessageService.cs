using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Baibaocp.Storaging.Entities;
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
    public class LotteryOrderingMessageService : ILotteryDispatcherMessageService<OrderingMessage>
    {
        private readonly IBusClient _busClient;

        private readonly ISchedulerManager _schedulerManager;

        private readonly ILogger<LotteryOrderingMessageService> _logger;

        private readonly IExecuteDispatcher<OrderingMessage> _dispatcher;

        private readonly ILotteryTicketingMessageService _lotteryTicketingMessageServiceManager;

        public Task PublishAsync(string merchanerId, OrderingMessage lvpOrderedMessage)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<OrderingMessage>(async (executer) =>
            {
                try
                {

                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                    MessageHandle handle = await _dispatcher.DispatchAsync(executer);
                    if (handle == MessageHandle.Accepted)
                    {
                        /* 投注成功，添加出票查询计划任务 */
                        await _schedulerManager.EnqueueAsync<ILotteryTicketingScheduler, TicketingScheduleArgs>(new TicketingScheduleArgs { LdpOrderId = executer.LdpOrderId, LdpVenderId = executer.LdpVenderId, LvpOrderId = executer.LvpOrder.LvpOrderId });
                    }
                    else if (handle == MessageHandle.Rejected)
                    {
                        /* 投注失败，将数据存入队列，进行通知和*/
                        LdpTicketedMessage ldpTicketedMessage = new LdpTicketedMessage
                        {
                            LdpOrderId = executer.LdpOrderId,
                            LdpVenderId = executer.LdpVenderId,
                            LvpOrder = executer.LvpOrder,
                            Status = OrderStatus.TicketNotRecv,
                        };
                        await _lotteryTicketingMessageServiceManager.PublishAsync(ldpTicketedMessage);
                    }
                    else if (handle == MessageHandle.Waiting)
                    {
                        /* 上游暂停接单 */
                        return new Nack();
                    }
                    return new Ack();
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
                        exchange.WithName("Baibaocp.LotteryVender")
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
                        consume.WithRoutingKey("Orders.Storaged.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
