using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Abstractions
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(DbConnection existingConnection) where TDbContext : DbContext;
    }
}