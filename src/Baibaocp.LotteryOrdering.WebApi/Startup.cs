using Fighting.DependencyInjection;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Baibaocp.LotteryOrdering.WebApi
{

    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

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
                        options.ConnectionString = Configuration.GetConnectionString("Hangfire.Redis");
                    });
                });

                fightBuilder.ConfigureStorage(storageBuilder =>
                {
                    storageBuilder.UseDapper(options =>
                    {
                        options.DefaultNameOrConnectionString = Configuration.GetConnectionString("Hangfire.Storage");
                    });
                });
            });

            services.AddHangfire(configure =>
            {
                configure.UseRedisStorage(Configuration.GetConnectionString("Hangfire.Redis"));
            });

            services.AddMvc();
            
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Lottery Ordering api",
                    Description = "The unified entry for hangfire invocation to add background job to queues.",
                    TermsOfService = "None",
                    Contact = new Contact { Name = "icsharp", Url = "https://github.com/icsharp/Hangfire.Topshelf" }
                });

                options.DocInclusionPredicate((docName, description) => true);

                // Define the BearerAuth scheme that's in use
                options.AddSecurityDefinition("bearerAuth", new ApiKeyScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "Baibaocp.LotteryOrdering.WebApi.xml"));

                options.DescribeAllEnumsAsStrings();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                loggerFactory.AddConsole(Configuration.GetSection("Logging"));

                loggerFactory.AddDebug();
            }

            app.UseMvc();

            app.UseSwagger(setupAction =>
            {
                setupAction.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.ConfigureOAuth2("100010", "secret", "", "Baibaocp.LotteryOrdering");
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "Lottery ordering api V1");
            });

            //app.UseHangfireServer();

            app.UseHangfireDashboard();
        }
    }
}
