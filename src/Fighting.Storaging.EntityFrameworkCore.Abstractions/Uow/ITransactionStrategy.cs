using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Fighting.Storaging.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Storaging.EntityFrameworkCore.Uow
{
    public interface ITransactionStrategy
    {
        void InitOptions(UnitOfWorkOptions options);

        DbContext CreateDbContext<TDbContext>(string connectionString, IDbContextResolver dbContextResolver) where TDbContext : DbContext;

        void Commit();

        void Dispose(IServiceProvider iocResolver);
    }
}
