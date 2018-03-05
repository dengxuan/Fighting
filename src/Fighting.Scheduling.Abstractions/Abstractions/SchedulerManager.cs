using Fighting.Json;
using Fighting.Timing;
using System;
using System.Threading.Tasks;

namespace Fighting.Scheduling.Abstractions
{

    public class SchedulerManager : ISchedulerManager
    {
        private readonly IScheduleStore _store;

        public SchedulerManager(IScheduleStore store)
        {
            _store = store;
        }

        public async Task<bool> DeleteAsync(string id)
        {
            if (long.TryParse(id, out long finalId) == false)
            {
                throw new ArgumentException($"The schedule id '{id}' should be a number.", nameof(id));
            }

            Schedule schedule = await _store.GetAsync(finalId);
            if (schedule == null)
            {
                return false;
            }

            await _store.DeleteAsync(schedule);
            return true;
        }

        public async Task<string> EnqueueAsync<TScheduler, TArgs>(TArgs args, SchedulerPriority priority = SchedulerPriority.Normal, TimeSpan? delay = null) where TScheduler : IScheduler<TArgs>
        {
            var schedule = new Schedule
            {
                SchedulerType = typeof(TScheduler).AssemblyQualifiedName,
                SchedulerArgs = args.ToJsonString(),
                Priority = priority
            };

            if (delay.HasValue)
            {
                schedule.NextTryTime = Clock.Now.Add(delay.Value);
            }

            await _store.InsertAsync(schedule);

            return schedule.Id.ToString();
        }
    }
}
