using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Baibaocp.LotteryOrdering.MessageServices.LotteryDispatcher.Abstractions;
using Baibaocp.LotteryOrdering.MessagesSevices;
using Fighting.ApplicationServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Fighting.Orleans.DependenceInjection;
using Fighting.Orleans.DependencyInjection;
using Fighting.Orleans.ClientCluster.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System;
using System.Net;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Fighting.Orleans;
using Fighting.ApplicationServices.Abstractions;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var siloPort = 11000;
            int gatewayPort = 31000;
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
                        });

                        fightBuilder.ConfigureOrleans(orleansSetup =>
                        {
                            orleansSetup.UseClientCluster<IApplicationService>(new OrleansOptions
                            {
                                ClusterAddress = IPAddress.Loopback,
                                ClusterId = "ApplicationService",
                                ClusterPort = 30000
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
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>(),
                        //Plugins = p =>
                        //{
                        //    p.UseMessageContext<MessageContext>();
                        //}
                    });
                })
                .UseDevelopmentClustering(options => options.PrimarySiloEndpoint = new IPEndPoint(siloAddress, siloPort))
                .ConfigureEndpoints(siloAddress, siloPort, gatewayPort)
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(Assembly.GetExecutingAssembly()).WithReferences())
                .ConfigureLogging(logging => logging.AddConsole());



            var host = builder.Build();
            IHostedService service = host.Services.GetRequiredService<IHostedService>();
            await service.StartAsync(CancellationToken.None);
            await host.StartAsync();
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
