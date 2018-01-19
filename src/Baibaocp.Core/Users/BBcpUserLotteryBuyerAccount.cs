using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Core.Users
{
    /// <summary>
    /// 百宝彩彩民账户
    /// </summary>
    [Table("BbcpUserLotteryBuyerAccounts")]
    public class BbcpUserLotteryBuyerAccount : Entity<long>
    {
        /// <summary>
        /// 账户类型编号
        /// </summary>
        [Required]
        public short AccountTypeId { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [ForeignKey("AccountTypeId")]
        public virtual BbcpLotteryBuyerAccountType AccountType { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
