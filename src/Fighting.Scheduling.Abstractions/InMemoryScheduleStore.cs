using Fighting.Scheduling.Abstractions;
using Fighting.Timing;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Fighting.Scheduling
{
    public class InMemoryScheduleStore : IScheduleStore
    {
        private readonly Dictionary<long, Schedule> _schedules;

        private long _lastId;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryScheduleStore"/> class.
        /// </summary>
        public InMemoryScheduleStore()
        {
            _schedules = new Dictionary<long, Schedule>();
        }

        public Task<Schedule> GetAsync(long id)
        {
            return Task.FromResult(_schedules[id]);
        }

        public Task InsertAsync(Schedule schedule)
        {
            schedule.Id = Interlocked.Increment(ref _lastId);
            _schedules[schedule.Id] = schedule;

            return Task.FromResult(0);
        }

        public Task<List<Schedule>> GetWaitingSchedulesAsync(int maxResultCount)
        {
            var schedules = _schedules.Values.Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                                               .OrderByDescending(t => t.Priority)
                                               .ThenBy(t => t.TryCount)
                                               .ThenBy(t => t.NextTryTime)
                                               .Take(maxResultCount)
                                               .ToList();

            return Task.FromResult(schedules);
        }

        public Task DeleteAsync(Schedule schedule)
        {
            if (!_schedules.ContainsKey(schedule.Id))
            {
                return Task.FromResult(0);
            }

            _schedules.Remove(schedule.Id);

            return Task.FromResult(0);
        }

        public Task UpdateAsync(Schedule schedule)
        {
            if (schedule.IsAbandoned)
            {
                return DeleteAsync(schedule);
            }

            return Task.FromResult(0);
        }
    }
}
