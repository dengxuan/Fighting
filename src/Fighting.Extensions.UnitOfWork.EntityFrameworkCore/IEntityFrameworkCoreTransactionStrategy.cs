using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore
{
    public interface IEntityFrameworkCoreTransactionStrategy
    {
        void InitOptions(UnitOfWorkOptions options);

        DbContext CreateDbContext<TDbContext>(IDbContextResolver dbContextResolver) where TDbContext : DbContext;

        void Commit();

        void Dispose(IServiceProvider iocResolver);
    }
}