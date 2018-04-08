using Fighting.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.Extensions.UnitOfWork.Abstractions.DependencyInjection
{
    public static class UnitOfWorkFightBuilderExtensions
    {
        public static StorageBuilder AddUnitOfWork(this StorageBuilder storageBuilder, Action<UnitOfWorkOptions> configure)
        {
            storageBuilder.Services.Configure(configure);
            return storageBuilder;
        }
    }
}
