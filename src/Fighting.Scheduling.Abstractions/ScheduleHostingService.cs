using Fighting.Hosting;
using Fighting.Scheduling.Abstractions;
using Fighting.Timing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Hosting
{
    public class ScheduleHostingService : BackgroundService
    {

        private readonly IServiceProvider _iocResolver;

        private readonly ILogger<ScheduleHostingService> _logger;

        public ScheduleHostingService(IServiceProvider iocResolver, ILogger<ScheduleHostingService> logger)
        {
            _iocResolver = iocResolver;
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return Task.Run(async () =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    IScheduleStore store = _iocResolver.GetRequiredService<IScheduleStore>();
                    var schedules = await store.GetWaitingSchedulesAsync(100000);

                    foreach (var schedule in schedules)
                    {
                        await TryExecuteScheduleAsync(schedule);
                    }
                    Thread.Sleep(1000);
                }
            });
        }

        private async Task TryExecuteScheduleAsync(Schedule schedule)
        {
            try
            {
                var scheduleType = Type.GetType(schedule.SchedulerType);
                var scheduler = _iocResolver.GetService(scheduleType);
                try
                {
                    var schedulerExecuteMethod = scheduler.GetType().GetTypeInfo().GetMethod("RunAsync");
                    var argsType = schedulerExecuteMethod.GetParameters()[0].ParameterType;
                    var argsObject = JsonConvert.DeserializeObject(schedule.SchedulerArgs, argsType);

                    bool result = await (schedulerExecuteMethod.Invoke(scheduler, new[] { argsObject }) as Task<bool>);

                    if (result == false)
                    {
                        var nextTryTime = schedule.CalculateNextTryTime();
                        if (nextTryTime.HasValue)
                        {
                            schedule.NextTryTime = nextTryTime.Value;
                        }
                        schedule.TryCount++;
                        schedule.LastTryTime = Clock.Now;
                        await TryUpdate(schedule);
                    }
                    else
                    {
                        IScheduleStore store = _iocResolver.GetRequiredService<IScheduleStore>();
                        await store.DeleteAsync(schedule);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);

                    var nextTryTime = schedule.CalculateNextTryTime();
                    if (nextTryTime.HasValue)
                    {
                        schedule.NextTryTime = nextTryTime.Value;
                    }
                    else
                    {
                        schedule.IsAbandoned = true;
                    }

                    await TryUpdate(schedule);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());

                schedule.IsAbandoned = true;

                await TryUpdate(schedule);
            }
        }

        private async Task TryUpdate(Schedule schedule)
        {
            try
            {
                IScheduleStore store = _iocResolver.GetRequiredService<IScheduleStore>();
                await store.UpdateAsync(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.ToString());
            }
        }
    }
}
