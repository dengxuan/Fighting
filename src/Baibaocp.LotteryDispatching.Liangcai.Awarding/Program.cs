using Baibaocp.LotteryDispatching.Liangcai.Handlers;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Baibaocp.LotteryDispatching.DependencyInjection;
using Baibaocp.LotteryDispatching.MessageServices.Messages.Dispatching;

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
                            dispatchBuilder.UseLotteryDispatching<AwardingMessage>(setupOptions =>
                            {
                                IConfiguration dispatchConfiguration = hostContext.Configuration.GetSection("DispatchConfiguration");
                                setupOptions.SecretKey = dispatchConfiguration.GetValue<string>("SecretKey");
                                setupOptions.Url = dispatchConfiguration.GetValue<string>("Url");
                            });
                            dispatchBuilder.ConfigureOptions(options =>
                            {
                                IConfiguration dispatchConfiguration = hostContext.Configuration.GetSection("DispatchConfiguration");
                            });
                        });

                        //services.AddRawRabbit(new RawRabbitOptions
                        //{
                        //    ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>()
                        //});

                    });
                });

            Console.WriteLine("Starting...");

            await host.RunConsoleAsync();
        }
    }
}
