﻿using Fighting.Aspects.DynamicProxy;
using Fighting.Reflection;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fighting.Aspects.Interceptors
{
    public class InterceptorFactory : IInterceptorFactory
    {

        private readonly IProxyGenerator _proxyGenerator;

        private readonly IInterceptBuilder _interceptBuilder;

        private readonly ConcurrentDictionary<Type, Dictionary<MethodInfo, InterceptorDelegate>> _typedInterceptors = new ConcurrentDictionary<Type, Dictionary<MethodInfo, InterceptorDelegate>>();

        /// <summary>
        /// Gets the interceptor chain builder.
        /// </summary>
        private readonly IInterceptorChainBuilder _chainBuilder;

        public InterceptorFactory(IInterceptorChainBuilder chainBuilder, IProxyGenerator proxyGenerator, IInterceptBuilder interceptBuilder)
        {
            _chainBuilder = chainBuilder;
            _proxyGenerator = proxyGenerator;
            _interceptBuilder = interceptBuilder;
        }

        /// <summary>
        /// Create a proxy wrapping specified target instance. 
        /// </summary>
        /// <param name="typeToProxy">The declaration type of proxy to create.</param>
        /// <param name="target">The target instance wrapped by the created proxy.</param>
        /// <returns>The proxy wrapping the specified target instance.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="typeToProxy"/> is null.</exception>
        /// <remarks>If the <paramref name="target"/> is null, this method will directly return null.</remarks>
        public object CreateProxy(Type typeToProxy, object target)
        {
            if (target == null)
            {
                return target;
            }
            Dictionary<MethodInfo, InterceptorDelegate> interceptors = _typedInterceptors.GetOrAdd(typeToProxy, CreateInterceptors);
            if (!interceptors.Any())
            {
                return target;
            }
            return this.CreateProxyCore(typeToProxy, target, interceptors);
        }


        /// <summary>
        /// Create a proxy wrapping specified target instance and interceptors. 
        /// </summary>
        /// <param name="typeToProxy">The declaration type of proxy to create.</param>
        /// <param name="target">The target instance wrapped by the created proxy.</param>
        /// <param name="initerceptors">The interceptors specific to each methods.</param>
        /// <returns>The proxy wrapping the specified target instance.</returns>
        /// <exception cref="ArgumentNullException">The argument <paramref name="typeToProxy"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The argument <paramref name="target"/> is null.</exception>
        /// <exception cref="ArgumentNullException">The argument <paramref name="initerceptors"/> is null.</exception>
        protected object CreateProxyCore(Type typeToProxy, object target, IDictionary<MethodInfo, InterceptorDelegate> initerceptors)
        {
            if (!initerceptors.Any())
            {
                return target;
            }
            IDictionary<MethodInfo, IInterceptor> dic = initerceptors.ToDictionary(it => it.Key, it => (IInterceptor)new DynamicProxyInterceptor(it.Value));
            var selector = new DynamicProxyInterceptorSelector(dic);
            var options = new ProxyGenerationOptions { Selector = selector };
            if (typeToProxy.GetTypeInfo().IsInterface)
            {
                return _proxyGenerator.CreateInterfaceProxyWithTarget(typeToProxy, target, options, dic.Values.ToArray());
            }
            else
            {
                return _proxyGenerator.CreateClassProxyWithTarget(typeToProxy, target, options, dic.Values.ToArray());
            }
        }

        internal Dictionary<MethodInfo, InterceptorDelegate> CreateInterceptors(Type typeToProxy)
        {
            Dictionary<MethodInfo, InterceptorDelegate> interceptors = new Dictionary<MethodInfo, InterceptorDelegate>();
            foreach (var method in typeToProxy.GetTypeInfo().GetMethods())
            {
                if (method.DeclaringType != typeToProxy)
                {
                    continue;
                }
                var aspectsAttribute = CustomAttributeAccessor.GetCustomAttribute<AspectsAttribute>(method, true);
                if(aspectsAttribute?.Disable != true)
                {
                    var newBuilder = _chainBuilder.New();
                    foreach (var provider in _interceptBuilder.Providers)
                    {
                        provider.Use(newBuilder);
                    }
                    interceptors[method] = newBuilder.Build();

                }
            }
            return interceptors;
        }
    }
}
