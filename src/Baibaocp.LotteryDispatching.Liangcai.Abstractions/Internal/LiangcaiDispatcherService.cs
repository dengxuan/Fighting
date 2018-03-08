using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Liangcai.Internal
{
    public class LiangcaiDispatcherService<TExecuter> : BackgroundService where TExecuter : IExecuter
    {
        private readonly ILogger<LiangcaiDispatcherService<TExecuter>> _logger;
        private readonly DispatcherOptions _dispatcherOptions;
        private readonly ILotteryDispatcherMessageService<TExecuter> _lotteryOrderingMessageServiceManager;

        public LiangcaiDispatcherService(ILotteryDispatcherMessageService<TExecuter> lotteryOrderingMessageServiceManager, DispatcherOptions dispatcherOptions, ILogger<LiangcaiDispatcherService<TExecuter>> logger)
        {
            _logger = logger;
            _dispatcherOptions = dispatcherOptions;
            _lotteryOrderingMessageServiceManager = lotteryOrderingMessageServiceManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _lotteryOrderingMessageServiceManager.SubscribeAsync(_dispatcherOptions.MerchanterId, stoppingToken);
        }
    }
}
