using System;

namespace Fighting.Aspects.Interceptors
{
    public interface IInterceptorFactory
    {
        object CreateProxy(Type typeToProxy, object target);
    }
}
