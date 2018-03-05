using Fighting.Storaging;
using Microsoft.Extensions.Options;

namespace Fighting.DependencyInjection.Builder
{
    internal class StorageOptionsSetup : ConfigureOptions<StorageOptions>
    {
        public StorageOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(StorageOptions options)
        {
        }
    }
}
