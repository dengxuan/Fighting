using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryDispatching.MessageServices.Handles
{

    public sealed class AcceptedHandle : IExecuteHandle
    {

        public Task<bool> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
