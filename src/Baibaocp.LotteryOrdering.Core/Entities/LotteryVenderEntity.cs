using Fighting.Storage.Abstractions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.LotteryOrdering.Core.Entities
{
    [Table("BbcpChannels")]
    public class LotteryVenderEntity : Entity<string>
    {
        [Column("ChannelName")]
        public string Name { get; set; }

        public string SecretKey { get; set; }

        [Column("RestPreMoney")]
        public decimal Balance { get; set; }

        [Column("OutTicketMoney")]
        public decimal TicketAmount { get; set; }

        [Column("RewardMoney")]
        public decimal RewardAmount { get; set; }

        public int ChannelTypeId { get; set; }
        
        public string NoticeAddress { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
