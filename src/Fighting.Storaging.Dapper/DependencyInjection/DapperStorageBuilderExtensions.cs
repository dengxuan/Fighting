using Fighting.DependencyInjection.Builder;
using Fighting.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.DependencyInjection
{
    public static class DapperStorageBuilderExtensions
    {
        public static StorageBuilder UseDapper(this StorageBuilder storageBuilder, Action<StorageOptions> options)
        {
            storageBuilder.Services.Configure(options);
            return storageBuilder;
        }
    }
}
