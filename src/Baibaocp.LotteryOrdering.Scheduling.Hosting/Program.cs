using Fighting.Hosting;
using System;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Fighting.ApplicationServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Scheduling.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Baibaocp.LotteryCalculating.DependencyInjection;
using Fighting.MessageServices.DependencyInjection;
using System.Threading.Tasks;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using Microsoft.Extensions.Configuration;
using Baibaocp.LotteryOrdering.Scheduling.DependencyInjection;
using Fighting.Scheduling;
using Baibaocp.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection;
using Fighting.Scheduling.Mysql.DependencyInjection;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Baibaocp.Storaging.EntityFrameworkCore;
using Baibaocp.LotteryCalculating.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryOrdering.Scheduling.Hosting
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
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
                            messageServiceBuilder.UseLotteryDispatchingMessagePublisher();
                        });

                        fightBuilder.ConfigureApplicationServices(applicationServiceBuilder =>
                       {
                           applicationServiceBuilder.UseBaibaocpApplicationService();
                           applicationServiceBuilder.UseLotteryOrderingApplicationService();
                       });

                        fightBuilder.ConfigureCacheing(cacheingBuilder =>
                        {
                            cacheingBuilder.UseRedisCache(redisOptions =>
                            {
                                redisOptions.ConnectionString = hostContext.Configuration.GetConnectionString("Baibaocp.Redis");
                            });
                        });
                        fightBuilder.ConfigureLotteryCalculating(lotteryCaclulatingBuilder =>
                        {
                            lotteryCaclulatingBuilder.UseLotteryCalculating();
                        });
                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.UseEntityFrameworkCore<LotteryOrderingDbContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Baibaocp.Storage"));
                            });
                            storageBuilder.UseEntityFrameworkCore<BaibaocpStorageContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Baibaocp.Storage"));
                            });
                        });

                        fightBuilder.ConfigureScheduling(schedulingBuilder =>
                        {
                            schedulingBuilder.AddScheduleServer();
                            schedulingBuilder.AddLotteryOrderingScheduling();
                            SchedulingConfiguration schedulingOptions = hostContext.Configuration.GetSection("SchedulingConfiguration").Get<SchedulingConfiguration>();
                            schedulingBuilder.UseMysqlStorage(schedulingOptions);
                        });
                    });
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>(),
                    });
                })
                .ConfigureLogging(logging => logging.AddConsole()).Build();
            await builder.StartAsync();
            Console.ReadLine();
        }
    }
}
