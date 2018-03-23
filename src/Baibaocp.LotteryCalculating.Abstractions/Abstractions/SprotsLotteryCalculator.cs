using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    public abstract class SprotsLotteryCalculator : LotteryCalculator
    {
        private readonly ILotterySportsMatchApplicationService _sportsMatchApplicationService;

        private readonly IDictionary<int, (int home, int guest)?> _results = new Dictionary<int, (int home, int guest)?>();

        public SprotsLotteryCalculator(ILotterySportsMatchApplicationService sportsMatchApplicationService, LotteryMerchanteOrder lotteryMerchanteOrder) : base(lotteryMerchanteOrder)
        {
            _sportsMatchApplicationService = sportsMatchApplicationService;
        }

        protected async Task<SportsMatchResult> GetMatchResultAsync(long matchId)
        {
            var lotterySportsMatch = await _sportsMatchApplicationService.FindMatchAsync(matchId);
            if (string.IsNullOrEmpty(lotterySportsMatch.HalfScore) || string.IsNullOrEmpty(lotterySportsMatch.Score))
            {
                return null;
            }
            return new SportsMatchResult(lotterySportsMatch.HalfScore, lotterySportsMatch.Score, (sbyte)lotterySportsMatch.RqspfRateCount);
        }
    }
}
