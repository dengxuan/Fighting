using Baibaocp.Storaging.Entities.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ISportsMatchApplicationService
    {
        Task CreateMatchAsync(LotterySportsMatch match);

        Task<LotterySportsMatch> FindMatchAsync(int matchId);

        Task UpdateMatchAsync(LotterySportsMatch match);
    }
}
