using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.Linghang.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Fighting.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Linghang.Dispatchers
{
    public class OrderingExecuteDispatcher : LinghangDispatcher<OrderingDispatchMessage>, IOrderingDispatcher
    {

        private readonly ILogger<OrderingExecuteDispatcher> _logger;

        public OrderingExecuteDispatcher(DispatcherConfiguration options, ILogger<OrderingExecuteDispatcher> logger) : base(options, logger, "101")
        {
            _logger = logger;
        }

        protected override string BuildRequest(OrderingDispatchMessage executer)
        {
            throw new NotImplementedException();
        }

        public Task<IOrderingHandle> DispatchAsync(OrderingDispatchMessage message)
        {
            throw new NotImplementedException();
        }
    }
}
