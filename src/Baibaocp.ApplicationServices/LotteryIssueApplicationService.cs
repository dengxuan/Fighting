using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.Core.Entities;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Storaging.Repositories.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices
{
    public class LotteryIssueApplicationService : ApplicationService, ILotteryIssueApplicationServices
    {
        private readonly IRepository<LotteryIssue, int> _lotteryIssueRepository;

        public LotteryIssueApplicationService(IRepository<LotteryIssue, int> lotteryIssueRepository, ICacheManager cacheManager) : base(cacheManager)
        {
            _lotteryIssueRepository = lotteryIssueRepository;
        }

        public Task<LotteryIssue> FindLotteryIssue(int issueNumber)
        {
            return _lotteryIssueRepository.FirstOrDefaultAsync(predicate => predicate.IssueNumber == issueNumber);
        }

        public Task<List<LotteryIssue>> FindLotteryIssues(int lotteryId, int skip = 0, int limit = 10)
        {
            return Task.FromResult(_lotteryIssueRepository.GetAll().Where(predicate => predicate.LotteryId == lotteryId).Skip(skip).Take(limit).ToList());
        }

        public Task<LotteryIssue> NextLotteryIssueInformation(int lotteryId, int issueNumber)
        {
            var lotteryIssue = _lotteryIssueRepository.GetAll()
               .Where(predicate => predicate.LotteryId == lotteryId)
               .Where(predicate => predicate.IssueNumber > issueNumber)
               .OrderBy(predicate => predicate.IssueNumber)
               .FirstOrDefault();
            return Task.FromResult(lotteryIssue);
        }

        public Task UpdateDrawNumber(int lotteryIssueId, string drawNumber)
        {
            throw new NotImplementedException();
        }

        public Task UpdateLotteryIssue(LotteryIssue issueInformation)
        {
            throw new NotImplementedException();
        }
    }
}
