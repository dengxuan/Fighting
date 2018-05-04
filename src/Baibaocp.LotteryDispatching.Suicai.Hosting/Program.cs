﻿using Baibaocp.LotteryDispatcher.Liangcai.DependencyInjection;
using Baibaocp.LotteryDispatching.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Baibaocp.LotteryNotifier.MessageServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Fighting.MessageServices.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Suicai.Ordering
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

                        fightBuilder.ConfigureMessageServices(messageServiceBuilder =>
                        {
                            messageServiceBuilder.UseLotteryDispatchingMessagePublisher();
                            messageServiceBuilder.UseLotteryNoticingMessagePublisher();
                        });


                        fightBuilder.ConfigureLotteryDispatcher(dispatchBuilder =>
                        {
                            var dispatcherOptions = hostContext.Configuration.GetSection("DispatcherConfiguration").Get<DispatcherConfiguration>();
                            dispatchBuilder.UseSuicaiExecuteDispatcher(dispatcherOptions);
                        });

                        RawRabbitOptions Options = new RawRabbitOptions { ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>() };

                        services.AddRawRabbit(Options);

                    });
                });

            await host.RunConsoleAsync();
        }
    }
}
