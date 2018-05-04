using Fighting.DependencyInjection.Builder;
using Fighting.Scheduling.Abstractions;

namespace Baibaocp.Scheduling.Abstractions
{
    [SingletonDependency]
    public interface ILotteryPhaseScheduler : IScheduler<LotteryPhaseSchedulerArgs>
    {
    }
}
