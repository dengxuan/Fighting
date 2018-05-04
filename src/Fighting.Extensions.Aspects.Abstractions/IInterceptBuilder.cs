using Fighting.Aspects.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Aspects
{
    public interface IInterceptBuilder
    {
        List<IInterceptorProvider> Providers { get; }
    }
}
