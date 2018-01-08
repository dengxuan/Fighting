using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INoticeDispatcher
    {
        Task<bool> DispatchAsync<TNotice>(INotifier<TNotice> notifier) where TNotice : class;
    }
}
