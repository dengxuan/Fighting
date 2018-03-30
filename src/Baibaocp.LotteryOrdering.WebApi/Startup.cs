using Baibaocp.LotteryOrdering.MessageServices.DependencyInjection;
using Fighting.DependencyInjection;
using Fighting.MessageServices.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RawRabbit.Configuration;
using RawRabbit.DependencyInjection.ServiceCollection;
using RawRabbit.Instantiation;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace Baibaocp.LotteryOrdering.WebApi
{

    /// <inheritdoc/>
    public class Startup
    {

        /// <inheritdoc/>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="env"></param>
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddFighting(fightBuilder =>
            {
                fightBuilder.ConfigureMessageServices(messageServiceBuilder =>
                {
                    messageServiceBuilder.UseLotteryOrderingMessagePublisher();
                });
            });

            services.AddRawRabbit(new RawRabbitOptions
            {
                ClientConfiguration = Configuration.GetSection("RawRabbitConfiguration").Get<RawRabbitConfiguration>(),
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

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
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
                //c.ConfigureOAuth2("100010", "secret", "", "Baibaocp.LotteryOrdering");
                c.SwaggerEndpoint("/api-docs/v1/swagger.json", "Lottery ordering api V1");
            });

        }
    }
}
