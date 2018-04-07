using Fighting.Aspects.Interceptors;
using System.Collections.Generic;

namespace Fighting.Aspects
{
    internal class InterceptBuilder : IInterceptBuilder
    {
        private readonly List<IInterceptorProvider> _providers = new List<IInterceptorProvider>();

        public List<IInterceptorProvider> Providers => _providers;

        public InterceptBuilder Use<TInterceptorProvider>(TInterceptorProvider provider) where TInterceptorProvider : IInterceptorProvider
        {
            _providers.Add(provider);
            return this;
        }
    }
}
