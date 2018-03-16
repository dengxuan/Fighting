using Baibaocp.LotteryNotifier.DependencyInjection;
using Baibaocp.LotteryNotifier.Internal.Services;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Hongdan
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
                    if (hostContext.HostingEnvironment.IsDevelopment())
                    {
                        loggerBuilder.AddConsole();
                        loggerBuilder.AddDebug();
                        loggerBuilder.SetMinimumLevel(LogLevel.Trace);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                    });
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>()
                    });
                    services.AddLotteryNotifier(builderAction =>
                    {
                        builderAction.ConfigureOptions(options =>
                        {
                            var noticeConfiguration = hostContext.Configuration.GetSection("NoticeConfiguration").Get<NoticeConfiguration>();
                            builderAction.Services.AddSingleton(noticeConfiguration);
                        });
                    });

                    services.AddScoped<IHostedService, LotteryAwardedService>();
                    services.AddScoped<IHostedService, LotteryTicketedService>();
                });
            await host.RunConsoleAsync();
        }
    }
}
