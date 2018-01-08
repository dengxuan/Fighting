using Baibaocp.Core.Entities;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ISportsMatchApplicationService
    {
        Task CreateMatchAsync(LotterySportsMatchEntity match);

        Task<LotterySportsMatchEntity> FindMatchAsync(int matchId);

        Task UpdateMatchAsync(LotterySportsMatchEntity match);
    }
}
