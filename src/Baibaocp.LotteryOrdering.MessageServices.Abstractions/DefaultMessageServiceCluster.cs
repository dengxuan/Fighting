using Baibaocp.LotteryOrdering.MessageServices.Abstractions;
using Orleans;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryOrdering.MessageServices
{
    public class DefaultMessageServiceCluster : IMessageServiceCluster
    {
        private readonly IGrainFactory _factory;

        public DefaultMessageServiceCluster(IGrainFactory factory)
        {
            _factory = factory;
        }

        public TMessageService GetMessageService<TMessageService>() where TMessageService : IMessageService
        {
            return _factory.GetGrain<TMessageService>(0);
        }
    }
}
