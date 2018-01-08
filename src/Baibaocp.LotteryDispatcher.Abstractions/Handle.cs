using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatcher
{
    public enum Handle
    {
        /// <summary>
        /// 接票
        /// </summary>
        Accepted,

        /// <summary>
        /// 未接票
        /// </summary>
        Rejected,

        /// <summary>
        /// 出票
        /// </summary>
        Success,

        /// <summary>
        /// 未出票
        /// </summary>
        Failure,

        /// <summary>
        /// 中奖
        /// </summary>
        Winning,

        /// <summary>
        /// 未中奖
        /// </summary>
        Loseing,

        /// <summary>
        /// 等待状态
        /// </summary>
        Waiting,
    }
}
