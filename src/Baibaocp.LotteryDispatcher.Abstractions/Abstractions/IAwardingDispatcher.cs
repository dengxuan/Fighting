﻿using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.Abstractions
{
    public interface IAwardingDispatcher
    {
        Task<IQueryingHandle> DispatchAsync(QueryingExecuteMessage message);
    }
}
