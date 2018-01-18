using Fighting.Reflection.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fighting.Reflection
{
    public class AbpAssemblyFinder : IAssemblyFinder
    {

        public List<Assembly> GetAllAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies().ToList();
        }
    }
}
