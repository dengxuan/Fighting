using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Merchants
{
    public class MerchanterLotteryMappingManager
    {
        private readonly IRepository<MerchanterLotteryMapping, int> _lotteryMerchanterRepository;

        public virtual IQueryable<MerchanterLotteryMapping> ChannelLotteryMapping { get { return _lotteryMerchanterRepository.GetAll(); } }

        public MerchanterLotteryMappingManager(IRepository<MerchanterLotteryMapping, int> channelLotteryRepository)
        {
            this._lotteryMerchanterRepository = channelLotteryRepository;
        }

        public async Task CreateChannelLottery(MerchanterLotteryMapping channelLottery)
        {
            await _lotteryMerchanterRepository.InsertAsync(channelLottery);
        }

        public async Task DeleteChannelLotteryMapping(MerchanterLotteryMapping channelLottery)
        {
            await _lotteryMerchanterRepository.DeleteAsync(channelLottery);
        }
    }
}
