using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Core.Lotteries
{
    public class BbcpLotteryIssueBonusManager
    {
        private readonly IRepository<BbcpLotteryIssueBonus, int> _lotteryIssueBonusesRepository;

        public virtual IQueryable<BbcpLotteryIssueBonus> Bonuses { get { return _lotteryIssueBonusesRepository.GetAll(); } }

        public BbcpLotteryIssueBonusManager(IRepository<BbcpLotteryIssueBonus, int> bonusesRepository)
        {
            this._lotteryIssueBonusesRepository = bonusesRepository;
        }

        public async Task CreateIssueBonus(BbcpLotteryIssueBonus BbcpIssueBonus)
        {
            await _lotteryIssueBonusesRepository.InsertAsync(BbcpIssueBonus);
        }

        public async Task UpdateIssueBonus(BbcpLotteryIssueBonus bbcpIssueBonus)
        {
            await _lotteryIssueBonusesRepository.UpdateAsync(bbcpIssueBonus);
        }
    }
}
