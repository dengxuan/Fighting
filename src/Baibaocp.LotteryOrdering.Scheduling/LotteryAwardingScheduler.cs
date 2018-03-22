using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling.Abstractions
{
    public class LotteryAwardingScheduler : ILotteryAwardingScheduler
    {
        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;

        public LotteryAwardingScheduler(IDispatchQueryingMessageService dispatchQueryingMessageService)
        {
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
        }

        public Task RunAsync(AwardingScheduleArgs args)
        {
            return _dispatchQueryingMessageService.PublishAsync(args.LdpOrderId, args.LdpMerchanerId, args.LvpOrderId, args.LvpMerchanerId, QueryingTypes.Awarding);
        }
    }
}
