using Fighting.Storaging.Uow;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Storaging.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// </summary>
    public static class UnitOfWorkExtensions
    {

        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork) where TDbContext : DbContext
        {
            if (unitOfWork == null)
            {
                throw new ArgumentNullException("unitOfWork");
            }

            if (!(unitOfWork is EntityFrameworkCoreUnitOfWork))
            {
                throw new ArgumentException("unitOfWork is not type of " + typeof(EntityFrameworkCoreUnitOfWork).FullName, "unitOfWork");
            }

            return (unitOfWork as EntityFrameworkCoreUnitOfWork).GetOrCreateDbContext<TDbContext>();
        }
    }
}