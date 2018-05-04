using Baibaocp.LotteryNotifier.DependencyInjection;
using Baibaocp.LotteryNotifier.MessageServices.DependencyInjection;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.DependencyInjection;
using Fighting.Hosting;
using Fighting.MessageServices.DependencyInjection;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System.Threading.Tasks;
using Fighting.ApplicationServices.DependencyInjection;
using Baibaocp.ApplicationServices.DependencyInjection;
using Fighting.Extensions.UnitOfWork.DependencyInjection;
using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.DependencyInjection;

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
                        fightBuilder.ConfigureApplicationServices(applicationServiceBuilder =>
                        {
                            applicationServiceBuilder.UseBaibaocpApplicationService();
                        });
                        fightBuilder.ConfigureCacheing(cacheBuilder =>
                        {
                            cacheBuilder.UseRedisCache(options =>
                            {
                                options.ConnectionString = hostContext.Configuration.GetConnectionString("Baibaocp.Redis");
                            });
                        });

                        //fightBuilder.AddUnitOfWork(unitOfWorkBuilder =>
                        //{
                        //    unitOfWorkBuilder.UseEntityFrameworkCore(typeof(BaibaocpStorageContext));
                        //});

                        fightBuilder.ConfigureStorage(storageBuilder =>
                        {
                            storageBuilder.AddEntityFrameworkCore<BaibaocpStorageContext>(optionsBuilder =>
                            {
                                optionsBuilder.UseMySql(hostContext.Configuration.GetConnectionString("Baibaocp.Storage"));
                            });
                        });

                        fightBuilder.ConfigureMessageServices(messageServiceBuilder =>
                        {
                            messageServiceBuilder.UseLotteryNoticingMessagePublisher();
                        });
                    });
                    services.AddRawRabbit(new RawRabbitOptions
                    {
                        ClientConfiguration = hostContext.Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>()
                    });
                    services.AddLotteryNotifier(builderAction => { });
                });
            await host.RunConsoleAsync();
        }
    }
}
