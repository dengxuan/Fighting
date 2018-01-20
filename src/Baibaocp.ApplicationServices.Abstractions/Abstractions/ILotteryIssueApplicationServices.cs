using Baibaocp.Storaging.Entities.Lotteries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ILotteryIssueApplicationServices
    {
        Task<List<LotteryPhase>> FindLotteryPhases(int lotteryId, int skip = 0, int limit = 10);

        Task<LotteryPhase> FindLotteryPhase(int issueNumber);

        Task UpdateLotteryPhase(LotteryPhase lotteryPhase);

        Task UpdateDrawNumber(int id, string drawNumber);

        Task<List<LotteryPhase>> NextLotteryPhases(int lotteryId, int issueNumber, int limit = 10);
    }
}
