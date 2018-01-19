using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Foundation.Baibaocp.Channels
{
    [Table("BbcpChannels")]
    public class BbcpVender : Entity<string>
    {
        /// <summary>
        /// 渠道名称<see cref="ChannelName"/>的最大长度
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
        public override string Id { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        [Required]
        [StringLength(MaxChannelNameLength)]
        public string ChannelName { get; set; }

        /// <summary>
        /// 渠道类型
        /// </summary>
        [Required]
        public int ChannelTypeId { get; set; }


        [ForeignKey("ChannelTypeId")]
        public virtual BbcpChannelType ChannelType { get; set; }

        /// <summary>
        /// 密钥
        /// </summary>
        [Required]
        [StringLength(MaxSecretKeyLength)]
        public string SecretKey { get; set; }

        /// <summary>
        /// 预存款余额
        /// </summary>
        public decimal RestPreMoney { get; set; }

        /// <summary>
        /// 出票金额
        /// </summary>
        public decimal OutTicketMoney { get; set; }

        /// <summary>
        /// 返奖金额
        /// </summary>
        public decimal RewardMoney { get; set; }

        //public string TenantId { get; set; }
    }
}
