using Microsoft.Extensions.Options;

namespace Fighting.Extensions.UnitOfWork.DependencyInjection.Builder
{
    internal class UnitOfWorkOptionsSetup : ConfigureOptions<UnitOfWorkOptions>
    {
        public UnitOfWorkOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(UnitOfWorkOptions options)
        {
        }
    }
}
