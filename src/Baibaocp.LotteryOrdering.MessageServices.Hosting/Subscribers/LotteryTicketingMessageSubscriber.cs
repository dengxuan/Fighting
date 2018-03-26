using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Fighting.Hosting;
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
    public class LotteryTicketingMessageSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly ISchedulerManager _schedulerManager;
        private readonly ILogger<LotteryOrderingMessageSubscriber> _logger;
        private readonly IOrderingApplicationService _orderingApplicationService;

        public LotteryTicketingMessageSubscriber(IBusClient busClient, ISchedulerManager schedulerManager, IOrderingApplicationService orderingApplicationService, ILogger<LotteryOrderingMessageSubscriber> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _schedulerManager = schedulerManager;
            _orderingApplicationService = orderingApplicationService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryTicketed>>(async (message) =>
            {
                try
                {
                    if (message.Content.TicketingType == LotteryTicketingTypes.Success)
                    {
                        var order = await _orderingApplicationService.TicketedAsync(message.LdpOrderId, message.LdpMerchanerId, message.Content.TicketedNumber, message.Content.TicketedTime, message.Content.TicketedOdds);
                        await _schedulerManager.EnqueueAsync<ILotteryAwardingScheduler, AwardingScheduleArgs>(new AwardingScheduleArgs
                        {
                            LdpOrderId = message.LdpOrderId,
                            LdpMerchanerId = message.LdpMerchanerId,
                            LvpOrderId = message.Content.LvpOrderId,
                            LvpMerchanerId = message.Content.LvpMerchanerId
                        }, delay: order.ExpectedBonusTime - DateTime.Now);
                    }
                    else
                    {
                        await _orderingApplicationService.RejectedAsync(message.LdpOrderId);
                    }
                    _logger.LogTrace("Received ticketing message: {1} {0}", message.LdpMerchanerId, message.LdpOrderId);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Received ticketing message: {1} {0}", message.LdpMerchanerId, message.LdpOrderId);
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
                        queue.WithName("LotteryOrdering.Tickets")
                             .WithAutoDelete(false)
                             .WithDurability(true);
                    });
                    configuration.Consume(consume =>
                    {
                        consume.WithRoutingKey("LotteryOrdering.Ticketed.#");
                    });
                });
            }, stoppingToken);
        }
    }
}
