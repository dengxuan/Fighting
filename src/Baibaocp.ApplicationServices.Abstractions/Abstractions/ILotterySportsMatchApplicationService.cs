using Baibaocp.Storaging.Entities.Lotteries;
using System.Threading.Tasks;

namespace Baibaocp.ApplicationServices.Abstractions
{
    public interface ILotterySportsMatchApplicationService
    {
        Task CreateMatchAsync(LotterySportsMatch match);

        Task<LotterySportsMatch> FindMatchAsync(long matchId);

        Task UpdateMatchAsync(LotterySportsMatch match);
    }
}
