using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryOrdering.Scheduling;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Fighting.Hosting;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class OrderingDispatcherService : BackgroundService
    {
        private readonly DispatcherConfiguration _dispatcherOptions;
        private readonly ILogger<OrderingDispatcherService> _logger;
        private readonly IOrderingDispatcher _orderingDispatcher;
        private readonly IOrderingMessageService _orderingMessageService;
        private readonly ISchedulerManager _schedulerManager;

        public OrderingDispatcherService(DispatcherConfiguration dispatcherOptions, ILogger<OrderingDispatcherService> logger, IOrderingDispatcher orderingDispatcher, IOrderingMessageService orderingMessageService, ISchedulerManager schedulerManager)
        {
            _logger = logger;
            _schedulerManager = schedulerManager;
            _dispatcherOptions = dispatcherOptions;
            _orderingDispatcher = orderingDispatcher;
            _orderingMessageService = orderingMessageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _orderingMessageService.SubscribeAsync(_dispatcherOptions.MerchanterName, async (message) =>
            {
                var handle = await _orderingDispatcher.DispatchAsync(message);
                switch (handle)
                {
                    case AcceptedHandle accepted:
                        {
                            _logger.LogInformation($"投注成功: {message.LdpVenderId}-{message.LvpOrder.LvpOrderId}-{message.LdpOrderId}");
                            await _schedulerManager.EnqueueAsync<ILotteryTicketingScheduler, TicketingScheduleArgs>(new TicketingScheduleArgs { LdpOrderId = message.LdpOrderId, LdpVenderId = message.LdpVenderId, LvpOrderId = message.LvpOrder.LvpOrderId });
                            return true;
                        }
                    case RejectedHandle rejected:
                        {
                            if (rejected.Reorder)
                            {
                                return false;
                            }
                            return true;
                        }
                    default:
                        {
                            return true;
                        }
                }
            }, stoppingToken);
        }
    }
}
