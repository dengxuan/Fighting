using Fighting.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using Fighting.DependencyInjection;
using Fighting.Scheduling.DependencyInjection;
using System.Threading.Tasks;
using Fighting.Scheduling.Abstractions;
using Baibaocp.Scheduling.Abstractions;
using Fighting.Scheduling.Mysql.DependencyInjection;
using Fighting.Scheduling;

namespace Baibaocp.Scheduling.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureHostConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureLogging((hostContext, loggerBuilder) =>
                {
                    loggerBuilder.AddConsole();
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerBuilder.AddDebug();
                    }
                    loggerBuilder.SetMinimumLevel(LogLevel.Trace);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureScheduling(setupAction =>
                        {
                            SchedulingConfiguration schedulingOptions = hostContext.Configuration.GetSection("SchedulingConfiguration").Get<SchedulingConfiguration>();
                            setupAction.UseMysqlStorage(schedulingOptions);
                        });
                    });
                })
                .Build();

            Console.WriteLine("Starting...");

            await host.StartAsync();
            //ISchedulerManager schedulerManager = host.Services.GetRequiredService<ISchedulerManager>();
            //for (int i = 0; i < 10000; i++)
            //{
            //    await schedulerManager.EnqueueAsync<ILotteryPhaseScheduler, LotteryPhaseSchedulerArgs>(new LotteryPhaseSchedulerArgs { });
            //}
            Console.CancelKeyPress += async (sender, e) =>
            {
                Console.WriteLine("Shutting down...");
                await host.StopAsync(new CancellationTokenSource(3000).Token);
                Environment.Exit(0);
            };
            await host.WaitForShutdownAsync();
        }
    }
}
