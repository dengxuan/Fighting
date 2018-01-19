using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Users
{
    [Table("BbcpIDCardTypes")]
    public class BbcpUserCredentialsType : Entity<int>
    {
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public override int Id { get; set; }

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
