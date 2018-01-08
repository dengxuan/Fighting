using Baibaocp.LotteryDispatcher.Core.Executers;
using Baibaocp.LotteryDispatcher.DependencyInjection;
using Baibaocp.LotteryDispatcher.Internal;
using Baibaocp.LotteryDispatcher.Shanghai.DependencyInjection;
using Baibaocp.LotteryDispatcher.Shanghai.Handlers;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Enrichers.MessageContext.Context;
using RawRabbit.Instantiation;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatcher.LiangcaiHosting
{
    class Program
    {

        public static async Task Main(string[] args)
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
                        fightBuilder.ConfigureHangfire(configuration =>
                        {
                            configuration.UseRedisStorage(hostContext.Configuration.GetConnectionString("Hangfire.Redis"));
                        });

                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Fighting.Redis");
                            });
                        });

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.UseDapper(options =>
                            {
                                options.DefaultNameOrConnectionString = hostContext.Configuration.GetConnectionString("Fighting.Storage");
                            });
                        });

                        fightBuilder.AddLotteryDispatcher(dispatchBuilder =>
                        {
                            dispatchBuilder.AddShanghai(setupOptions =>
                            {
                                IConfiguration dispatchConfiguration = hostContext.Configuration.GetSection("DispatchConfiguration");
                                setupOptions.SecretKey = dispatchConfiguration.GetValue<string>("SecretKey");
                                setupOptions.Url = dispatchConfiguration.GetValue<string>("Url");
                            });
                            dispatchBuilder.ConfigureOptions(options =>
                            {
                                IConfiguration dispatchConfiguration = hostContext.Configuration.GetSection("DispatchConfiguration");
                                options.AddHandler<ShanghaiOrderingExecuteHandler, OrderingExecuter>(dispatchConfiguration.GetValue<string>("LdpVenderId"));
                                options.AddHandler<ShanghaiAwardingExecuteHandler, AwardingExecuter>(dispatchConfiguration.GetValue<string>("LdpVenderId"));
                                options.AddHandler<ShanghaiTicketingExecuteHandler, TicketingExecuter>(dispatchConfiguration.GetValue<string>("LdpVenderId"));
                            });
                        });

                        fightBuilder.ConfigureLotteryAwardCalculator(calculateBuilder =>
                        {
                        });

                        services.AddRawRabbit(new RawRabbitOptions
                        {
                            ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>(),
                            Plugins = p =>
                            {
                                p.UseMessageContext<MessageContext>();
                            },
                            DependencyInjection = ioc =>
                            {
                            }
                        });

                    });

                    services.AddSingleton(sp =>
                    {
                        NodeConfiguration options = hostContext.Configuration.GetSection("NodeConfiguration").Get<NodeConfiguration>();
                        return options;
                    });
                    services.AddSingleton<IHostedService, LiangcaiLotteryDispatcherService>();
                    services.AddSingleton<IHostedService, LotteryOrderingService>();
                });

            Console.WriteLine("Starting...");

            await host.RunConsoleAsync();
        }
    }
}
