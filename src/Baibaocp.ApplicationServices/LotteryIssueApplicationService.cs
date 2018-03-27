using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class LotteryIssueApplicationService : ApplicationService, ILotteryPhaseApplicationService
    {
        private readonly IRepository<LotteryPhase> _lotteryIssueRepository;

        public LotteryIssueApplicationService(IRepository<LotteryPhase> lotteryIssueRepository, ICacheManager cacheManager) : base(cacheManager)
        {
            _lotteryIssueRepository = lotteryIssueRepository;
        }

        public Task<LotteryPhase> FindLotteryPhase(int lotteryId, int issueNumber)
        {
            ICache cache = CacheManager.GetCache("LotteryPhases");
            return cache.GetAsync($"{lotteryId}-{issueNumber}", (key) =>
            {
                return _lotteryIssueRepository.FirstOrDefault(predicate => predicate.IssueNumber == issueNumber);
            });
        }

        public Task<List<LotteryPhase>> FindLotteryPhases(int lotteryId, int skip = 0, int limit = 10)
        {
            return Task.FromResult(_lotteryIssueRepository.GetAll().Where(predicate => predicate.LotteryId == lotteryId).Skip(skip).Take(limit).ToList());
        }

        public Task<List<LotteryPhase>> NextLotteryPhases(int lotteryId, int issueNumber, int limit = 10)
        {
            var lotteryPhases = _lotteryIssueRepository.GetAll()
               .Where(predicate => predicate.LotteryId == lotteryId)
               .Where(predicate => predicate.IssueNumber > issueNumber)
               .OrderBy(predicate => predicate.IssueNumber)
               .Take(limit)
               .ToList();
            return Task.FromResult(lotteryPhases);
        }

        public Task UpdateDrawNumber(int id, string drawNumber)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLotteryPhase(LotteryPhase lotteryPhase)
        {
            throw new NotImplementedException();
        }
    }
}
