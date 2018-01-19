using Baibaocp.Core.Lotteries;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Baibaocp.EntityFrameworkCore
{
    public class BaibaocpStorageContext : StorageContext
    {
        public BaibaocpStorageContext(StorageOptions storageOptions) : base(storageOptions)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(StorageOptions.DefaultNameOrConnectionString);
        }

        /// <summary>
        /// 彩种类别
        /// </summary>
        public virtual DbSet<BbcpLotteryCategory> BbcpLotteryCategoties { get; set; }

        /// <summary>
        /// 彩种类型
        /// </summary>
        public virtual DbSet<BbcpLotteryType> BbcpLotteryTypes { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        public DbSet<BbcpLottery> BbcpLotteries { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        public virtual DbSet<BbcpLotteryPlay> BbcpLotteryPlaies { get; set; }

        /// <summary>
        /// 彩种玩法映射
        /// </summary>
        public virtual DbSet<BbcpLotteryPlayMapping> BbcpLotteryPlayMappings { get; set; }

        /// <summary>
        /// 彩种期号
        /// </summary>
        public DbSet<BbcpLotteryIssue> BbcpLotteryIssues { get; set; }

        /// <summary>
        /// 期号奖金明细
        /// </summary>
        public virtual DbSet<BbcpLotteryIssueBonus> BbcpLotteryIssueBonuses { get; set; }
    }
}
