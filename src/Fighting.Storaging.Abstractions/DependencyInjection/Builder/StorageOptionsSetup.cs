using Fighting.Storaging;
using Microsoft.Extensions.Options;

namespace Fighting.DependencyInjection.Builder
{
    internal class StorageOptionsSetup : ConfigureOptions<StorageConfiguration>
    {
        public StorageOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(StorageConfiguration options)
        {
        }
    }
}
