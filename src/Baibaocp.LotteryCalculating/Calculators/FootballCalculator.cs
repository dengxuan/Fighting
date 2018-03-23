using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities.Extensions;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Baibaocp.LotteryCalculating.Calculators
{
    public class FootballCalculator : SprotsLotteryCalculator
    {
        const string GAME_CANCLE = "-1";

        public FootballCalculator(ILotterySportsMatchApplicationService sportsMatchApplicationService, LotteryMerchanteOrder lotteryMerchanteOrder) : base(sportsMatchApplicationService, lotteryMerchanteOrder)
        {
        }

        private string CalculateGameResult(int lotteryId, SportsMatchResult matchResult)
        {
            if (matchResult.FinalScore.Home == -1 && matchResult.FinalScore.Guest == -1)
            {
                return GAME_CANCLE;
            }
            switch (lotteryId)
            {
                case 20201:
                case 20206:
                    {
                        int homeScore = matchResult.FinalScore.Home + matchResult.LetBallNumber;
                        int guestScore = matchResult.FinalScore.Guest;
                        if (homeScore > guestScore)
                        {
                            return "3";
                        }
                        else if (homeScore == guestScore)
                        {
                            return "1";
                        }
                        else
                        {
                            return "0";
                        }
                    }
                case 20202:
                    {
                        string home = matchResult.FinalScore.Home.ToString();
                        string guest = matchResult.FinalScore.Guest.ToString();
                        return string.Join("", matchResult.FinalScore.Home > 5 ? "9" : home, matchResult.FinalScore.Guest > 5 ? "9" : guest);
                    }
                default:
                    break;
            }
            return null;
        }

        public override async Task<Handle> CalculateAsync()
        {
            string[] investMatches = LotteryMerchanteOrder.InvestCode.Trim('^').Split('^');

            int minWinnerCount = int.Parse($"N{LotteryMerchanteOrder.LotteryPlayId}".ToJingcaiLottery());
            Stack<decimal> winnerOdds = new Stack<decimal>();
            int completedCount = 0;
            for (int i = 0; i < investMatches.Length; i++)
            {
                string[] codes = investMatches[i].Split('|');
                string matchId = string.Join("", codes[0], codes[1], codes[2]);
                var matchResult = await GetMatchResultAsync(long.Parse(matchId));
                if (matchResult == null)
                {
                    return Handle.Waiting;
                }
                string result = CalculateGameResult(LotteryMerchanteOrder.LotteryId == 20205 ? int.Parse(codes[3]) : LotteryMerchanteOrder.LotteryId, matchResult);
                if (result == null)
                {
                    return Handle.Waiting;
                }
                completedCount = completedCount + 1;
                if (result == GAME_CANCLE)
                {
                    winnerOdds.Push(1);
                    continue;
                }
                //201803073003@0|3*1.47#1*3.95#0*5.10#^201803073006@0|3*2.65#1*3.00#0*2.40#^201803073009@0|3*2.37#1*3.10#0*2.60#^201803073012@0|3*1.35#1*4.50#0*6.00#^201803073011@0|3*2.20#1*3.15#0*2.80#
                //201803235004-20201@0|3*2.90#1*3.22#0*2.11#^201803235003-20201@0|3*1.80#1*3.35#0*3.63#^201803235002-20206@-1|3*2.53#1*3.15#0*2.40#
                string[] investCodes = codes[4].Trim('#').Split('#');

                for (int j = 0; j < investCodes.Length; j++)
                {
                    string[] codeAndOdds = investCodes[j].Split('*');
                    if (codeAndOdds[0] == result)
                    {
                        winnerOdds.Push(decimal.Parse(codeAndOdds[1]));
                        break;
                    }
                }
            }
            if (completedCount < minWinnerCount)
            {
                return Handle.Waiting;
            }
            //中奖赛事与串关数量
            if (winnerOdds.Count >= minWinnerCount)
            {
                return Handle.Winner;
            }
            return Handle.Losing;
        }
    }
}
