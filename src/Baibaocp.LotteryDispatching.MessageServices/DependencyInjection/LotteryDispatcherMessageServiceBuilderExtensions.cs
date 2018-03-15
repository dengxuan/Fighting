﻿using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.MessageServices.DependencyInjection
{
    public static class LotteryDispatcherMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessageServices(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<IOrderingMessageService, OrderingMessageService>();
            messageServiceBuilder.Services.AddSingleton<IQueryingMessageService, QueryingMessageService>();
            return messageServiceBuilder;
        }
    }
}
