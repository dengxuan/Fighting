using Microsoft.EntityFrameworkCore;

namespace Fighting.Storaging.EntityFrameworkCore.Abstractions
{
    public interface IDbContextProvider<out TDbContext> where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
