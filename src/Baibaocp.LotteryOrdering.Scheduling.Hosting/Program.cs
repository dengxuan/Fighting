using Fighting.Hosting;
using System;
using Fighting.DependencyInjection;
using Fighting.Scheduling.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Fighting.MessageServices.DependencyInjection;
using System.Threading.Tasks;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using Microsoft.Extensions.Configuration;
using Baibaocp.LotteryOrdering.Scheduling.DependencyInjection;
using Fighting.Scheduling;
using Fighting.Scheduling.Mysql.DependencyInjection;

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
                .ConfigureLogging(logging => logging.AddConsole());

            await builder.RunConsoleAsync();
        }
    }
}
