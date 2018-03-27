using Baibaocp.LotteryDispatching.Suicai.WebApi;
using Baibaocp.LotteryDispatching.MessageServices.DependencyInjection;
using Baibaocp.LotteryNotifier.MessageServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.MessageServices.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using Fighting.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.ApplicationServices.DependencyInjection;
using Baibaocp.ApplicationServices.DependencyInjection;
using Baibaocp.LotteryOrdering.EntityFrameworkCore;
using Baibaocp.Storaging.EntityFrameworkCore;
using Fighting.Storaging.EntityFrameworkCore.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Baibaocp.LotteryDispatching.Suicai.WebApi
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment Env)
        {
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Env.ContentRootPath)
                 .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                 .AddJsonFile($"appsettings.{Env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFighting(fightBuilder =>
            {
                fightBuilder.ConfigureCacheing(cacheBuilder =>
                {
                    cacheBuilder.UseRedisCache(options =>
                    {
                        options.ConnectionString = Configuration.GetConnectionString("Fighting.Redis");
                    });
                });

                fightBuilder.ConfigureApplicationServices(applicationServiceBuilder =>
                {
                    applicationServiceBuilder.UseBaibaocpApplicationService();
                    applicationServiceBuilder.UseLotteryOrderingApplicationService();
                });

                fightBuilder.ConfigureMessageServices(messageServiceBuilder =>
                {
                    messageServiceBuilder.UseLotteryDispatchingMessagePublisher();
                    messageServiceBuilder.UseLotteryNoticingMessagePublisher();
                });


                fightBuilder.ConfigureStorage(storageBuilder =>
                {
                    storageBuilder.UseEntityFrameworkCore<LotteryOrderingDbContext>(optionsBuilder =>
                    {
                        optionsBuilder.UseMySql(Configuration.GetConnectionString("Baibaocp.Storage"));
                    });
                    storageBuilder.UseEntityFrameworkCore<BaibaocpStorageContext>(optionsBuilder =>
                    {
                        optionsBuilder.UseMySql(Configuration.GetConnectionString("Baibaocp.Storage"));
                    });
                });

                services.AddRawRabbit(new RawRabbitOptions
                {
                    ClientConfiguration = Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>()
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMiddleware<SuicaiNoticing>();
        }
    }
}
