using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Lotteries
{
    [Table("BbcpLotteryTypes")]
    public class BbcpLotteryType : Entity<int>
    {
        public const int MaxTextLength = 6;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }


        [ForeignKey("LotteryTypeId")]
        public ICollection<BbcpLotteryCategory> LotteryCategories { get; set; }
    }
}
