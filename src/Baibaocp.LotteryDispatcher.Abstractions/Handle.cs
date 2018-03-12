using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatching
{
    public enum HandleTypes
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

    public interface IHandle
    {
        HandleTypes HandleType { get; }
    }

    public sealed class Accepted : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Accepted;
    }

    public sealed class Rejected : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Rejected;
    }

    public sealed class Success : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Success;

        public string TicketNumber { get; }

        public string TicketOdds { get; }

        public Success(string ticketNumber, string ticketOdds)
        {
            TicketNumber = ticketNumber;
            TicketOdds = ticketOdds;
        }
    }

    public sealed class Failure : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Failure;
    }

    public sealed class Winning : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Winning;

        /// <summary>
        /// 奖金。单位：分
        /// </summary>
        public int BonusAmount { get; }

        /// <summary>
        /// 税后奖金。单位：分
        /// </summary>
        public int AftertaxBonusAmount { get; }

        public Winning(int bonusAmount, int aftertaxBonusAmount)
        {
            BonusAmount = bonusAmount;
            AftertaxBonusAmount = aftertaxBonusAmount;
        }
    }

    public sealed class Loseing : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Loseing;
    }

    public sealed class Waiting : IHandle
    {
        public HandleTypes HandleType => HandleTypes.Waiting;
    }
}
