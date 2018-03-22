using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Merchants
{

    [TransientDependency]
    public class MerchanterManager
    {
        private readonly IRepository<Merchanter> _merchanterRepository;

        public virtual IQueryable<Merchanter> Merchanters { get { return _merchanterRepository.GetAll(); } }

        public MerchanterManager(IRepository<Merchanter> merchanterRepository)
        {
            _merchanterRepository = merchanterRepository;
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task CreateMerchanter(Merchanter channel)
        {
            await _merchanterRepository.InsertAsync(channel);
        }

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task UpdateMerchanter(Merchanter channel)
        {
            await _merchanterRepository.UpdateAsync(channel);
        }

        /// <summary>
        /// 增加商户余额
        /// </summary>
        /// <param name="merchanterId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task IncreaseBalance(Merchanter merchanter, int amount)
        {
            merchanter.Balance = merchanter.Balance + amount;
            await UpdateMerchanter(merchanter);
        }

        /// <summary>
        /// 减少商户余额
        /// </summary>
        /// <param name="merchanterId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task DecreaseBalance(Merchanter merchanter, int amount)
        {
            merchanter.Balance = merchanter.Balance - amount;
            await UpdateMerchanter(merchanter);
        }

        /// <summary>
        /// 增加总出票金额
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">出票金额</param>
        /// <returns></returns>
        public async Task IncreaseTicketedAmount(Merchanter merchanter, int amount)
        {
            merchanter.TotalTicketedAmount = merchanter.TotalTicketedAmount + amount;
            await UpdateMerchanter(merchanter);
        }

        /// <summary>
        /// 增加总返奖金额
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">返奖金额</param>
        /// <returns></returns>
        public async Task IncreaseAwardedAmount(Merchanter merchanter, int amount)
        {
            merchanter.TotalAwardedAmount = merchanter.TotalAwardedAmount + amount;
            await UpdateMerchanter(merchanter);
        }
    }
}
