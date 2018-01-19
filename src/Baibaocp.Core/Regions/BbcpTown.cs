using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Regions
{
    [Table("BbcpTowns")]
    public class BbcpTown : Entity<int>
    {

        /// <summary>
        /// Max length of the <see cref="Text"/> property.
        /// </summary>
        public const int MaxTextLength = 20;
        [Required]

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required] 
        public virtual int CityId { get; set; }

        [Required]
        [ForeignKey("CityId")]
        public virtual BbcpCity City { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }
    }
}
