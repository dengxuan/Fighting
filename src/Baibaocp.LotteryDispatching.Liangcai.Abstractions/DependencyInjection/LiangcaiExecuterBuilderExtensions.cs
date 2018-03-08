﻿using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Text;

namespace Baibaocp.LotteryDispatching.Liangcai.DependencyInjection
{
    public static class LiangcaiExecuterBuilderExtensions
    {
        public static LotteryDispatcherBuilder UseDispatcherServer<TExecuterHandler, TExecuter>(this LotteryDispatcherBuilder builder, Action<DispatcherOptions> setupOptions) where TExecuterHandler : IExecuteHandler<TExecuter> where TExecuter : IExecuter
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            builder.Services.AddSingleton(c => c.GetRequiredService<IOptions<DispatcherOptions>>().Value);
            builder.Services.Configure(setupOptions);
            return builder;
        }
    }
}