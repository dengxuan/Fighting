using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Linghang.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Linghang.Dispatchers
{
    public class QueryingExecuteDispatcher : LinghangDispatcher<QueryingDispatchMessage>, IQueryingDispatcher
    {
        public QueryingExecuteDispatcher(DispatcherConfiguration options, ILogger<LinghangDispatcher<QueryingDispatchMessage>> logger, string command) : base(options, logger, command)
        {
        }

        public Task<IQueryingHandle> DispatchAsync(QueryingDispatchMessage message)
        {
            throw new System.NotImplementedException();
        }

        protected override string BuildRequest(QueryingDispatchMessage message)
        {
            throw new System.NotImplementedException();
        }
    }
}
