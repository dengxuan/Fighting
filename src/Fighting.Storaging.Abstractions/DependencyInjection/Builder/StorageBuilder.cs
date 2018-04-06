﻿using Fighting.Storaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Fighting.DependencyInjection.Builder
{
    public class StorageBuilder
    {
        public IServiceCollection Services { get; }

        internal StorageBuilder(IServiceCollection services) => Services = services;


        internal void AddStorageServices()
        {
            Services.TryAddEnumerable(ServiceDescriptor.Transient<IConfigureOptions<StorageConfiguration>, StorageOptionsSetup>());
            Services.AddSingleton(sp => sp.GetRequiredService<IOptions<StorageConfiguration>>().Value);
        }

        internal void Build()
        {
            AddStorageServices();
        }
    }
}
