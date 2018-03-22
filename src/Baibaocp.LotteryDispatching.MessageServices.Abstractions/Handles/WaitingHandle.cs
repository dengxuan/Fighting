using Baibaocp.LotteryDispatching.MessageServices.Abstractions;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{

    public sealed class WaitingHandle : IQueryingHandle
    {
        public int DelayTime { get; }

        public WaitingHandle(int delayTime = 10)
        {
            DelayTime = delayTime;
        }
    }
}
