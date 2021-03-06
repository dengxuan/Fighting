﻿using Fighting.DependencyInjection.Builder;
using Fighting.Reflection;
using Fighting.Storaging.Entities.Abstractions;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Fighting.Storaging.EntityFrameworkCore.Repositories;
using Fighting.Storaging.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Fighting.Storaging.EntityFrameworkCore.DependencyInjection
{
    public static class EntityFrameworkCoreStoragingBuilderExtensions
    {
        public static StorageBuilder AddEntityFrameworkCore<TDbContext>(this StorageBuilder storageBuilder, Action<DbContextOptionsBuilder> optionsAction) where TDbContext : StorageContext
        {
            storageBuilder.Services.TryAddTransient<IDbContextProvider<TDbContext>, DefaultDbContextProvider<TDbContext>>();
            storageBuilder.Services.AddDbContext<TDbContext>(optionsAction,ServiceLifetime.Transient, ServiceLifetime.Transient);
            RegisterRepository<TDbContext>(storageBuilder.Services);
            return storageBuilder;
        }

        internal static IEnumerable<EntityType> GetEntityTypeInfos(Type dbContextType)
        {
            return
                from property in dbContextType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                where
                    property.PropertyType.IsAssignableToGenericType(typeof(DbSet<>)) &&
                    property.PropertyType.GenericTypeArguments[0].IsAssignableToGenericType(typeof(IEntity<>))
                select new EntityType(property.PropertyType.GenericTypeArguments[0], property.DeclaringType);
        }

        internal static void RegisterRepository<TDbContext>(IServiceCollection services)
        {
            foreach (var entityTypeInfo in GetEntityTypeInfos(typeof(TDbContext)))
            {
                var primaryKeyType = EntityHelper.GetPrimaryKeyType(entityTypeInfo.Type);
                if (primaryKeyType == typeof(int))
                {
                    var genericRepositoryType = typeof(IRepository<>).MakeGenericType(entityTypeInfo.Type);
                    var implType = typeof(EntityFrameworkCoreRepository<,>).MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.Type);
                    services.AddTransient(genericRepositoryType, implType);
                }

                var genericRepositoryTypeWithPrimaryKey = typeof(IRepository<,>).MakeGenericType(entityTypeInfo.Type, primaryKeyType);
                var implTypeWithPrimaryKey = typeof(EntityFrameworkCoreRepository<,,>).MakeGenericType(entityTypeInfo.DeclaringType, entityTypeInfo.Type, primaryKeyType);
                services.AddTransient(genericRepositoryTypeWithPrimaryKey, implTypeWithPrimaryKey);
            }
        }
    }
}
