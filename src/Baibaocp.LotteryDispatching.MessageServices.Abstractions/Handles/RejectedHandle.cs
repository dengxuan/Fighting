using Baibaocp.LotteryDispatching.MessageServices.Abstractions;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{
    public sealed class RejectedHandle : IOrderingHandle
    {
        public bool Reorder { get; }

        public RejectedHandle(bool reorder = false)
        {
            Reorder = reorder;
        }
    }
}
