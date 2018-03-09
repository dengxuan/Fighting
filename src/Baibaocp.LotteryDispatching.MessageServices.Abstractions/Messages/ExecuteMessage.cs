namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public abstract class ExecuteMessage : IExecuteMessage
    {

        public string LdpOrderId { get; }

        public string LdpVenderId { get; }

        internal ExecuteMessage(string ldpOrderId, string ldpVenderId)
        {
            LdpVenderId = ldpVenderId;
            LdpOrderId = ldpOrderId;
        }
    }
}
