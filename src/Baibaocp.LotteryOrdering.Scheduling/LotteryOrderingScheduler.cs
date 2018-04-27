using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling
{
    public class LotteryOrderingScheduler : ILotteryOrderingScheduler
    {

        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;

        public LotteryOrderingScheduler(IDispatchOrderingMessageService dispatchQueryingMessageService)
        {
            _dispatchOrderingMessageService = dispatchQueryingMessageService;
        }

        public async Task<bool> RunAsync(OrderingScheduleArgs args)
        {
            await _dispatchOrderingMessageService.PublishAsync(args.LdpOrderId, args.LdpMerchanerId, args.Message);
            return true;
        }
    }
}
