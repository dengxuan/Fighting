using Baibaocp.LotteryNotifier.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Concurrent;

namespace Baibaocp.LotteryNotifier.Internal
{
    internal class NoticeHandlerFactory : INoticeHandlerFactory
    {
        private readonly IServiceProvider _iocResolver;
        private readonly LotteryNoticeOptions _options;

        private readonly ConcurrentDictionary<Type, ObjectFactory> _objectFactorys = new ConcurrentDictionary<Type, ObjectFactory>();

        public NoticeHandlerFactory(IServiceProvider iocResolver, LotteryNoticeOptions options)
        {
            _options = options;
            _iocResolver = iocResolver;
        }

        public INoticeHandler<TNotice> GetHandler<TNotice>(NoticeConfiguration configure) where TNotice : class
        {
            var handler = _objectFactorys.GetOrAdd(typeof(INotifier<TNotice>), (key) =>
            {
                //return _iocResolver.GetRequiredService<INoticeHandler<TNotice>>();
                if (_options.Mappings.TryGetValue(typeof(INoticeHandler<TNotice>), out Type handlerType))
                {
                    ObjectFactory objectFactory = ActivatorUtilities.CreateFactory(handlerType, new Type[] { typeof(NoticeConfiguration) });
                    return objectFactory;
                }
                throw new Exception();
            });
            return handler.Invoke(_iocResolver, new object[] { configure }) as INoticeHandler<TNotice>;
        }
    }
}
