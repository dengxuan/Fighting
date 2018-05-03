using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling
{
    public class LotteryOrderingScheduler : ILotteryOrderingScheduler
    {
        private readonly ILogger<LotteryOrderingScheduler> _logger;

        private readonly IDispatchOrderingMessageService _dispatchOrderingMessageService;

        public LotteryOrderingScheduler(IDispatchOrderingMessageService dispatchQueryingMessageService, ILogger<LotteryOrderingScheduler> logger)
        {
            _logger = logger;
            _dispatchOrderingMessageService = dispatchQueryingMessageService;
        }

        public async Task<bool> RunAsync(OrderingScheduleArgs args)
        {
            _logger.LogInformation("Execute Ordering Scheduler: {0}-{1}-{2}", args.LdpMerchanerId, args.LdpOrderId, args.Message);
            await _dispatchOrderingMessageService.PublishAsync(args.LdpOrderId, args.LdpMerchanerId, args.Message);
            return true;
        }
    }
}
