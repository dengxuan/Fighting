namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public interface IExecuteMessage
    {
        string LdpOrderId { get; }

        string LdpVenderId { get; }
    }
}
