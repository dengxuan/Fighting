using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{

    public sealed class SuccessHandle : IQueryingHandle
    {
        public string TicketedNumber { get; }

        public DateTime TicketedTime { get; set; }

        public string TicketedOdds { get; }

        /// <summary>
        /// 出票成功
        /// </summary>
        /// <param name="ticketedNumber">票号</param>
        /// <param name="ticketedTime">出票时间</param>
        /// <param name="ticketedOdds">出票赔率，仅竞彩，足彩，篮彩有</param>
        public SuccessHandle(string ticketedNumber, DateTime? ticketedTime, string ticketedOdds = default(string))
        {
            TicketedNumber = ticketedNumber;
            TicketedTime = ticketedTime ?? DateTime.Now;
            TicketedOdds = ticketedOdds;
        }
    }
}
