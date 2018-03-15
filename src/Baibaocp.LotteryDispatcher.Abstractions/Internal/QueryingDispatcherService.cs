using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.Hosting;
using Fighting.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class QueryingDispatcherService : BackgroundService
    {
        private readonly ISchedulerManager _schedulerManager;
        private readonly IQueryingDispatcher _queryingDispatcher;
        private readonly ILogger<OrderingDispatcherService> _logger;
        private readonly DispatcherConfiguration _dispatcherConfiguration;
        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;
        private readonly ILotteryTicketingMessageService _lotteryTicketingMessageService;

        public QueryingDispatcherService(DispatcherConfiguration dispatcherConfiguration, ILogger<OrderingDispatcherService> logger, IQueryingDispatcher queryingDispatcher, IDispatchQueryingMessageService dispatchQueryingMessageService, ILotteryTicketingMessageService lotteryTicketingMessageService, ISchedulerManager schedulerManager)
        {
            _logger = logger;
            _dispatcherConfiguration = dispatcherConfiguration;
            _schedulerManager = schedulerManager;
            _queryingDispatcher = queryingDispatcher;
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
            _lotteryTicketingMessageService = lotteryTicketingMessageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _dispatchQueryingMessageService.SubscribeAsync(_dispatcherConfiguration.MerchanterName, QueryingTypes.Ticketing, async (message) =>
            {
                var handle = await _queryingDispatcher.DispatchAsync(message);
                switch (handle)
                {
                    case SuccessHandle success:
                        {
                            return true;
                        }
                    case FailureHandle failure:
                        {
                            return true;
                        }
                    case WinningHandle winning:
                        {
                            return true;
                        }
                    case LoseingHandle loseing:
                        {
                            return true;
                        }
                    case WaitingHandle waiting:
                        {
                            return false;
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
