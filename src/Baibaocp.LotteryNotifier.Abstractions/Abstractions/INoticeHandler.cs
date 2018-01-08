using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.Abstractions
{
    public interface INoticeHandler<TNotice> where TNotice : class
    {
        Task<bool> HandleAsync(TNotice notice);
    }
}
