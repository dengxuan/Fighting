﻿using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Fighting.MessageServices.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Baibaocp.LotteryDispatching.MessageServices.Publisher.DependencyInjection
{
    public static class LotteryDispatcherMessageServiceBuilderExtensions
    {
        public static MessageServiceBuilder UseLotteryDispatchingMessagePublisher(this MessageServiceBuilder messageServiceBuilder)
        {
            messageServiceBuilder.Services.AddSingleton<IOrderingDispatcherMessagePublisher, OrderingMessagePublisher>();
            messageServiceBuilder.Services.AddSingleton<IQueryingDispatcherMessagePublisher, QueryingMessagePublisher>();
            return messageServiceBuilder;
        }
    }
}