using Baibaocp.LotteryDispatching.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.Scheduling.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Fighting.MessageServices.DependencyInjection;
using Fighting.Scheduling;
using Fighting.Scheduling.DependencyInjection;
using Fighting.Scheduling.Mysql.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System;
using System.Threading.Tasks;
using Baibaocp.LotteryDispatching.Liangcai.Handlers;

namespace Baibaocp.LotteryDispatching.Liangcai.Ordering
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
                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Fighting.Redis");
                            });
                        });

                        fightBuilder.ConfigureScheduling(setupAction =>
                        {
                            setupAction.AddLotteryOrderingScheduling();
                            SchedulingConfiguration schedulingOptions = hostContext.Configuration.GetSection("SchedulingConfiguration").Get<SchedulingConfiguration>();
                            setupAction.UseMysqlStorage(schedulingOptions);
                        });

                        fightBuilder.ConfigureMessageServices(messageServiceBuilder =>
                        {
                            messageServiceBuilder.UseLotteryDispatchingMessageService();
                        });

                        fightBuilder.ConfigureLotteryDispatcher(dispatchBuilder =>
                        {
                            dispatchBuilder.UseLotteryDispatching<OrderingExecuteHandler, OrderingExecuteMessage>(setupOptions =>
                            {
                                IConfiguration dispatchConfiguration = hostContext.Configuration.GetSection("DispatchConfiguration");
                                setupOptions.MerchanterId = dispatchConfiguration.GetValue<string>("LdpVenderId");
                                setupOptions.SecretKey = dispatchConfiguration.GetValue<string>("SecretKey");
                                setupOptions.Url = dispatchConfiguration.GetValue<string>("Url");
                            });
                        });

                        services.AddRawRabbit(new RawRabbitOptions
                        {
                            ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>()
                        });

                    });
                });

            Console.WriteLine("Starting...");

            await host.RunConsoleAsync();
        }
    }
}
