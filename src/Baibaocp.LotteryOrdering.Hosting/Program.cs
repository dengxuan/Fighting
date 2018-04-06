﻿using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.Instantiation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Hosting
{

    class Program
    {

        static void Main(string[] args)
        {
            MainAsync().GetAwaiter().GetResult();
        }

        static async Task MainAsync()
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
                    services.AddSingleton(sp =>
                    {
                        return hostContext.Configuration.GetSection("HostingConfugiration").Get<HostingConfugiration>();
                    });

                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Hangfire.Redis");
                            });
                        });

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.UseEntityFrameworkCore<LotteryOrderingDbContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Fighting.Storage"));
                            });
                        });
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
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddScoped<IHostedService, LotteryOrderingService>();
                    services.AddScoped<IHostedService, LotteryTicketingService>();
                    services.AddScoped<IHostedService, LotteryAwardingService>();
                })
                .Build();

            Console.WriteLine("Starting...");

            await host.StartAsync();

            Console.CancelKeyPress += async (sender, e) =>
            {
                Console.WriteLine("Shutting down...");
                await host.StopAsync(new CancellationTokenSource(3000).Token);
                Environment.Exit(0);
            };
            await host.WaitForShutdownAsync();
        }
    }
}