using Microsoft.Extensions.Options;

namespace Fighting.DependencyInjection.Builder
{
    internal class FightOptionsSetup : ConfigureOptions<FightOptions>
    {
        public FightOptionsSetup(): base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(FightOptions options)
        {
        }
    }
}
