using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{

    public sealed class WaitingHandle : IExecuteHandle
    {
        public WaitTypes WaitType { get; }

        public Task<bool> HandleAsync()
        {
            return Task.FromResult(false);
        }
    }
}
