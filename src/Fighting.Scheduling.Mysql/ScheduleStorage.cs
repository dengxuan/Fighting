using Fighting.Extensions.UnitOfWork.Abstractions;
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
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IRepository<Schedule, long> _scheduleRepository;

        public ScheduleStorage(IUnitOfWorkManager unitOfWorkManager, IRepository<Schedule, long> scheduleRepository)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _scheduleRepository = scheduleRepository;
        }

        public async Task DeleteAsync(Schedule schedule)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _scheduleRepository.DeleteAsync(schedule);
                uow.Complete();
            }
        }

        public Task<Schedule> GetAsync(long id)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                Task<Schedule> schedule = _scheduleRepository.GetAsync(id);

                uow.Complete();
                return schedule;
            }
        }

        public Task<List<Schedule>> GetWaitingSchedulesAsync(int maxResultCount)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
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

        public async Task InsertAsync(Schedule schedule)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _scheduleRepository.InsertAsync(schedule);
                uow.Complete();
            }
        }

        public async Task UpdateAsync(Schedule schedule)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _scheduleRepository.UpdateAsync(schedule);
                uow.Complete();
            }
        }
    }
}
