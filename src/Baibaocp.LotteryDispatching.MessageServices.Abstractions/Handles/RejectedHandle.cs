using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{
    public sealed class RejectedHandle : IExecuteHandle
    {

        public bool ReOrdering { get; }

        public Task<bool> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
