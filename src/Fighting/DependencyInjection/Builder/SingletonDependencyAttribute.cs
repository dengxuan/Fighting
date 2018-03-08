using System;

namespace Fighting.DependencyInjection.Builder
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class ISingletonDependencyAttribute : Attribute
    {
    }
}
