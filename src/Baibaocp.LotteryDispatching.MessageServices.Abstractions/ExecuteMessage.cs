namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public abstract class ExecuteMessage : IExecuteMessage
    {
        internal ExecuteMessage(string ldpVenderId)
        {
            LdpVenderId = ldpVenderId;
        }

        public string LdpVenderId { get; }
    }
}
