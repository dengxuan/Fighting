using Baibaocp.LotteryDispatching.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices;
using System;
using System.Collections.Concurrent;

namespace Baibaocp.LotteryDispatching
{
    public class LotteryDispatcherOptions
    {
        private readonly ConcurrentDictionary<(string ldpVenderId, Type executerType), Type> _ldpHandlerTypesMapping = new ConcurrentDictionary<(string ldpVenderId, Type executerType), Type>();

        public void AddHandler<TExecuteHandler, TExecuteMessage>(string ldpVenderId) where TExecuteHandler : IExecuteHandler<TExecuteMessage> where TExecuteMessage : IExecuteMessage
        {
            Console.WriteLine("Add Handler: {0} {1} {2}", ldpVenderId, typeof(TExecuteHandler), typeof(TExecuteMessage));
            _ldpHandlerTypesMapping.TryAdd((ldpVenderId, typeof(TExecuteMessage)), typeof(TExecuteHandler));
        }

        internal Type GetHandler<TExecuter>(string ldpVenderId)
        {
            Console.WriteLine("Get Handler: {0} {1}", ldpVenderId, typeof(TExecuter));
            _ldpHandlerTypesMapping.TryGetValue((ldpVenderId, typeof(TExecuter)), out Type value);
            return value;
        }
    }
}
