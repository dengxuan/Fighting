﻿using Baibaocp.LotteryNotifier.MessageServices.Notices;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Baibaocp.LotteryNotifier.MessageServices.Abstractions
{
    public interface ITicketingNoticeMessageService
    {
        Task PublishAsync(LvpTicketedMessage message);

        Task SubscribeAsync(Func<Notice<Ticketed>, Task<bool>> subscriber, CancellationToken stoppingToken);
    }
}