namespace Baibaocp.LotteryDispatcher.Core
{
    public abstract class Executer : IExecuter
    {
        internal Executer(string ldpVenderId)
        {
            LdpVenderId = ldpVenderId;
        }

        public string LdpVenderId { get; }
    }
}
