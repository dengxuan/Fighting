using Fighting.Scheduling.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling.Abstractions
{
    public class LotteryAwardingScheduler : IScheduler<AwardingScheduleArgs>
    {
        public Task RunAsync(AwardingScheduleArgs args)
        {
            throw new System.NotImplementedException();
        }
    }
}
