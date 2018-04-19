using Baibaocp.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Baibaocp.LotteryTrading.TradeLogging.DependencyInjection;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.ApplicationServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.Extensions.UnitOfWork.DependencyInjection;
using Fighting.Hosting;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using System.Threading.Tasks;
using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.DependencyInjection;

namespace Baibaocp.LotteryTrading.TradeLogging.Hosting
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

                        fightBuilder.ConfigureTradeLogging();
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
