using Fighting.Storaging.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities
{
    [Table("BbcpProvinces")]
    public class Province : Entity
    {

        /// <summary>
        /// Max length of the <see cref="Text"/> property.
        /// </summary>
        public const int MaxTextLength = 20;

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

        [Required]
        [StringLength(MaxTextLength)]
        public virtual string Text { get; set; }

        [ForeignKey("ProvinceId")]
        public ICollection<City> Cities { get; set; }
    }
}
