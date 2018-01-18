using Fighting.Storaging.Entities.Abstractions;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.LotteryOrdering.Core.Entities
{
    [Table("BbcpChannelAccountDetails")]
    public class LotteryVenderAccountDetailEntity : Entity<string>
    {
        [Column("ChannelId")]
        public string VenderId { get; set; }

        public string OrderId { get; set; }

        public int LotteryId { get; set; }

        public int Amount { get; set; }

        public int Status { get; set; }

        public DateTime CreationTime { get; set; }
    }
}
