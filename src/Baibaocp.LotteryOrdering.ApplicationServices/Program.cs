using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Threading.Tasks;
using Fighting.DependencyInjection;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Baibaocp.LotteryOrdering.ApplicationServices.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = ClusterConfiguration.LocalhostPrimarySilo();
            config.AddMemoryStorageProvider();

            var builder = new SiloHostBuilder()
                .UseConfiguration(config)
                .ConfigureHostConfiguration(configurationBuilder => 
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(OrderingApplicationServices).Assembly).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole())
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Hangfire.Redis");
                            });
                        });

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.UseEntityFrameworkCore<LotteryOrderingDbContext>(options =>
                            {
                                options.DefaultNameOrConnectionString = hostContext.Configuration.GetConnectionString("Fighting.Storage");
                            });
                        });

                    });
                });

            var host = builder.Build();
            await host.StartAsync();
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
