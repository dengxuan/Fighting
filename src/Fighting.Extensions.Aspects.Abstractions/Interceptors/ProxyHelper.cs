using Fighting.Reflection;
using System;

namespace Fighting.Aspects.Interceptors
{
    public class ProxyHelper
    {
        public static bool ShouldProxy(Type serviceType)
        {
            if (typeof(IAspects).IsAssignableFrom(serviceType))
            {
                var attribute = CustomAttributeAccessor.GetCustomAttribute<AspectsAttribute>(serviceType, true);
                return attribute?.Disable != true;
            }
            return false;
        }
    }
}
