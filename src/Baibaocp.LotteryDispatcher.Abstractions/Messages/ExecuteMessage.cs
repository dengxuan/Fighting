namespace Baibaocp.LotteryDispatcher.MessageServices.Messages
{
    public abstract class ExecuteMessage
    {
        internal ExecuteMessage(string ldpVenderId)
        {
            LdpVenderId = ldpVenderId;
        }

        public string LdpVenderId { get; }
    }
}
