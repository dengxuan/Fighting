using Fighting.Scheduling.Abstractions;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fighting.Scheduling.Mysql
{
    public class ScheduleDbContext : StorageContext
    {
        public ScheduleDbContext(StorageOptions storageOptions, DbContextOptions<ScheduleDbContext> options) : base(storageOptions, options)
        {
        }

        public DbSet<Schedule> Schedules { get; set; }
    }
}
