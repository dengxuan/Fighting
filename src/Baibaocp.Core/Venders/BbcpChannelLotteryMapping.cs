using Abp.Domain.Entities.Auditing;
using Baibaocp.Core.Lotteries;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.Core.Foundation.Baibaocp.Channels
{
    public class BbcpChannelLotteryMapping:FullAuditedEntity
    {
        /// <summary>
        /// 渠道编号
        /// </summary
        public string ChannelId { get; set; }

        /// <summary>
        /// 渠道 <see cref="BbcpVender"/>
        /// </summary>
        [ForeignKey("ChannelId")]
        public virtual BbcpVender Channel { get; set; }

        /// <summary>
        /// 彩种编号
        /// </summary>
        [Required]
        public int LotteryId { get; set; }

        /// <summary>
        /// 彩种 <see cref="BbcpLottery"/>
        /// </summary>
        [ForeignKey("LotteryId")]
        public virtual BbcpLottery Lottery { get; set; }

        /// <summary>
        /// 佣金比例
        /// </summary>
        public decimal CommissionRate { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string NoticeAddress { get; set; }
    }
}
