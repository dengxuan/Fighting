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
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Orleans.Configuration;

namespace Baibaocp.LotteryOrdering.ApplicationServices.Hosting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var siloPort = 10000;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;

            var builder = new SiloHostBuilder()
                .Configure(options => options.ClusterId = "OrderingApplicationService")
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Fighting.Redis");
                            });
                        });

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.UseEntityFrameworkCore<LotteryOrderingDbContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Fighting.Storage"));
                            });
                        });

                    });
                })
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());


            var host = builder.Build();
            await host.StartAsync();
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
