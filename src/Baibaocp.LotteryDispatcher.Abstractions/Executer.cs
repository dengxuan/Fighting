using Baibaocp.LotteryDispatching.Abstractions;

namespace Baibaocp.LotteryDispatching
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
