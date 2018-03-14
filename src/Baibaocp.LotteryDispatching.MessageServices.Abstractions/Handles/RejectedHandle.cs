﻿using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{
    public sealed class RejectedHandle : IExecuteHandle
    {

        public bool ReOrdering { get; }

        public Task<bool> HandleAsync()
        {
            return Task.FromResult(true);
        }
    }
}