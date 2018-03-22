﻿using Baibaocp.LotteryDispatcher.Liangcai.DependencyInjection;
using Baibaocp.LotteryDispatching.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Liangcai.Awarding
{
    class Program
    {
        public async static Task Main(string[] args)
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

                        fightBuilder.ConfigureLotteryDispatcher(dispatchBuilder =>
                        {
                            var dispatcherOptions = hostContext.Configuration.Get<DispatcherConfiguration>();
                            dispatchBuilder.UseLiangcaiExecuteDispatcher(dispatcherOptions);
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
