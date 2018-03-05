using Fighting.Storaging.Entities.Abstractions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Baibaocp.Storaging.Entities.Users
{
    /// <summary>
    /// 百宝彩彩民账户
    /// </summary>
    [Table("BbcpUserLotteryBuyerAccounts")]
    public class UserLotteryBuyerAccount : Entity
    {
        /// <summary>
        /// 账户类型编号
        /// </summary>
        [Required]
        public short LotteryBuyerAccountTypeId { get; set; }

        /// <summary>
        /// 账户类型
        /// </summary>
        [ForeignKey("LotteryBuyerAccountTypeId")]
        public virtual UserLotteryBuyerAccountType LotteryBuyerAccountType { get; set; }

        /// <summary>
        /// 账户余额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
