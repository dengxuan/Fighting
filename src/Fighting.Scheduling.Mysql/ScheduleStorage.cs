using Fighting.Scheduling.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using Fighting.Storaging.Uow;
using Fighting.Timing;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fighting.Scheduling.Mysql
{
    public class ScheduleStorage : IScheduleStore
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IRepository<Schedule, long> _scheduleRepository;

        public ScheduleStorage(IRepository<Schedule, long> scheduleRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _unitOfWorkManager = unitOfWorkManager;
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
            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    _scheduleRepository.Count();
                    var schedules = _scheduleRepository.GetAll()
                                                    .Where(t => !t.IsAbandoned && t.NextTryTime <= Clock.Now)
                                                    .OrderByDescending(t => t.Priority)
                                                    .ThenBy(t => t.TryCount)
                                                    .ThenBy(t => t.NextTryTime)
                                                    .Take(maxResultCount)
                                                    .ToList();
                    uow.Complete();
                    return Task.FromResult(schedules);
                }
            }
            catch (System.Exception ex)
            {
                System.Console.WriteLine(ex);
                throw;
            }
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
