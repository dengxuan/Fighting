using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Hosting;
using Orleans.Runtime.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;

namespace Baibaocp.ApplicationServices
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
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences())
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
                            storageBuilder.UseEntityFrameworkCore<BaibaocpStorageContext>(options =>
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
