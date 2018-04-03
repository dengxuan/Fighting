using Baibaocp.LotteryNotifier.DependencyInjection;
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
                    loggerBuilder.AddConsole();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddFighting(fightBuilder =>
                    {
                        fightBuilder.ConfigureMessageServices(messageServiceBuilder =>
                        {
                            messageServiceBuilder.UseLotteryNoticingMessagePublisher();
                        });
                    });
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>()
                    });
                    services.AddLotteryNotifier(builderAction =>
                    {
                        builderAction.ConfigureOptions(options =>
                        {
                            options.Configuration = hostContext.Configuration.GetSection("NoticeConfiguration").Get<NoticeConfiguration>();
                        });
                    });
                });
            await host.RunConsoleAsync();
        }
    }
}
