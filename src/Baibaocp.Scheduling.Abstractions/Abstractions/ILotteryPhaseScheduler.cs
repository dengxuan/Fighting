using Fighting.DependencyInjection.Builder;
using Fighting.Scheduling.Abstractions;

namespace Baibaocp.Scheduling.Abstractions
{
    public interface ILotteryPhaseScheduler : IScheduler<LotteryPhaseSchedulerArgs>, ISingletonDependency
    {
    }
}
