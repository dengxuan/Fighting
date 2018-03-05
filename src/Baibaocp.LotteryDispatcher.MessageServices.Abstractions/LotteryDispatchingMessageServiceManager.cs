using Baibaocp.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryDispatcher.MessageServices.Messages.ExecuteMessages;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
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

namespace Baibaocp.LotteryDispatcher.MessageServices
{
    public class LotteryDispatchingMessageServiceManager : ILotteryDispatchingMessageServiceManager
    {
        private readonly IBusClient _busClient;

        private readonly ISchedulerManager _schedulerManager;

        private readonly ILogger<LotteryDispatchingMessageServiceManager> _logger;

        private readonly IExecuterDispatcher<OrderingExecuteMessage> _dispatcher;

        private readonly ILotteryTicketingMessageServiceManager _lotteryTicketingMessageServiceManager;

        public Task PublishAsync(string merchanerId, OrderingExecuteMessage lvpOrderedMessage)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<OrderingExecuteMessage>(async (orderingExecuteMessage) =>
            {
                try
                {

                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", orderingExecuteMessage.LdpOrderId, orderingExecuteMessage.LdpVenderId);
                    MessageHandle handle = await _dispatcher.DispatchAsync(orderingExecuteMessage);
                    if (handle == MessageHandle.Accepted)
                    {
                        /* 投注成功，添加出票查询计划任务 */
                        TicketingExecuteMessage ticketingMessage = new TicketingExecuteMessage(orderingExecuteMessage.LdpOrderId, orderingExecuteMessage.LdpVenderId, orderingExecuteMessage.LvpOrder);
                        await _schedulerManager.EnqueueAsync<ILotteryTicketingScheduler<TicketingExecuteMessage>, TicketingExecuteMessage>(ticketingMessage);
                    }
                    else if (handle == MessageHandle.Rejected)
                    {
                        /* 投注失败，将数据存入队列，进行通知和*/
                        LdpTicketedMessage ldpTicketedMessage = new LdpTicketedMessage
                        {
                            LdpOrderId = orderingExecuteMessage.LdpOrderId,
                            LdpVenderId = orderingExecuteMessage.LdpVenderId,
                            LvpOrder = orderingExecuteMessage.LvpOrder,
                            Status = OrderStatus.TicketNotRecv,
                        };
                        await _lotteryTicketingMessageServiceManager.PublishAsync(ldpTicketedMessage);
                    }
                    //else if (handle == MessageHandle.Waiting)
                    //{
                    //    BackgroundJob.Schedule<IExecuterDispatcher<ExecuteOrderingMessage>>(dispatcher => dispatcher.DispatchAsync(executer), TimeSpan.FromSeconds(10));
                    //}
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error of the ordering executer:{0} VenderId:{1}", orderingExecuteMessage.LdpOrderId, orderingExecuteMessage.LdpVenderId);
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
                        queue.WithName($"Orders.Dispatching.{merchanerId}")
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
