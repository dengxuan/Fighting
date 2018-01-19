using Fighting.Storaging.Repositories.Abstractions;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.Core.Foundation.Baibaocp.Channels
{
    public class BbcpChannelManager
    {
        private readonly IRepository<BbcpVender, string> _channelRepository;

        public virtual IQueryable<BbcpVender> Channels { get { return _channelRepository.GetAll(); } }

        public BbcpChannelManager(IRepository<BbcpVender, string> channelRepository)
        {
            _channelRepository = channelRepository;
        }

        public async Task CreateChannel(BbcpVender channel)
        {
            await _channelRepository.InsertAsync(channel);
        }

        public async Task UpdateChannel(BbcpVender channel)
        {
            await _channelRepository.UpdateAsync(channel);
        }
    }
}
