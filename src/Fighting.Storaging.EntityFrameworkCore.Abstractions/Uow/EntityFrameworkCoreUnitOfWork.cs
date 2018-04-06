using Fighting.DependencyInjection.Builder;
using Fighting.Extensions;
using Fighting.Storaging.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Fighting.Storaging.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Implements Unit of work for Entity Framework.
    /// </summary>
    [TransientDependency]
    public class EntityFrameworkCoreUnitOfWork : UnitOfWork
    {

        private readonly IDbContextResolver _dbContextResolver;

        private readonly ITransactionStrategy _transactionStrategy;
        protected IServiceProvider IocResolver { get; }

        protected IDictionary<string, DbContext> ActiveDbContexts { get; }

        /// <summary>
        /// Creates a new <see cref="EntityFrameworkCoreUnitOfWork"/>.
        /// </summary>
        public EntityFrameworkCoreUnitOfWork(
            IServiceProvider iocResolver,
            IConnectionStringResolver connectionStringResolver,
            IDbContextResolver dbContextResolver,
            IUnitOfWorkDefaultOptions defaultOptions,
            ITransactionStrategy transactionStrategy) : base(connectionStringResolver, defaultOptions)
        {
            IocResolver = iocResolver;
            _dbContextResolver = dbContextResolver;
            _transactionStrategy = transactionStrategy;

            ActiveDbContexts = new Dictionary<string, DbContext>();
        }

        protected override void BeginUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.InitOptions(Options);
            }
        }

        public override void SaveChanges()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                SaveChangesInDbContext(dbContext);
            }
        }

        public override async Task SaveChangesAsync()
        {
            foreach (var dbContext in GetAllActiveDbContexts())
            {
                await SaveChangesInDbContextAsync(dbContext);
            }
        }

        protected override void CompleteUow()
        {
            SaveChanges();
            CommitTransaction();
        }

        protected override async Task CompleteUowAsync()
        {
            await SaveChangesAsync();
            CommitTransaction();
        }

        private void CommitTransaction()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Commit();
            }
        }

        public IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContexts.Values.ToImmutableList();
        }

        public virtual TDbContext GetOrCreateDbContext<TDbContext>() where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);
            var connectionStringResolveArgs = new ConnectionStringResolveArgs
            {
                ["DbContextType"] = dbContextType
            };
            var connectionString = ResolveConnectionString(connectionStringResolveArgs);

            var dbContextKey = dbContextType.FullName + "#" + connectionString;

            if (!ActiveDbContexts.TryGetValue(dbContextKey, out DbContext dbContext))
            {
                if (Options.IsTransactional == true)
                {
                    dbContext = _transactionStrategy.CreateDbContext<TDbContext>(connectionString, _dbContextResolver);
                }
                else
                {
                    dbContext = _dbContextResolver.Resolve<TDbContext>(connectionString, null);
                }

                if (Options.Timeout.HasValue && dbContext.Database.IsRelational() && !dbContext.Database.GetCommandTimeout().HasValue)
                {
                    dbContext.Database.SetCommandTimeout(Options.Timeout.Value.TotalSeconds.To<int>());
                }

                ActiveDbContexts[dbContextKey] = dbContext;
            }

            return (TDbContext)dbContext;
        }

        protected override void DisposeUow()
        {
            if (Options.IsTransactional == true)
            {
                _transactionStrategy.Dispose(IocResolver);
            }
            else
            {
                foreach (var context in GetAllActiveDbContexts())
                {
                    Release(context);
                }
            }

            ActiveDbContexts.Clear();
        }

        protected virtual void SaveChangesInDbContext(DbContext dbContext)
        {
            dbContext.SaveChanges();
        }

        protected virtual async Task SaveChangesInDbContextAsync(DbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        protected virtual void Release(DbContext dbContext)
        {
            dbContext.Dispose();
            //IocResolver.Release(dbContext);
        }
    }
}