using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Merchants
{

    [TransientDependency]
    public class MerchanterLotteryMappingManager
    {
        private readonly IRepository<MerchanterLotteryMapping, int> _lotteryMerchanterRepository;

        public virtual IQueryable<MerchanterLotteryMapping> MerchanterLotteryMappings { get { return _lotteryMerchanterRepository.GetAll(); } }

        public MerchanterLotteryMappingManager(IRepository<MerchanterLotteryMapping, int> channelLotteryRepository)
        {
            this._lotteryMerchanterRepository = channelLotteryRepository;
        }

        public async Task CreateChannelLottery(MerchanterLotteryMapping channelLottery)
        {
            await _lotteryMerchanterRepository.InsertAsync(channelLottery);
        }

        public IList<MerchanterLotteryMapping> FindLdpMerchanterId(string lvpMerchanterId, int lotteryId)
        {
            var merchanterLotteryMappings = MerchanterLotteryMappings.Where(predicate => predicate.LvpMerchanterId == lvpMerchanterId)
                                                                     .Where(predicate => predicate.LotteryId == lotteryId)
                                                                     .ToList();
            return merchanterLotteryMappings;
        }

        public async Task DeleteChannelLotteryMapping(MerchanterLotteryMapping channelLottery)
        {
            await _lotteryMerchanterRepository.DeleteAsync(channelLottery);
        }
    }
}
