using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Storaging.Entities.Merchants
{
    public class MerchanterAccountLoggingManager
    {

        private readonly IRepository<MerchanterAccountLogging, long> _accountLoggingRepositiry;

        public virtual IQueryable<MerchanterAccountLogging> AccountLoggings { get { return _accountLoggingRepositiry.GetAll(); } }

        public MerchanterAccountLoggingManager(IRepository<MerchanterAccountLogging, long> accountDetailingRepositiry)
        {
            _accountLoggingRepositiry = accountDetailingRepositiry;
        }

        public async Task CreateAccountLogging(MerchanterAccountLogging accountLogging)
        {
            await _accountLoggingRepositiry.InsertAsync(accountLogging);
        }
    }
}
