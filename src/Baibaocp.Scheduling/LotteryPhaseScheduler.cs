using Baibaocp.Scheduling.Abstractions;
using System;
using System.Threading.Tasks;

namespace Baibaocp.Scheduling
{
    public class LotteryPhaseScheduler : ILotteryPhaseScheduler
    {
        public Task Run(LotteryPhaseSchedulerArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
