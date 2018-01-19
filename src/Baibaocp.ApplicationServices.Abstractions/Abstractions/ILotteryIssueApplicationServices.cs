using Baibaocp.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ILotteryIssueApplicationServices
    {
        Task<List<LotteryIssue>> FindLotteryIssues(int lotteryId, int skip = 0, int limit = 10);

        Task<LotteryIssue> FindLotteryIssue(int issueNumber);

        Task UpdateLotteryIssue(LotteryIssue issueInformation);

        Task UpdateDrawNumber(int lotteryIssueId, string drawNumber);

        Task<LotteryIssue> NextLotteryIssueInformation(int lotteryId, int issueNumber);
    }
}
