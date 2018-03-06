using Baibaocp.LotteryOrdering.MessageServices.LotteryDispatcher.Abstractions;
using Fighting.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessagesSevices
{
    public class LotteryOrderingService : BackgroundService
    {
        private readonly ILotteryOrderingMessageService _orderingMessageService;

        public LotteryOrderingService(ILotteryOrderingMessageService orderingMessageService)
        {
            _orderingMessageService = orderingMessageService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _orderingMessageService.SubscribeAsync(stoppingToken);
        }
    }
}
