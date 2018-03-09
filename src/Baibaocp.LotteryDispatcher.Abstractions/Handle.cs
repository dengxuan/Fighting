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

    internal class Handle : IHandle
    {
        public HandleTypes HandleType { get; }

        internal Handle(HandleTypes handleType) => HandleType = handleType;
    }

    public interface IHandleAwarded : IHandle
    {
        /// <summary>
        /// 奖金。单位：分
        /// </summary>
        int BonusAmount { get; }

        /// <summary>
        /// 税后奖金。单位：分
        /// </summary>
        int AftertaxBonusAmount { get; }
    }

    internal class HandleAwarded : Handle, IHandleAwarded
    {
        public int BonusAmount { get; }

        public int AftertaxBonusAmount { get; }

        internal HandleAwarded(HandleTypes handleType) : base(handleType)
        {
        }
    }

    public interface IHandleTicketed : IHandle
    {
        string TicketNumber { get; }

        string TicketOdds { get; }
    }

    internal class HandleTicketed : Handle, IHandleTicketed
    {

        public string TicketNumber { get; }

        public string TicketOdds { get; }

        internal HandleTicketed(HandleTypes handleType) : this(handleType, string.Empty, string.Empty)
        {

        }

        internal HandleTicketed(HandleTypes handleType, string ticketNumber, string ticketOdds) : base(handleType)
        {
            TicketNumber = ticketNumber;
            TicketOdds = ticketOdds;
        }
    }

    public class HandleHelper
    {
        /// <summary>
        /// 接票
        /// </summary>
        /// <returns></returns>
        public static IHandle Accept()
        {
            return new Handle(HandleTypes.Accepted);
        }

        /// <summary>
        /// 未接票
        /// </summary>
        /// <returns></returns>
        public static IHandle Reject()
        {
            return new Handle(HandleTypes.Rejected);
        }

        /// <summary>
        /// 等待状态
        /// </summary>
        /// <returns></returns>
        public static IHandle Waiting()
        {
            return new Handle(HandleTypes.Waiting);
        }

        /// <summary>
        /// 出票成功
        /// </summary>
        /// <returns></returns>
        public static IHandle Success()
        {
            return new HandleTicketed(HandleTypes.Success);
        }

        /// <summary>
        /// 出票失败
        /// </summary>
        /// <returns></returns>
        public static IHandle Failure()
        {
            return new HandleTicketed(HandleTypes.Failure);
        }

        /// <summary>
        /// 中奖
        /// </summary>
        /// <returns></returns>
        public static IHandle Winning()
        {
            return new HandleAwarded(HandleTypes.Winning);
        }

        /// <summary>
        /// 未中奖
        /// </summary>
        /// <returns></returns>
        public static IHandle Loseing()
        {
            return new HandleAwarded(HandleTypes.Loseing);
        }
    }
}
