using Fighting.Scheduling;
using Fighting.Scheduling.Mysql;
using Fighting.Storaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace Baibaocp.Storaging.EntityFrameworkCore
{
    public class ScheduleStorageContextFactory : IDesignTimeDbContextFactory<ScheduleDbContext>
    {
        public ScheduleDbContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<ScheduleDbContext>();
            SchedulingConfiguration schedulingOptions = builder.GetSection("SchedulingConfiguration").Get<SchedulingConfiguration>();
            optionsBuilder.UseMySql(schedulingOptions.DefaultConnection);
            return new ScheduleDbContext(new StorageOptions { }, optionsBuilder.Options);
        }
    }
}
