using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.Hosting;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessagesSevices
{
    public class LotteryTicketingService : BackgroundService
    {
        private readonly ILotteryTicketingMessageService _ticketingMessageService;

        public LotteryTicketingService(ILotteryTicketingMessageService ticketingMessageService)
        {
            _ticketingMessageService = ticketingMessageService;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return _ticketingMessageService.SubscribeAsync(stoppingToken);
        }
    }
}
