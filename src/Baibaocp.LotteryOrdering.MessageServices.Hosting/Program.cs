using Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Baibaocp.LotteryOrdering.MessageServices.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Baibaocp.LotteryOrdering.MessagesSevices;
using Baibaocp.LotteryOrdering.Scheduling.DependencyInjection;
using Fighting.Abstractions;
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
using System;
using System.Threading.Tasks;
using Baibaocp.ApplicationServices.DependencyInjection;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.Storaging.EntityFrameworkCore.Repositories;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Baibaocp.Storaging.Entities;
using Fighting.Storaging.Entities.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using Baibaocp.Storaging.Entities.Merchants;
using System.Text;

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
                            messageServiceBuilder.UseLotteryDispatchingMessageServices();
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
                    });
                    services.AddSingleton<IHostedService, LotteryTicketingService>();
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>(),
                    });
                })
                .ConfigureLogging(logging => logging.AddConsole());

            var host = builder.Build();
            await host.StartAsync();
            var messageService = host.Services.GetRequiredService<ILotteryOrderingMessageService>();
            var identityGenerater = host.Services.GetRequiredService<IIdentityGenerater>();

            await messageService.PublishAsync(new Messages.LvpOrderedMessage {
                LvpOrderId = identityGenerater.Generate().ToString(),
                InvestAmount = 200,
                InvestCode = "01,02,03,04,05,06*01",
                InvestCount = 1,
                InvestTimes = 1,
                InvestType = false,
                LotteryId = 1,
                IssueNumber = 2018029,
                LotteryPlayId = 10011071,
                LvpUserId = 1000,
                LvpVenderId = "10081000345"
            });
            Console.ReadLine();
            await host.StopAsync();
        }
    }
}
