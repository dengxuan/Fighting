namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INotifier<TNotice> where TNotice : class
    {
        string LvpVenderId { get; set; }

        TNotice Notice { get; set; }
    }
}
