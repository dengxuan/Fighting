﻿using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Abstractions
{
    public interface IOrderingMessageService
    {
        Task PublishAsync(string merchanerId, string ldpOrderId, LvpOrderedMessage message);

        Task SubscribeAsync(string merchanerId, Func<OrderingExecuteMessage, Task<bool>> subscriber, CancellationToken stoppingToken);
    }
}