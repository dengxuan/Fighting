using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{
    public sealed class WinningHandle : IExecuteHandle
    {
        /// <summary>
        /// 奖金。单位：分
        /// </summary>
        public int BonusAmount { get; }

        /// <summary>
        /// 税后奖金。单位：分
        /// </summary>
        public int AftertaxBonusAmount { get; }

        public WinningHandle(int bonusAmount, int aftertaxBonusAmount)
        {
            BonusAmount = bonusAmount;
            AftertaxBonusAmount = aftertaxBonusAmount;
        }

        public Task<bool> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
