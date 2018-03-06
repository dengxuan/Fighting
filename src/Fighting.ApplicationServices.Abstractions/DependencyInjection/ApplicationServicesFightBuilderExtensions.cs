using Fighting.ApplicationServices.DependencyInjection.Builder;
using Fighting.DependencyInjection.Builder;
using System;

namespace Fighting.ApplicationServices.DependencyInjection
{
    public static class ApplicationServicesFightBuilderExtensions
    {
        public static FightBuilder ConfigureApplicationServices(this FightBuilder fightBuilder, Action<ApplicationServiceBuilder> setupAction)
        {
            ApplicationServiceBuilder builder = new ApplicationServiceBuilder(fightBuilder.Services);
            setupAction?.Invoke(builder);
            builder.Build();
            return fightBuilder;
        }
    }
}
