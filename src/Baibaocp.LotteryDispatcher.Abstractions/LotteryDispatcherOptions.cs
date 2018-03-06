using Baibaocp.LotteryDispatcher.Abstractions;
using System;
using System.Collections.Concurrent;

namespace Baibaocp.LotteryDispatcher
{
    public class LotteryDispatcherOptions
    {
        private readonly ConcurrentDictionary<(string ldpVenderId, Type executerType), Type> _ldpHandlerTypesMapping = new ConcurrentDictionary<(string ldpVenderId, Type executerType), Type>();

        public void AddHandler<THandler, TExecuter>(string ldpVenderId) where THandler : IExecuteHandler<TExecuter> where TExecuter : IExecuter
        {
            Console.WriteLine("Add Handler: {0} {1} {2}", ldpVenderId, typeof(THandler), typeof(TExecuter));
            _ldpHandlerTypesMapping.TryAdd((ldpVenderId, typeof(TExecuter)), typeof(THandler));
        }

        internal Type GetHandler<TExecuter>(string ldpVenderId)
        {
            Console.WriteLine("Get Handler: {0} {1}", ldpVenderId, typeof(TExecuter));
            _ldpHandlerTypesMapping.TryGetValue((ldpVenderId, typeof(TExecuter)), out Type value);
            return value;
        }
    }
}
