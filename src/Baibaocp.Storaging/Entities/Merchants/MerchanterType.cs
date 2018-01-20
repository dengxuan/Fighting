using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [Table("BbcpMerchanterTypes")]
    public class MerchanterType : Entity<short>
    {
        public string Name { get; set; }
    }
}
