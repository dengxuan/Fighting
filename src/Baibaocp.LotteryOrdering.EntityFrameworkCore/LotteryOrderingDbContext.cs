using Baibaocp.LotteryOrdering.Core.Entities;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Baibaocp.LotteryOrdering.EntityFrameworkCore
{
    public class LotteryOrderingDbContext : FightDbContext
    {
        private readonly StorageOptions _storageOptions;

        public LotteryOrderingDbContext(DbContextOptions options, StorageOptions storageOptions) : base(options)
        {
            _storageOptions = storageOptions;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(_storageOptions.DefaultNameOrConnectionString);
        }

        public DbSet<LotteryVenderEntity> LotteryVenders { get; set; }

        public DbSet<LotteryVenderAccountDetailEntity> LotteryVenderAccountDetails { get; set; }

        public DbSet<LotteryVenderOrderEntity> LotteryVenderOrders { get; set; }
    }
}
