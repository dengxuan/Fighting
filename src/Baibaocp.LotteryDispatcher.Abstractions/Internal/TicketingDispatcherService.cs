using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Handles;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Internal
{
    internal class TicketingDispatcherService : BackgroundService
    {
        private readonly ILogger<OrderingDispatcherService> _logger;
        private readonly DispatcherConfiguration _dispatcherConfiguration;
        private readonly ITicketingDispatcher _ticketingDispatcher;
        private readonly IQueryingMessageService _queryingMessageService;

        public TicketingDispatcherService(DispatcherConfiguration dispatcherConfiguration, ILogger<OrderingDispatcherService> logger, ITicketingDispatcher ticketingDispatcher, IQueryingMessageService queryingMessageService)
        {
            _logger = logger;
            _dispatcherConfiguration = dispatcherConfiguration;
            _ticketingDispatcher = ticketingDispatcher;
            _queryingMessageService = queryingMessageService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _queryingMessageService.SubscribeAsync(_dispatcherConfiguration.MerchanterName, QueryingTypes.Ticketing, async (message) =>
            {
                var handle = await _ticketingDispatcher.DispatchAsync(message);
                switch (handle)
                {
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
