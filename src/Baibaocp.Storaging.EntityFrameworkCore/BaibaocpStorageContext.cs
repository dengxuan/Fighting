using Baibaocp.Storaging.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using Baibaocp.Storaging.Entities.Merchants;
using Baibaocp.Storaging.Entities.Users;
using Fighting.Storaging;
using Fighting.Storaging.EntityFrameworkCore.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Baibaocp.Storaging.EntityFrameworkCore
{
    public class BaibaocpStorageContext : StorageContext
    {
        public BaibaocpStorageContext(StorageConfiguration storageOptions, DbContextOptions<BaibaocpStorageContext> options) : base(storageOptions, options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 省
        /// </summary>
        public virtual DbSet<Province> Provinces { get; set; }

        /// <summary>
        /// 市
        /// </summary>
        public virtual DbSet<City> Cities { get; set; }

        /// <summary>
        /// 县
        /// </summary>
        public virtual DbSet<Town> Towns { get; set; }

        /// <summary>
        /// 证件类别
        /// </summary>
        public virtual DbSet<CredentialsType> CredentialsTypes { get; set; }

        /// <summary>
        /// 彩种类别
        /// </summary>
        public virtual DbSet<LotteryCategory> BbcpLotteryCategoties { get; set; }

        /// <summary>
        /// 彩种类型
        /// </summary>
        public virtual DbSet<LotteryType> BbcpLotteryTypes { get; set; }

        /// <summary>
        /// 彩种
        /// </summary>
        public DbSet<Lottery> BbcpLotteries { get; set; }

        /// <summary>
        /// 玩法
        /// </summary>
        public virtual DbSet<LotteryPlay> BbcpLotteryPlays { get; set; }

        /// <summary>
        /// 彩种玩法映射
        /// </summary>
        public virtual DbSet<LotteryPlayMapping> BbcpLotteryPlayMappings { get; set; }

        /// <summary>
        /// 竞技彩对阵
        /// </summary>
        public virtual DbSet<LotterySportsMatch> LotterySportsMatches { get; set; }

        /// <summary>
        /// 彩种期号
        /// </summary>
        public DbSet<LotteryPhase> BbcpLotteryPahses { get; set; }

        /// <summary>
        /// 期号奖金明细
        /// </summary>
        public virtual DbSet<LotteryPhaseBonus> BbcpLotteryPhaseBonuses { get; set; }

        /// <summary>
        /// 商户
        /// </summary>
        public virtual DbSet<Merchanter> Merchanters { get; set; }

        /// <summary>
        /// 商户类型
        /// </summary>
        public virtual DbSet<MerchanterType> MerchanterTypes { get; set; }

        /// <summary>
        /// 商户彩种开通的彩种
        /// </summary>
        public virtual DbSet<MerchanterLotteryMapping> LotteryMerchanterMappings { get; set; }

        /// <summary>
        /// 商户账户交易记录
        /// </summary>
        public virtual DbSet<MerchanterAccountLogging> MerchanterAccountLoggings { get; set; }

        /// <summary>
        /// 彩民
        /// </summary>
        public virtual DbSet<UserLotteryBuyer> UserLotteryBuyers { get; set; }

        /// <summary>
        /// 彩民账户
        /// </summary>
        public virtual DbSet<UserLotteryBuyerAccount> UserLotteryBuyerAccounts { get; set; }

        /// <summary>
        /// 彩民账户类别
        /// </summary>
        public virtual DbSet<UserLotteryBuyerAccountType> UserLotteryBuyerAccountTypes { get; set; }

        /// <summary>
        /// 彩民订单
        /// </summary>
        public virtual DbSet<UserLotteryBuyerOrder> UserOrders { get; set; }
    }
}
