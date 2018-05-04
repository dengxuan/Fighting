using Fighting.Aspects;
using Fighting.Aspects.DynamicProxy;
using Fighting.Aspects.Interceptors;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Extensions.Aspects.Abstractions.DependencyInjection
{
    public static class AspectsIServiceCollectionExtensions
    {
        public static IServiceCollection AddAspects(this IServiceCollection services)
        {
            services.AddSingleton<IProxyGenerator, ProxyGenerator>();
            services.AddSingleton<IInterceptorFactory, InterceptorFactory>();
            services.AddSingleton<IInterceptorChainBuilder, InterceptorChainBuilder>();
            services.AddSingleton<InvocationContext>();
            services.AddSingleton<IInterceptBuilder, InterceptBuilder>();
            return services;
        }
    }
}
