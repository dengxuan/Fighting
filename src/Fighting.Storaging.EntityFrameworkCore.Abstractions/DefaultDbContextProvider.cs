using Fighting.Storaging.EntityFrameworkCore.Abstractions;

namespace Fighting.Storaging.EntityFrameworkCore
{
    public class DefaultDbContextProvider<TDbContext> : IDbContextProvider<TDbContext> where TDbContext : StorageContext
    {
        private readonly TDbContext _dbContext;

        public DefaultDbContextProvider(TDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TDbContext GetDbContext()
        {
            return _dbContext;
        }
    }
}
