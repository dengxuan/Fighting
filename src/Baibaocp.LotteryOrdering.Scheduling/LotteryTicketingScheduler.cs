using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling
{
    public class LotteryTicketingScheduler : ILotteryTicketingScheduler
    {
        private readonly ILotteryTicketingMessageService _ticketingMessageService;

        public async Task RunAsync(TicketingScheduleArgs args)
        {
            await _ticketingMessageService.PublishAsync(new LdpTicketedMessage { LdpOrderId = args.LdpOrderId, LdpVenderId = args.LdpVenderId, TicketingType = LotteryTicketingTypes.Success, });
        }
    }
}
