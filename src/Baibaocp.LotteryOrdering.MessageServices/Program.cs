using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Fighting.DependencyInjection;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Fighting.ApplicationServices.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using System;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using Baibaocp.LotteryOrdering.MessagesSevices;
using Microsoft.Extensions.Hosting;
using Baibaocp.LotteryOrdering.MessageServices.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Orleans.Runtime;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var siloPort = 11111;
            int gatewayPort = 30000;
            var siloAddress = IPAddress.Loopback;

            var builder = new SiloHostBuilder()
                .Configure(options => options.ClusterId = "OrderingMessageService")
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureApplicationServices(applicationServiceBuilder =>
                        {
                            applicationServiceBuilder.Services.AddSingleton<Task<IGrainFactory>>(async sp =>
                            {
                               var client = new ClientBuilder()
                                    .ConfigureCluster(options => options.ClusterId = "OrderingApplicationSergice")
                                    .UseStaticClustering(options => options.Gateways.Add((new IPEndPoint(siloAddress, gatewayPort)).ToGatewayUri()))
                                    .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IOrderingApplicationService).Assembly).WithReferences())
                                    .ConfigureLogging(logging => logging.AddConsole())
                                    .Build();

                                await client.Connect();
                                return client;
                            });
                        });

                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Baibaocp.Redis");
                            });
                        });

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.UseEntityFrameworkCore<LotteryOrderingDbContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Baibaocp.Storage"));
                            });
                        });
                    });
                    services.AddSingleton<ILotteryOrderingMessageService, LotteryOrderingMessageService>();
                    services.AddSingleton<IHostedService, LotteryOrderingService>();
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
