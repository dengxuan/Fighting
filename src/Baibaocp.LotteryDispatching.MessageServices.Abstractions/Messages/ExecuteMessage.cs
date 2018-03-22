namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public abstract class ExecuteMessage : IExecuteMessage
    {

        public long LdpOrderId { get; }

        public string LdpMerchanerId { get; }

        internal ExecuteMessage(long ldpOrderId, string ldpMerchanerId)
        {
            LdpOrderId = ldpOrderId;
            LdpMerchanerId = ldpMerchanerId;
        }
    }
}
