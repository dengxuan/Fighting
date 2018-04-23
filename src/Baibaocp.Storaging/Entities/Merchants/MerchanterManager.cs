using Fighting.DependencyInjection.Builder;
using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Merchants
{

    [TransientDependency]
    public class MerchanterManager
    {
        private readonly IRepository<Merchanter, string> _merchanterRepository;

        public virtual IQueryable<Merchanter> Merchanters { get { return _merchanterRepository.GetAll(); } }

        public MerchanterManager(IRepository<Merchanter, string> merchanterRepository)
        {
            _merchanterRepository = merchanterRepository;
        }

        /// <summary>
        /// 添加商户
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task CreateAsync(Merchanter channel)
        {
            await _merchanterRepository.InsertAsync(channel);
        }

        /// <summary>
        /// 根据商户编号查询商户
        /// </summary>
        /// <param name="merchanterId">商户编号</param>
        /// <returns></returns>
        public Task<Merchanter> FindMerchanterAsync(string merchanterId)
        {
            var merchanter = Merchanters.Where(predicate => predicate.Id == merchanterId).FirstOrDefault();
            return Task.FromResult(merchanter);
        }

        /// <summary>
        /// 根据商户编号查询商户
        /// </summary>
        /// <param name="merchanterId">商户编号</param>
        /// <returns></returns>
        public Merchanter FindMerchanter(string merchanterId)
        {
            var merchanter = Merchanters.Where(predicate => predicate.Id == merchanterId).FirstOrDefault();
            return merchanter;
        }

        /// <summary>
        /// 更新商户
        /// </summary>
        /// <param name="channel"></param>
        /// <returns></returns>
        public async Task UpdateAsync(Merchanter channel)
        {
            await _merchanterRepository.UpdateAsync(channel);
        }

        /// <summary>
        /// 增加商户余额
        /// </summary>
        /// <param name="merchanterId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task AddBalanceAsync(Merchanter merchanter, int amount)
        {
            merchanter.Balance = merchanter.Balance + amount;
            await UpdateAsync(merchanter);
        }

        /// <summary>
        /// 减少商户余额
        /// </summary>
        /// <param name="merchanterId"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public async Task SubBalanceAsync(Merchanter merchanter, int amount)
        {
            merchanter.Balance = merchanter.Balance - amount;
            await UpdateAsync(merchanter);
        }

        /// <summary>
        /// 增加总出票金额
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">出票金额</param>
        /// <returns></returns>
        public async Task AddTotalTicketedAmount(Merchanter merchanter, int amount)
        {
            merchanter.TotalTicketedAmount = merchanter.TotalTicketedAmount + amount;
            await UpdateAsync(merchanter);
        }

        /// <summary>
        /// 增加总返奖金额
        /// </summary>
        /// <param name="merchanterId">渠道编号</param>
        /// <param name="amount">返奖金额</param>
        /// <returns></returns>
        public async Task SubTotalAwardedAmount(Merchanter merchanter, int amount)
        {
            merchanter.TotalAwardedAmount = merchanter.TotalAwardedAmount + amount;
            await UpdateAsync(merchanter);
        }
    }
}
