using Microsoft.Extensions.Options;

namespace Fighting.Orleans.DependencyInjection.Builder
{
    internal class OrleansOptionsSetup : ConfigureOptions<OrleansOptions>
    {
        public OrleansOptionsSetup(): base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(OrleansOptions options)
        {
        }
    }
}
