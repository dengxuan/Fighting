using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Fighting.Storaging.EntityFrameworkCore
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

        public TDbContext Resolve<TDbContext>(string connectionString, DbConnection existingConnection) where TDbContext : DbContext
        {
            var dbContextType = typeof(TDbContext);

            try
            {
                var objectFactory = _objectFactories.GetOrAdd(dbContextType, (type) =>
                {
                    var factory = ActivatorUtilities.CreateFactory(type, new Type[] { typeof(DbContextOptions<TDbContext>) });
                    return factory;
                });
                return (TDbContext)objectFactory.Invoke(_iocResolver, new object[] { CreateOptions<TDbContext>(connectionString, existingConnection) });
                //if (isAbstractDbContext)
                //{
                //    return (TDbContext)_iocResolver.Resolve(concreteType, new
                //    {
                //        options = CreateOptionsForType(concreteType, connectionString, existingConnection)
                //    });
                //}

                //return _iocResolver.Resolve<TDbContext>(new
                //{
                //    options = CreateOptions<TDbContext>(connectionString, existingConnection)
                //});
            }
            catch (Exception ex)
            {
                //var hasOptions = isAbstractDbContext ? HasOptions(concreteType) : HasOptions(dbContextType);
                //if (!hasOptions)
                //{
                //    throw new AggregateException($"The parameter name of {dbContextType.Name}'s constructor must be 'options'", ex);
                //}

                throw;
            }

            //bool HasOptions(Type contextType)
            //{
            //    return contextType.GetConstructors().Any(ctor =>
            //    {
            //        var parameters = ctor.GetParameters();
            //        return parameters.Length == 1 && parameters.FirstOrDefault()?.Name == "options";
            //    });
            //}
        }

        private object CreateOptionsForType(Type dbContextType, string connectionString, DbConnection existingConnection)
        {
            return CreateOptionsMethod.MakeGenericMethod(dbContextType).Invoke(this, new object[] { connectionString, existingConnection });
        }

        protected virtual DbContextOptions<TDbContext> CreateOptions<TDbContext>(string connectionString, DbConnection existingConnection) where TDbContext : DbContext
        {
            //if (_iocResolver.IsRegistered<IAbpDbContextConfigurer<TDbContext>>())
            //{
            //    var configuration = new AbpDbContextConfiguration<TDbContext>(connectionString, existingConnection);
            //    ReplaceServices(configuration);

            //    using (var configurer = _iocResolver.ResolveAsDisposable<IAbpDbContextConfigurer<TDbContext>>())
            //    {
            //        configurer.Object.Configure(configuration);
            //    }

            //    return configuration.DbContextOptions.Options;
            //}

            return _iocResolver.GetRequiredService<DbContextOptions<TDbContext>>();
        }

        //protected virtual void ReplaceServices<TDbContext>(AbpDbContextConfiguration<TDbContext> configuration) where TDbContext : DbContext
        //{
        //    configuration.DbContextOptions.ReplaceService<IEntityMaterializerSource, AbpEntityMaterializerSource>();
        //}
    }
}
