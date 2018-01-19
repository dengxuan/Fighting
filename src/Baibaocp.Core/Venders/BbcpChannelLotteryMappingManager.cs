using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.Core.Foundation.Baibaocp.Channels
{
    public class BbcpChannelLotteryMappingManager : IDomainService, IEventHandler<EntityChangedEventData<BbcpChannelLotteryMapping>>
    {
        protected IRepository<BbcpChannelLotteryMapping, int> ChannelLotteryRepository { get; set; }

        public virtual IQueryable<BbcpChannelLotteryMapping> ChannelLotteryMapping { get { return this.ChannelLotteryRepository.GetAll(); } }

        public BbcpChannelLotteryMappingManager(IRepository<BbcpChannelLotteryMapping, int> channelLotteryRepository, IUnitOfWorkManager unitOfWorkManager, IIocResolver iocResolver, ICacheManager cacheManager)
        {
            this.ChannelLotteryRepository = channelLotteryRepository;
        }
        public void HandleEvent(EntityChangedEventData<BbcpChannelLotteryMapping> eventData)
        {
            //throw new NotImplementedException();
        }
        public async Task CreateChannelLottery(BbcpChannelLotteryMapping channelLottery)
        {
            await ChannelLotteryRepository.InsertAsync(channelLottery);
        }
        public async Task DeleteChannelLotteryMapping(BbcpChannelLotteryMapping channelLottery)
        {
            await ChannelLotteryRepository.DeleteAsync(channelLottery);
        }
    }
}
