using Baibaocp.LotteryDispatcher.MessageServices;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.Internal
{
    public class ShanghaiLotteryOrderingService : BackgroundService
    {
        private readonly ILogger<ShanghaiLotteryOrderingService> _logger;
        private readonly ILotteryDispatchingMessageServiceManager _lotteryOrderingMessageServiceManager;

        public ShanghaiLotteryOrderingService(LotteryDispatchingMessageServiceManager lotteryOrderingMessageServiceManager, ILogger<ShanghaiLotteryOrderingService> logger)
        {
            _logger = logger;
            _lotteryOrderingMessageServiceManager = lotteryOrderingMessageServiceManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _lotteryOrderingMessageServiceManager.SubscribeAsync("", stoppingToken);
        }
    }
}
