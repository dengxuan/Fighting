using Fighting.DependencyInjection;
using Fighting.Scheduling.Abstractions;
using Fighting.Scheduling.DependencyInjection.Builder;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace Fighting.Scheduling.Mysql.DependencyInjection
{
    public static class SchedulingBuilderExtensions
    {
        public static SchedulingBuilder UseMysqlStorage(this SchedulingBuilder schedulingBuilder, SchedulingConfiguration schedulingConfiguration)
        {
            schedulingBuilder.Services.TryAddSingleton<IScheduleStore, ScheduleStorage>();
            schedulingBuilder.FightBuilder.ConfigureStorage(setupAction =>
            {
                setupAction.AddEntityFrameworkCore<ScheduleDbContext>(optionsBuilder =>
                {
                    optionsBuilder.UseMySql(schedulingConfiguration.DefaultConnection);
                });
            });
            return schedulingBuilder;
        }
    }
}
