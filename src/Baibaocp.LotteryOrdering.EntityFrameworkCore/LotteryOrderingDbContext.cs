using Baibaocp.LotteryOrdering.Core.Entities;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Baibaocp.LotteryOrdering.EntityFrameworkCore
{
    public class LotteryOrderingDbContext : StorageContext
    {

        public LotteryOrderingDbContext(StorageOptions storageOptions, DbContextOptions options) : base(storageOptions, options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(StorageOptions.DefaultNameOrConnectionString);
        }

        public DbSet<LotteryVenderEntity> LotteryVenders { get; set; }

        public DbSet<LotteryVenderAccountDetailEntity> LotteryVenderAccountDetails { get; set; }

        public DbSet<LotteryVenderOrderEntity> LotteryVenderOrders { get; set; }
    }
}
