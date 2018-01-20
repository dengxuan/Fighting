using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities
{
    [Table("BbcpCredentialsTypes")]
    public class CredentialsType : Entity<short>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override short Id { get; set; }

        /// <summary>
        /// 证件名称 <see cref="Text"/> 的最大长度
        /// </summary>
        public const int MaxTextLength = 10;

        /// <summary>
        /// 证件名称
        /// </summary>
        [Required]
        [StringLength(MaxTextLength)]
        public string Text { get; set; }

    }
}
