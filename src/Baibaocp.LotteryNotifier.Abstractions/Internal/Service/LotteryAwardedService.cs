using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.Abstractions.Abstractions;
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
    public class LotteryAwardedService : BackgroundService
    {
        private readonly IBusClient _client;

        private readonly IAwardingNotifier _dispatcher;

        private readonly ILogger<LotteryAwardedService> _logger;

        private readonly IAwardingNoticeMessageService _ticketingNoticeMessageService;

        public LotteryAwardedService(IBusClient client, IAwardingNotifier dispatcher, ILogger<LotteryAwardedService> logger, IAwardingNoticeMessageService ticketingNoticeMessageService)
        {
            _client = client;
            _logger = logger;
            _dispatcher = dispatcher;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _ticketingNoticeMessageService.SubscribeAsync(async (message) =>
            {
                _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1} ", message.OrderId, message.VenderId);
                return await _dispatcher.DispatchAsync(message);
            }, stoppingToken);
        }
    }
}
