using Fighting.Scheduling.Abstractions;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Fighting.Scheduling.Mysql
{
    public class ScheduleDbContext : StorageContext
    {
        public ScheduleDbContext(StorageConfiguration storageOptions, DbContextOptions<ScheduleDbContext> options) : base(storageOptions, options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Schedule> Schedules { get; set; }
    }
}
