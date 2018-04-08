using Fighting.Scheduling.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using Fighting.Timing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fighting.Scheduling.Mysql
{
    public class ScheduleStorage : IScheduleStore
    {

        private readonly IRepository<Schedule, long> _scheduleRepository;

        public ScheduleStorage(IRepository<Schedule, long> scheduleRepository)
        {
            _scheduleRepository = scheduleRepository;
        }

        public async Task DeleteAsync(Schedule schedule)
        {
            await _scheduleRepository.DeleteAsync(schedule);
        }

        public Task<Schedule> GetAsync(long id)
        {
            return _scheduleRepository.GetAsync(id);
        }

        public Task<List<Schedule>> GetWaitingSchedulesAsync(int maxResultCount)
        {
            var schedules = _scheduleRepository.GetAll()
                                            .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                                            .OrderByDescending(t => t.Priority)
                                            .ThenBy(t => t.TryCount)
                                            .ThenBy(t => t.NextTryTime)
                                            .Take(maxResultCount)
                                            .ToList();
            return Task.FromResult(schedules);
        }

        public async Task InsertAsync(Schedule schedule)
        {
            await _scheduleRepository.InsertAsync(schedule);
        }

        public async Task UpdateAsync(Schedule schedule)
        {
            await _scheduleRepository.UpdateAsync(schedule);
        }
    }
}
