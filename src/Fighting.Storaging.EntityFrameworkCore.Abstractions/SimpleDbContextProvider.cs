using Fighting.Storaging.EntityFrameworkCore.Abstractions;

namespace Fighting.Storaging.EntityFrameworkCore
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : StorageContext
    {
        public TDbContext DbContext { get; }

        public SimpleDbContextProvider(TDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            return DbContext;
        }
    }
}
