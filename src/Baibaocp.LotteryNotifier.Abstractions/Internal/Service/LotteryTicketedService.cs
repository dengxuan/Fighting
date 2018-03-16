using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.Notifiers;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.Storaging.Entities;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration.Exchange;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.Services
{
    public class LotteryTicketedService : BackgroundService
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
                _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1} LdpOrderId:{2} LdpVenderId:{3}", message.OrderId, message.VenderId);
                return await _dispatcher.DispatchAsync(message);
            }, stoppingToken);
        }
    }
}
