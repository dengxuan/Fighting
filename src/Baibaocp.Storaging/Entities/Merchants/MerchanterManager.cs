using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Merchants
{
    public class MerchanterManager
    {
        private readonly IRepository<Merchanter> _merchanterRepository;

        public virtual IQueryable<Merchanter> Merchanters { get { return _merchanterRepository.GetAll(); } }

        public MerchanterManager(IRepository<Merchanter> merchanterRepository)
        {
            _merchanterRepository = merchanterRepository;
        }

        public async Task CreateMerchanter(Merchanter channel)
        {
            await _merchanterRepository.InsertAsync(channel);
        }

        public async Task UpdateMerchanter(Merchanter channel)
        {
            await _merchanterRepository.UpdateAsync(channel);
        }
    }
}
