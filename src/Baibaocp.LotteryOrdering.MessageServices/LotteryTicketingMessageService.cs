using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
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

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class LotteryTicketingMessageService : ILotteryTicketingMessageService
    {
        private readonly IBusClient _busClient;
        private readonly ISchedulerManager _schedulerManager;
        private readonly ILogger<LotteryOrderingMessageService> _logger;
        private readonly IOrderingApplicationService _orderingApplicationService;

        public LotteryTicketingMessageService(IBusClient busClient, ISchedulerManager schedulerManager, IOrderingApplicationService orderingApplicationService, ILogger<LotteryOrderingMessageService> logger)
        {
            _logger = logger;
            _schedulerManager = schedulerManager;
            _busClient = busClient;
            _orderingApplicationService = orderingApplicationService;
        }

        public Task PublishAsync(LdpTicketedMessage message)
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
                    configuration.WithRoutingKey($"LotteryOrdering.Ticketed.{message.LdpVenderId}");
                });
            });
        }

        public Task SubscribeAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<LdpTicketedMessage>(async (message) =>
            {
                try
                {
                    if (message.TicketingType == LotteryTicketingTypes.Success)
                    {
                        await _orderingApplicationService.TicketedAsync(long.Parse(message.LdpOrderId), message.LdpVenderId, message.TicketOdds);
                        await _schedulerManager.EnqueueAsync<ILotteryAwardingScheduler, AwardingScheduleArgs>(new AwardingScheduleArgs { });
                    }
                    else
                    {
                        await _orderingApplicationService.RejectedAsync(long.Parse(message.LdpOrderId));
                    }
                    _logger.LogTrace("Received ticketing message: {1} {0}", message.LdpVenderId, message.LdpOrderId);
                    return new Ack();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Received ticketing message: {1} {0}", message.LdpVenderId, message.LdpOrderId);
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
