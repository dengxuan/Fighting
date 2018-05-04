using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Fighting.Extensions.UnitOfWork.Abstractions;
using Fighting.Hosting;
using Fighting.Scheduling.Abstractions;
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
    public class LotteryTicketingMessageSubscriber : BackgroundService
    {
        private readonly IBusClient _busClient;
        private readonly IServiceProvider _iocResolver;
        private readonly ISchedulerManager _schedulerManager;
        private readonly ILogger<LotteryTicketingMessageSubscriber> _logger;

        public LotteryTicketingMessageSubscriber(IBusClient busClient, ISchedulerManager schedulerManager, IServiceProvider iocResolver, ILogger<LotteryTicketingMessageSubscriber> logger)
        {
            _logger = logger;
            _busClient = busClient;
            _iocResolver = iocResolver;
            _schedulerManager = schedulerManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _busClient.SubscribeAsync<NoticeMessage<LotteryTicketed>>(async (message) =>
            {
                try
                {
                    _logger.LogInformation("Received ticketing message: {0} {1} Content:{2}", message.LdpMerchanerId, message.LdpOrderId, message.Content);
                    IOrderingApplicationService orderingApplicationService = _iocResolver.GetRequiredService<IOrderingApplicationService>();
                    IUnitOfWorkManager unitOfWorkManager = _iocResolver.GetRequiredService<IUnitOfWorkManager>();
                    using (var uow = unitOfWorkManager.Begin())
                    {
                        if (message.Content.TicketingType == LotteryTicketingTypes.Success)
                        {
                            var order = await orderingApplicationService.TicketedAsync(message.LdpOrderId, message.LdpMerchanerId, message.Content.TicketedNumber, message.Content.TicketedTime, message.Content.TicketedOdds);
                            if(order != null)
                            {
                                await _schedulerManager.EnqueueAsync<ILotteryAwardingScheduler, AwardingScheduleArgs>(new AwardingScheduleArgs
                                {
                                    LdpOrderId = message.LdpOrderId,
                                    LdpMerchanerId = message.LdpMerchanerId,
                                    LvpOrderId = message.Content.LvpOrderId,
                                    LvpMerchanerId = message.Content.LvpMerchanerId
                                }, delay: order.ExpectedBonusTime - DateTime.Now);
                            }
                        }
                        else
                        {
                            await orderingApplicationService.RejectedAsync(message.LdpOrderId);
                        }
                        uow.Complete();
                        return new Ack();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex, "Received ticketing message: {0} {1} Content:{2}", message.LdpMerchanerId, message.LdpOrderId, message.Content);
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
