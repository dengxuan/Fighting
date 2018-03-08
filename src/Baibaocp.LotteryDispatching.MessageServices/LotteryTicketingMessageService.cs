using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
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
    public class LotteryTicketingMessageService : ILotteryDispatcherMessageService<TicketingMessage>
    {
        private readonly IBusClient _busClient;

        private readonly ISchedulerManager _schedulerManager;

        private readonly ILogger<LotteryTicketingMessageService> _logger;

        private readonly IExecuteDispatcher<TicketingMessage> _dispatcher;

        private readonly ILotteryTicketingMessageService _ticketingMessageService;

        public Task PublishAsync(string merchanerId, TicketingMessage executer)
        {
            throw new NotImplementedException();
        }

        public Task SubscribeAsync(string merchanerId, CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<TicketingMessage>(async (executer) =>
            {
                try
                {

                    _logger.LogTrace("Received ordering executer:{0} VenderId:{1}", executer.LdpOrderId, executer.LdpVenderId);
                    MessageHandle handle = await _dispatcher.DispatchAsync(executer);
                    if (handle == MessageHandle.Success)
                    {
                        /* 出票成功 */
                        await _schedulerManager.EnqueueAsync<LotteryAwardingScheduler, AwardingScheduleArgs>(new AwardingScheduleArgs { });
                    }
                    else if (handle == MessageHandle.Rejected)
                    {
                        /* 出票失败 */
                        await _ticketingMessageService.PublishAsync(new LdpTicketedMessage { });
                    }
                    else if (handle == MessageHandle.Waiting)
                    {
                        // 等待出票
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
