using System.Collections.Generic;
using System.Threading.Tasks;

namespace Fighting.Scheduling.Abstractions
{
    public interface IScheduleStore
    {
        Task<Schedule> GetAsync(long id);

        Task InsertAsync(Schedule schedule);

        Task<List<Schedule>> GetWaitingSchedulesAsync(int maxResultCount);

        Task DeleteAsync(Schedule schedule);

        Task UpdateAsync(Schedule schedule);
    }
}
