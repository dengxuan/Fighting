using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Generic;

namespace Fighting.Extensions.UnitOfWork.EntityFrameworkCore
{
    public class ActiveTransaction
    {
        public IDbContextTransaction DbContextTransaction { get; }

        public DbContext StarterDbContext { get; }

        public List<DbContext> AttendedDbContexts { get; }

        public ActiveTransaction(IDbContextTransaction dbContextTransaction, DbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<DbContext>();
        }
    }
}
