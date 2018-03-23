namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public abstract class DispatchMessage : IDispatchMessage
    {

        public long LdpOrderId { get; }

        public string LdpMerchanerId { get; }

        internal DispatchMessage(long ldpOrderId, string ldpMerchanerId)
        {
            LdpOrderId = ldpOrderId;
            LdpMerchanerId = ldpMerchanerId;
        }
    }
}
