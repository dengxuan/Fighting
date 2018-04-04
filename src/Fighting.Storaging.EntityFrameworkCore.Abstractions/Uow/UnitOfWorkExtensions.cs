using Fighting.Storaging.Uow.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Storaging.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Extension methods for UnitOfWork.
    /// </summary>
    public static class UnitOfWorkExtensions
    {
        /// <summary>
        /// Gets a DbContext as a part of active unit of work.
        /// This method can be called when current unit of work is an <see cref="EntityFrameworkCoreUnitOfWork"/>.
        /// </summary>
        /// <typeparam name="TDbContext">Type of the DbContext</typeparam>
        /// <param name="unitOfWork">Current (active) unit of work</param>
        public static TDbContext GetDbContext<TDbContext>(this IActiveUnitOfWork unitOfWork)
            where TDbContext : DbContext
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