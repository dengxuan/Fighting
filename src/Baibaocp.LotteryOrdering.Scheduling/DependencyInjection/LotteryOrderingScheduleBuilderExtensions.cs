using Baibaocp.LotteryOrdering.Scheduling.Abstractions;
using Fighting.Scheduling.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryOrdering.Scheduling.DependencyInjection
{
    public static class LotteryOrderingScheduleBuilderExtensions
    {
        public static SchedulingBuilder AddLotteryOrderingScheduling(this SchedulingBuilder schedulingBuilder)
        {
            schedulingBuilder.Services.AddTransient<ILotteryAwardingScheduler, LotteryAwardingScheduler>();
            schedulingBuilder.Services.AddTransient<ILotteryTicketingScheduler, LotteryTicketingScheduler>();
            return schedulingBuilder;
        }
    }
}
