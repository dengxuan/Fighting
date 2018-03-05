﻿using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.MessageServices.LotteryDispatcher.Abstractions
{
    public interface ILotteryOrderingMessageServiceManager
    {
        Task PublishAsync(LvpOrderedMessage lvpOrderedMessage);

        Task SubscribeAsync(CancellationToken stoppingToken);
    }
}
