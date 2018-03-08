using Fighting.MessageServices.Abstractions;

namespace Baibaocp.LotteryDispatching.MessageServices
{
    public interface IExecuteMessage : IMessage
    {
        string LdpVenderId { get; }
    }
}
