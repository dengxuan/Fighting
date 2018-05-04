using Fighting.Extensions.UnitOfWork.DependencyInjection.Builder;
using Fighting.Extensions.UnitOfWork.EntityFrameworkCore.Abstractions;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore.DependencyInjection
{
    public static class EntityFramworkCoreUnitOfWorkBuilderExtensions
    {
        public static UnitOfWorkBuilder UseEntityFrameworkCore(this UnitOfWorkBuilder unitOfWorkBuilder, params Type[] dbContextTypes)
        {
            unitOfWorkBuilder.Services.AddSingleton<IDbContextResolver, DefaultDbContextResolver>();
            foreach (var item in dbContextTypes)
            {
                Type serviceType = typeof(IDbContextProvider<>).MakeGenericType(item);
                Type implementionType = typeof(UnitOfWorkDbContextProvider<>).MakeGenericType(item);
                unitOfWorkBuilder.Services.AddTransient(serviceType, implementionType);
            }
            return unitOfWorkBuilder;
        }
    }
}
