using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    [Table("BbcpLotteryTypes")]
    public class LotteryType : Entity<short>
    {
        public const int MaxTextLength = 6;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override short Id { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }


        [ForeignKey("LotteryTypeId")]
        public ICollection<LotteryCategory> LotteryCategories { get; set; }
    }
}
