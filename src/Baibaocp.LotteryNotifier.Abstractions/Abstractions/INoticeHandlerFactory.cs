namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INoticeHandlerFactory
    {
        INoticeHandler<TNotice> GetHandler<TNotice>(NoticeConfiguration configure) where TNotice : class;
    }
}
