using Baibaocp.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Baibaocp.LotteryNotifier.MessageServices.DependencyInjection;
using Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Baibaocp.LotteryOrdering.MessageServices.DependencyInjection;
using Baibaocp.LotteryOrdering.Scheduling.DependencyInjection;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.ApplicationServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Hosting;
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
using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.DependencyInjection;
using Fighting.Extensions.UnitOfWork.DependencyInjection;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices
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
                            messageServiceBuilder.UseLotteryOrderingMessagePublisher();
                            messageServiceBuilder.UseLotteryNoticingMessagePublisher();
                            messageServiceBuilder.UseLotteryDispatchingMessagePublisher();
                        });

                        fightBuilder.ConfigureScheduling(setupAction =>
                        {
                            setupAction.AddLotteryOrderingScheduling();
                            SchedulingConfiguration schedulingOptions = hostContext.Configuration.GetSection("SchedulingConfiguration").Get<SchedulingConfiguration>();
                            setupAction.UseMysqlStorage(schedulingOptions);
                        });
                        fightBuilder.ConfigureApplicationServices(applicationServiceBuilder =>
                        {
                            applicationServiceBuilder.UseBaibaocpApplicationService();
                            applicationServiceBuilder.UseLotteryOrderingApplicationService();
                        });
                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Baibaocp.Redis");
                            });
                        });

                        fightBuilder.AddUnitOfWork(unitOfWorkBuilder =>
                        {
                            unitOfWorkBuilder.UseEntityFrameworkCore(typeof(LotteryOrderingDbContext), typeof(BaibaocpStorageContext));
                        });

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.AddEntityFrameworkCore<LotteryOrderingDbContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Baibaocp.Storage"));
                            });
                            storageBuilder.AddEntityFrameworkCore<BaibaocpStorageContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Baibaocp.Storage"));
                            });
                        });
                    });
                    services.AddSingleton<IHostedService, LotteryAwardingMessageSubscriber>();
                    services.AddSingleton<IHostedService, LotteryOrderingMessageSubscriber>();
                    services.AddSingleton<IHostedService, LotteryTicketingMessageSubscriber>();
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>(),
                    });
                })
                .ConfigureLogging(logging => logging.AddConsole());

            await builder.RunConsoleAsync();
        }
    }
}
