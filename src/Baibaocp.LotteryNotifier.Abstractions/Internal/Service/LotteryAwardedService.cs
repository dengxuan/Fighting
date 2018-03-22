using Baibaocp.LotteryNotifier.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Internal.Services
{
    internal class LotteryAwardedService : BackgroundService
    {

        private readonly IAwardingNotifier _dispatcher;

        private readonly ILogger<LotteryAwardedService> _logger;

        private readonly IAwardingNoticeMessageService _awardingNoticeMessageService;

        public LotteryAwardedService(IAwardingNotifier dispatcher, ILogger<LotteryAwardedService> logger, IAwardingNoticeMessageService awardingNoticeMessageService)
        {
            _logger = logger;
            _dispatcher = dispatcher;
            _awardingNoticeMessageService = awardingNoticeMessageService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _awardingNoticeMessageService.SubscribeAsync(async (message) =>
            {
                _logger.LogTrace("Received ordering LvpOrderId:{0} LvpVenderId:{1} ", message.Content.OrderId, message.VenderId);
                return await _dispatcher.DispatchAsync(message);
            }, stoppingToken);
        }
    }
}
