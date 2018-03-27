using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class LotterySportsMatchApplicationService : ApplicationService, ILotterySportsMatchApplicationService
    {
        private readonly IRepository<LotterySportsMatch, long> _lotterySportsMatchRepository;

        public LotterySportsMatchApplicationService(ICacheManager cacheManager, IRepository<LotterySportsMatch, long> lotterySportsMatchRepository) : base(cacheManager)
        {
            _lotterySportsMatchRepository = lotterySportsMatchRepository;
        }

        public async Task CreateMatchAsync(LotterySportsMatch match)
        {
            await _lotterySportsMatchRepository.InsertAsync(match);
        }

        public async Task<LotterySportsMatch> FindMatchAsync(long matchId)
        {
            ICache cacher = CacheManager.GetCache(nameof(LotterySportsMatchApplicationService));
            return await cacher.GetAsync($"{matchId}", (key) =>
            {
                return _lotterySportsMatchRepository.FirstOrDefault(matchId);
            });
        }

        public async Task UpdateMatchAsync(LotterySportsMatch match)
        {
            await _lotterySportsMatchRepository.UpdateAsync(match);
        }
    }
}
