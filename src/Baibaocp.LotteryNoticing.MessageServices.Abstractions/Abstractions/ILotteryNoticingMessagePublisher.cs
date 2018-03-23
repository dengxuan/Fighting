using Baibaocp.LotteryNotifier.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.MessageServices.Abstractions
{
    public interface ILotteryNoticingMessagePublisher
    {
        Task PublishAsync<TContent>(string routingKey, NoticeMessage<TContent> message) where TContent : class, INoticeContent;
    }
}
