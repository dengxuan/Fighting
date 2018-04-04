using System;

namespace Fighting.Storaging.EntityFrameworkCore.Abstractions
{
    public interface IDbContextTypeMatcher
    {
        void Populate(Type[] dbContextTypes);

        Type GetConcreteType(Type sourceDbContextType);
    }
}
