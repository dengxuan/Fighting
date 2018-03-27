namespace Baibaocp.LotteryNotifier.MessageServices.Messages
{
    public class NoticeMessage<TContent> where TContent : class, INoticeContent
    {
        public string LdpOrderId { get; }

        public string LdpMerchanerId { get; }

        public TContent Content { get; }

        public NoticeMessage(string ldpOrderId, string ldpMerchanerId, TContent content)
        {
            LdpOrderId = ldpOrderId;
            LdpMerchanerId = ldpMerchanerId;
            Content = content;
        }
    }
}
