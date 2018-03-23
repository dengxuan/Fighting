using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling.Abstractions
{
    public class LotteryQueryingScheduler : ILotteryQueryingScheduler
    {
        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;

        public LotteryQueryingScheduler(IDispatchQueryingMessageService dispatchQueryingMessageService)
        {
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
        }

        public Task RunAsync(QueryingScheduleArgs args)
        {
            return _dispatchQueryingMessageService.PublishAsync(args.LdpOrderId, args.LdpMerchanerId, args.LvpOrderId, args.LvpMerchanerId, args.QueryingType);
        }
    }
}
