namespace Baibaocp.LotteryNotifier.MessageServices
{
    public class Notice<TContent> where TContent : class
    {
        public string VenderId { get;}

        public TContent Content { get; set; }

        public Notice(string venderId) => VenderId = venderId;
    }
}
