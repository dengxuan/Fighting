using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities
{
    [Table("BbcpTowns")]
    public class Town : Entity
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
        public virtual City City { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }
    }
}
