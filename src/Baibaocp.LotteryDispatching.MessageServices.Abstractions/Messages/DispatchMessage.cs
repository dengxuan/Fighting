namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public abstract class DispatchMessage : IDispatchMessage
    {

        public string LdpOrderId { get; }

        public string LdpMerchanerId { get; }

        internal DispatchMessage(string ldpOrderId, string ldpMerchanerId)
        {
            LdpOrderId = ldpOrderId;
            LdpMerchanerId = ldpMerchanerId;
        }
    }
}
