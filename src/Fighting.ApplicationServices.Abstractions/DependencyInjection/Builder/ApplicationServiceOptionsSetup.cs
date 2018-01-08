using Microsoft.Extensions.Options;

namespace Fighting.ApplicationServices.DependencyInjection.Builder
{
    internal class ApplicationServiceOptionsSetup : ConfigureOptions<ApplicationServiceOptions>
    {
        public ApplicationServiceOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(ApplicationServiceOptions options)
        {
        }
    }
}
