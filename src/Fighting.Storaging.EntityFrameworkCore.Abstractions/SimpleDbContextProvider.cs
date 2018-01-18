using Fighting.Storaging.EntityFrameworkCore.Abstractions;

namespace Fighting.Storaging.EntityFrameworkCore
{
    public sealed class SimpleDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : FightDbContext
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
