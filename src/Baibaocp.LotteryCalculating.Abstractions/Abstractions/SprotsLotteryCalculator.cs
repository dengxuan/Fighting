using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Abstractions
{
    /// <summary>
    /// 竞技型彩票
    /// </summary>
    public abstract class SprotsLotteryCalculator : LotteryCalculator
    {

        private readonly IDictionary<int, (int home, int guest)?> _results = new Dictionary<int, (int home, int guest)?>();

        public SprotsLotteryCalculator(IServiceProvider iocResolver, LotteryMerchanteOrder lotteryMerchanteOrder) : base(iocResolver, lotteryMerchanteOrder)
        {
        }

        protected async Task<SportsMatchResult> GetMatchResultAsync(long matchId)
        {
            ILotterySportsMatchApplicationService sportsMatchApplicationService = IocResolver.GetRequiredService<ILotterySportsMatchApplicationService>();
            var lotterySportsMatch = await sportsMatchApplicationService.FindMatchAsync(matchId);
            if (string.IsNullOrEmpty(lotterySportsMatch.HalfScore) || string.IsNullOrEmpty(lotterySportsMatch.Score))
            {
                return null;
            }
            return new SportsMatchResult(lotterySportsMatch.HalfScore, lotterySportsMatch.Score);
        }
    }
}
