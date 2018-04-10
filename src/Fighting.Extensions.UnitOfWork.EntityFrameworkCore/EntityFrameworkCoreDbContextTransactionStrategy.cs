using Fighting.DependencyInjection.Builder;
using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Abstractions;
using Fighting.Storaging.Data.Transactions.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Transactions;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore
{
    [TransientDependency]
    public class EntityFrameworkCoreDbContextTransactionStrategy : IEntityFrameworkCoreTransactionStrategy
    {
        protected UnitOfWorkOptions Options { get; private set; }

        protected IDictionary<Type, ActiveTransaction> ActiveTransactions { get; }

        public EntityFrameworkCoreDbContextTransactionStrategy()
        {
            ActiveTransactions = new Dictionary<Type, ActiveTransaction>();
        }

        public void InitOptions(UnitOfWorkOptions options)
        {
            Options = options;
        }

        public DbContext CreateDbContext<TDbContext>(IDbContextResolver dbContextResolver) where TDbContext : DbContext
        {
            DbContext dbContext;
            if (ActiveTransactions.TryGetValue(typeof(TDbContext), out ActiveTransaction activeTransaction) == false)
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(null);

                var dbtransaction = dbContext.Database.BeginTransaction((Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel());
                activeTransaction = new ActiveTransaction(dbtransaction, dbContext);
                ActiveTransactions[typeof(DbContext)] = activeTransaction;
            }
            else
            {
                dbContext = dbContextResolver.Resolve<TDbContext>(activeTransaction.DbContextTransaction.GetDbTransaction().Connection);

                if (dbContext.HasRelationalTransactionManager())
                {
                    dbContext.Database.UseTransaction(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    dbContext.Database.BeginTransaction();
                }

                activeTransaction.AttendedDbContexts.Add(dbContext);
            }

            return dbContext;
        }

        public void Commit()
        {
            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Commit();

                foreach (var dbContext in activeTransaction.AttendedDbContexts)
                {
                    if (dbContext.HasRelationalTransactionManager())
                    {
                        continue; //Relational databases use the shared transaction
                    }

                    dbContext.Database.CommitTransaction();
                }
            }
        }

        public void Dispose(IServiceProvider iocResolver)
        {
            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Dispose();

                foreach (var attendedDbContext in activeTransaction.AttendedDbContexts)
                {
                    //iocResolver.Release(attendedDbContext);
                    attendedDbContext.Dispose();
                }
                activeTransaction.StarterDbContext.Dispose();
                //iocResolver.Release(activeTransaction.StarterDbContext);
            }

            ActiveTransactions.Clear();
        }
    }
}
