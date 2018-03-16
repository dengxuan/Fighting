using Baibaocp.LotteryNotifier.Abstractions;

namespace Baibaocp.LotteryNotifier.Internal
{
    public class Notifier<TNotice> : INotice<TNotice> where TNotice : class
    {
        public string VenderId { get;}

        public TNotice Notice { get; set; }

        public Notifier(string venderId) => VenderId = venderId;
    }
}
