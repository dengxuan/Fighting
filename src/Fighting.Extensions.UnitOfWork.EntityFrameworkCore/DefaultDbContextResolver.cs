using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;
using System.Data.Common;
using System.Reflection;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore
{
    public class DefaultDbContextResolver : IDbContextResolver
    {
        private static readonly MethodInfo CreateOptionsMethod = typeof(DefaultDbContextResolver).GetMethod("CreateOptions", BindingFlags.NonPublic | BindingFlags.Instance);

        private readonly IServiceProvider _iocResolver;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _objectFactories = new ConcurrentDictionary<Type, ObjectFactory>();

        public DefaultDbContextResolver(IServiceProvider iocResolver)
        {
            _iocResolver = iocResolver;
        }

        public TDbContext Resolve<TDbContext>(DbConnection existingConnection) where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);

            try
            {
                var objectFactory = _objectFactories.GetOrAdd(dbContextType, (type) =>
                {
                    var factory = ActivatorUtilities.CreateFactory(type, new Type[] { typeof(DbContextOptions<TDbContext>) });
                    return factory;
                });
                return (TDbContext)objectFactory.Invoke(_iocResolver, new object[] { CreateOptions<TDbContext>(existingConnection) });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private object CreateOptionsForType(Type dbContextType, string connectionString, DbConnection existingConnection)
        {
            return CreateOptionsMethod.MakeGenericMethod(dbContextType).Invoke(this, new object[] { connectionString, existingConnection });
        }

        protected virtual DbContextOptions<TDbContext> CreateOptions<TDbContext>(DbConnection existingConnection) where TDbContext : DbContext
        {
            DbContextOptions<TDbContext> options = _iocResolver.GetRequiredService<DbContextOptions<TDbContext>>();
            return options;
        }
    }
}
