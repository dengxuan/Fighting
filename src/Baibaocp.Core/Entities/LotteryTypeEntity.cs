using Fighting.Storage.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Entities
{
    public class LotteryTypeEntity : Entity<int>
    {
        public const int MaxTextLength = 6;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public override int Id { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }


        [ForeignKey("LotteryTypeId")]
        public ICollection<LotteryCategoryEntity> LotteryCategories { get; set; }
    }
}
