using Baibaocp.LotteryDispatching.Executers;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Fighting.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.MessagesSevices;
using Baibaocp.LotteryOrdering.Scheduling.DependencyInjection;
using Baibaocp.LotteryDispatching.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Baibaocp.LotteryOrdering.MessageServices.DependencyInjection;
using Fighting.MessageServices.DependencyInjection;
using Fighting.Scheduling;
using Fighting.Scheduling.DependencyInjection;
using Fighting.Scheduling.Mysql.DependencyInjection;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration(configurationBuilder =>
                {
                    configurationBuilder.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
                    configurationBuilder.AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureMessageServices(messageServiceBuilder => 
                        {
                            messageServiceBuilder.UseLotteryOrderingMessageServices();
                            messageServiceBuilder.UseLotteryDispatchingMessageService();
                        });

                        fightBuilder.ConfigureScheduling(setupAction =>
                        {
                            setupAction.AddLotteryOrderingScheduling();
                            SchedulingConfiguration schedulingOptions = hostContext.Configuration.GetSection("SchedulingConfiguration").Get<SchedulingConfiguration>();
                            setupAction.UseMysqlStorage(schedulingOptions);
                        });
                        fightBuilder.ConfigureApplicationServices(applicationServiceBuilder => 
                        {
                            applicationServiceBuilder.UseLotteryOrderingApplicationService();
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
                .ConfigureLogging(logging => logging.AddConsole());



            var host = builder.Build();
            await host.StartAsync();
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
