using Fighting.Scheduling.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling.Abstractions
{
    public class ILotteryRewardingScheduler : IScheduler<object>
    {
        public Task RunAsync(object args)
        {
            throw new System.NotImplementedException();
        }
    }
}
