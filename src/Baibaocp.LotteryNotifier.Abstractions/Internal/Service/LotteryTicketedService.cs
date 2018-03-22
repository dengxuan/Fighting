using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.Services
{
    internal class LotteryTicketedService : BackgroundService
    {
        private readonly ITicketingNoticeMessageService _ticketingNoticeMessageService;

        private readonly ITicketingNotifier _dispatcher;

        private readonly ILogger<LotteryTicketedService> _logger;

        public LotteryTicketedService(ITicketingNotifier dispatcher, ITicketingNoticeMessageService ticketingNoticeMessageService, ILogger<LotteryTicketedService> logger)
        {
            _ticketingNoticeMessageService = ticketingNoticeMessageService;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _ticketingNoticeMessageService.SubscribeAsync(async (message) =>
            {
                _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1}", message.Content, message.VenderId);
                return await _dispatcher.DispatchAsync(message);
            }, stoppingToken);
        }
    }
}
