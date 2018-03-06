using Baibaocp.LotteryDispatcher.Abstractions;

namespace Baibaocp.LotteryDispatcher
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
