using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using Fighting.DependencyInjection.Builder;

namespace Baibaocp.Storaging.Entities.Merchants
{
    [TransientDependency]
    public class MerchanterAccountLoggingManager
    {

        private readonly IRepository<MerchanterAccountLogging, long> _accountLoggingRepositiry;

        public virtual IQueryable<MerchanterAccountLogging> AccountLoggings { get { return _accountLoggingRepositiry.GetAll(); } }

        public MerchanterAccountLoggingManager(IRepository<MerchanterAccountLogging, long> accountLoggingRepositiry)
        {
            _accountLoggingRepositiry = accountLoggingRepositiry;
        }

        public async Task CreateAccountLogging(MerchanterAccountLogging accountLogging)
        {
            await _accountLoggingRepositiry.InsertAsync(accountLogging);
        }
    }
}
