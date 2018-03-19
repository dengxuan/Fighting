using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Fighting.Hosting;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class OrderingDispatcherService : BackgroundService
    {
        private readonly DispatcherConfiguration _dispatcherOptions;
        private readonly ILogger<OrderingDispatcherService> _logger;
        private readonly IOrderingDispatcher _orderingDispatcher;
        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;
        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;
        private readonly ILotteryTicketingMessageService _lotteryTicketingMessageService;

        public OrderingDispatcherService(DispatcherConfiguration dispatcherOptions, ILogger<OrderingDispatcherService> logger, IOrderingDispatcher orderingDispatcher, IDispatchOrderingMessageService dispatchOrderingMessageService, ILotteryTicketingMessageService lotteryTicketingMessageService, IDispatchQueryingMessageService dispatchQueryingMessageService)
        {
            _logger = logger;
            _dispatcherOptions = dispatcherOptions;
            _orderingDispatcher = orderingDispatcher;
            _dispatchOrderingMessageService = dispatchOrderingMessageService;
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
            _lotteryTicketingMessageService = lotteryTicketingMessageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _dispatchOrderingMessageService.SubscribeAsync(_dispatcherOptions.MerchanterName, async (message) =>
            {
                var handle = await _orderingDispatcher.DispatchAsync(message);
                switch (handle)
                {
                    case AcceptedHandle accepted:
                        {
                            _logger.LogInformation($"投注成功: {message.LdpVenderId}-{message.LvpOrder.LvpOrderId}-{message.LdpOrderId}");
                            await _dispatchQueryingMessageService.PublishAsync(message.LdpVenderId, message.LdpOrderId, QueryingTypes.Ticketing);
                            return true;
                        }
                    case RejectedHandle rejected:
                        {
                            if (rejected.Reorder)
                            {
                                return false;
                            }
                            await _lotteryTicketingMessageService.PublishAsync(new LdpTicketedMessage { LdpOrderId = message.LdpOrderId, LdpVenderId = message.LdpVenderId, TicketingType = LotteryTicketingTypes.Failure });
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
