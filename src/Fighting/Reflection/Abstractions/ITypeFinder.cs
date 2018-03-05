using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Abstractions
{
    public interface ITypeFinder
    {
        Type[] Find(Func<Type, bool> predicate);

        Type[] FindAll();
    }
}
