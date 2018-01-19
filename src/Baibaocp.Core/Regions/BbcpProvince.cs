using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Regions
{
    [Table("BbcpProvinces")]
    public class BbcpProvince : Entity<int>
    {

        /// <summary>
        /// Max length of the <see cref="Text"/> property.
        /// </summary>
        public const int MaxTextLength = 20;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Required]
        public override int Id { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public virtual string Text { get; set; }

        [ForeignKey("ProvinceId")]
        public ICollection<BbcpCity> Cities { get; set; }
    }
}
