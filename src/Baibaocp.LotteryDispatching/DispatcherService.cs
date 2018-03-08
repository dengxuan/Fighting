using Baibaocp.LotteryDispatching.MessageServices;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching
{
    public class DispatcherService<TExecuteMessage> : BackgroundService where TExecuteMessage : IExecuteMessage
    {
        private readonly ILogger<DispatcherService<TExecuteMessage>> _logger;
        private readonly DispatcherOptions _dispatcherOptions;
        private readonly ILotteryDispatcherMessageService<TExecuteMessage> _lotteryOrderingMessageService;

        public DispatcherService(ILotteryDispatcherMessageService<TExecuteMessage> lotteryOrderingMessageServiceManager, DispatcherOptions dispatcherOptions, ILogger<DispatcherService<TExecuteMessage>> logger)
        {
            _logger = logger;
            _dispatcherOptions = dispatcherOptions;
            _lotteryOrderingMessageService = lotteryOrderingMessageServiceManager;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _lotteryOrderingMessageService.SubscribeAsync(_dispatcherOptions.MerchanterId, stoppingToken);
        }
    }
}
