namespace Baibaocp.LotteryDispatching.MessageServices.Messages
{
    public interface IDispatchMessage
    {
        string LdpOrderId { get; }

        string LdpMerchanerId { get; }
    }
}
