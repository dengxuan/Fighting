using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IExecuteHandle
    {

        Task<bool> HandleAsync();
    }
}
