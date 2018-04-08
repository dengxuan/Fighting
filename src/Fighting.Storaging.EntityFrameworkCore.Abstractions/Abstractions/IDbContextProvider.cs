using Microsoft.EntityFrameworkCore;
using System;

namespace Fighting.Storaging.EntityFrameworkCore.Abstractions
{
    public interface IDbContextProvider<out TDbContext> where TDbContext : DbContext
    {
        TDbContext GetDbContext();
    }
}
