﻿using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{
    public sealed class LoseingHandle : IExecuteHandle
    {
        public Task<bool> HandleAsync()
        {
            return Task.FromResult(true);
        }
    }
}
