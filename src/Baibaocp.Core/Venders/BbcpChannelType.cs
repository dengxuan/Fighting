using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Foundation.Baibaocp.Channels
{
    [Table("BbcpChannelTypes")]
    public class BbcpChannelType : Entity
    {
        public string Name { get; set; }
    }
}
