using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Lotteries
{
    public class LotteryPhaseBonusManager
    {
        private readonly IRepository<LotteryPhaseBonus, int> _lotteryIssueBonusesRepository;

        public virtual IQueryable<LotteryPhaseBonus> Bonuses { get { return _lotteryIssueBonusesRepository.GetAll(); } }

        public LotteryPhaseBonusManager(IRepository<LotteryPhaseBonus, int> bonusesRepository)
        {
            this._lotteryIssueBonusesRepository = bonusesRepository;
        }

        public async Task CreateIssueBonus(LotteryPhaseBonus BbcpIssueBonus)
        {
            await _lotteryIssueBonusesRepository.InsertAsync(BbcpIssueBonus);
        }

        public async Task UpdateIssueBonus(LotteryPhaseBonus bbcpIssueBonus)
        {
            await _lotteryIssueBonusesRepository.UpdateAsync(bbcpIssueBonus);
        }
    }
}
