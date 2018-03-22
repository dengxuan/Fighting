using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Baibaocp.LotteryOrdering.EntityFrameworkCore
{
    public class LotteryOrderingDbContext : StorageContext
    {

        public LotteryOrderingDbContext(StorageOptions storageOptions, DbContextOptions<LotteryOrderingDbContext> options) : base(storageOptions, options)
        {
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    base.OnConfiguring(optionsBuilder);
        //    optionsBuilder.UseMySql(StorageOptions.DefaultNameOrConnectionString);
        //}

        public DbSet<LotteryMerchanteOrder> LotteryVenderOrders { get; set; }
    }
}
