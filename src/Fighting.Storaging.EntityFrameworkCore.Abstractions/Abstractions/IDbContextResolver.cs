using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace Fighting.Storaging.EntityFrameworkCore
{
    public interface IDbContextResolver
    {
        TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection) where TDbContext : DbContext;
    }
}