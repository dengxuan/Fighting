using Fighting.DependencyInjection.Builder;
using Fighting.Scheduling.Abstractions;

namespace Baibaocp.Scheduling.Abstractions
{
    [ISingletonDependency]
    public interface ILotteryPhaseScheduler : IScheduler<LotteryPhaseSchedulerArgs>
    {
    }
}
