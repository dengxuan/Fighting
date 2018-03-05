using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [Table("BbcpMerchants")]
    public class Merchanter : Entity
    {
        /// <summary>
        /// 渠道名称<see cref="Name"/>的最大长度
        /// </summary>
        public const int MaxChannelNameLength = 20;

        /// <summary>
        /// 密钥<see cref="SecretKey"/>的最大长度
        /// </summary>  
        public const int MaxSecretKeyLength = 24;

        /// <summary>
        /// 渠道code
        /// </summary>
        [Required]
        [StringLength(MaxChannelNameLength)]
        public override int Id { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        [Required]
        [StringLength(MaxChannelNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 渠道类型
        /// </summary>
        [Required]
        public short MerchanterTypeId { get; set; }


        [ForeignKey("MerchanterTypeId")]
        public virtual MerchanterType MerchanterType { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        [Required]
        [StringLength(MaxSecretKeyLength)]
        public string SecretKey { get; set; }

        /// <summary>
        /// 预存款余额
        /// </summary>
        [Column("RestPreMoney")]
        public int Balance { get; set; }

        /// <summary>
        /// 总出票金额
        /// </summary>
        [Column("OutTicketMoney")]
        public int TotalTicketedAmount { get; set; }

        /// <summary>
        /// 总返奖金额
        /// </summary>
        [Column("RewardMoney")]
        public int TotalAwardedAmount { get; set; }

        //public string TenantId { get; set; }
    }
}
