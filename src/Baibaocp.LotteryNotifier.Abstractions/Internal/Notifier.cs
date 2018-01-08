using Baibaocp.LotteryNotifier.Abstractions;

namespace Baibaocp.LotteryNotifier.Internal
{
    internal class Notifier<TNotice> : INotifier<TNotice> where TNotice : class
    {
        public string LvpVenderId { get; set; }

        public TNotice Notice { get; set; }

        public Notifier(string lvpVenderId) => LvpVenderId = lvpVenderId;
    }
}
