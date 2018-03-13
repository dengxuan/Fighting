using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{
    public interface IExecuteHandle
    {

        Task<bool> HandleAsync();
    }
}
