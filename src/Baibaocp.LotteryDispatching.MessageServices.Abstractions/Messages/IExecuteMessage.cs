namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public interface IExecuteMessage
    {
        long LdpOrderId { get; }

        string LdpMerchanerId { get; }
    }
}
