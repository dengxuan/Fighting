using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fighting.Reflection.Extensions
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Find the best available name to describe a type.
        /// </summary>
        /// <remarks>
        /// Usually the best name will be <see cref="Type.FullName"/>, but
        /// sometimes that's null (see http://msdn.microsoft.com/en-us/library/system.type.fullname%28v=vs.110%29.aspx)
        /// in which case the method falls back to <see cref="MemberInfo.Name"/>.
        /// </remarks>
        /// <param name="type">the type to name</param>
        /// <returns>the best name</returns>
        public static string GetBestName(this Type type)
        {
            return type.AssemblyQualifiedName ?? type.FullName ?? type.Name;
        }

        public static Assembly GetAssembly(this Type type)
        {
            return type.GetTypeInfo().Assembly;
        }

        public static IEnumerable<Attribute> GetCustomAttributes(this MemberInfo memberInfo, bool inherit = true)
        {
            if (inherit)
            {
                return memberInfo.GetCustomAttributes(true).OfType<Attribute>().ToArray();
            }
            return memberInfo.GetCustomAttributes(false).OfType<Attribute>().ToArray();
        }
        public static IEnumerable<Attribute> GetCustomAttributes(this Type type, bool inherit = true)
        {
            TypeInfo typeInfo = type.GetTypeInfo();
            if (inherit)
            {
                return typeInfo.GetCustomAttributes(true).OfType<Attribute>().ToArray();
            }
            return typeInfo.GetCustomAttributes(false).OfType<Attribute>().ToArray();
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
        {
            return GetCustomAttributes(memberInfo, inherit).OfType<TAttribute>();
        }

        public static IEnumerable<TAttribute> GetCustomAttributes<TAttribute>(this Type type, bool inherit = true)
        {
            return GetCustomAttributes(type, inherit).OfType<TAttribute>();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this MemberInfo memberInfo, bool inherit = true)
        {
            return GetCustomAttributes(memberInfo, inherit).OfType<TAttribute>().FirstOrDefault();
        }

        public static TAttribute GetCustomAttribute<TAttribute>(this Type type, bool inherit = true)
        {
            return GetCustomAttributes(type, inherit).OfType<TAttribute>().FirstOrDefault();
        }
    }
}
