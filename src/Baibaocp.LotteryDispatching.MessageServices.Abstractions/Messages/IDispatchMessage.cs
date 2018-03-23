namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public interface IDispatchMessage
    {
        long LdpOrderId { get; }

        string LdpMerchanerId { get; }
    }
}
