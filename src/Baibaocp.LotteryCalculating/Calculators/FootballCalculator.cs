using Baibaocp.ApplicationServices.Abstractions;
using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities.Extensions;
using System;
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

        private string CalculateGameResult(int lotteryId, sbyte letBallCount, SportsMatchResult matchResult)
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
                        int homeScore = matchResult.FinalScore.Home + letBallCount;
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

        protected string ResolveMatchId(string investMatch)
        {
            int index = investMatch.IndexOfAny(new char[] { '-', '@' });
            return investMatch.Substring(0, index);
        }

        protected int? ResolveLotteryId(string investMatch)
        {
            int startIndex = investMatch.IndexOf('-');
            if (startIndex == -1)
            {
                return null;
            }
            int endIndex = investMatch.IndexOf('@');
            return int.Parse(investMatch.Substring(startIndex + 1, endIndex - startIndex - 1));
        }

        protected sbyte ResolveLetBallCount(string investMatch)
        {
            int startIndex = investMatch.IndexOf('@');
            int endIndex = investMatch.IndexOf('|');
            return sbyte.Parse(investMatch.Substring(startIndex + 1, endIndex - startIndex - 1));
        }

        protected string[] ResolveInvestCodes(string investMatch)
        {
            int index = investMatch.IndexOf('|');
            string codes = investMatch.Substring(index + 1);
            return codes.Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 混合投注
        /// </summary>
        /// <param name="investCode"></param>
        /// <returns></returns>
        protected string ResolveMixedInvested(string investCode, SportsMatchResult matchResult)
        {
            return null;
        }

        /// <summary>
        /// 胜平负
        /// </summary>
        /// <param name="investCode"></param>
        /// <returns></returns>
        protected string ResolveVictoryLevels(string investCode, SportsMatchResult matchResult)
        {
            int homeScore = matchResult.FinalScore.Home;
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

        /// <summary>
        /// 让球胜平负
        /// </summary>
        /// <param name="investCode"></param>
        /// <returns></returns>
        protected string ResolveLetBallVictoryLevels(sbyte letBallCount, string investCode, SportsMatchResult matchResult)
        {
            int homeScore = matchResult.FinalScore.Home + letBallCount;
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


        /// <summary>
        /// 比分
        /// </summary>
        /// <param name="investCode"></param>
        /// <returns></returns>
        protected string ResolveScoreInvested(string investCode, SportsMatchResult matchResult)
        {
            string home = matchResult.FinalScore.Home.ToString();
            string guest = matchResult.FinalScore.Guest.ToString();
            return string.Join("", matchResult.FinalScore.Home > 5 ? "9" : home, matchResult.FinalScore.Guest > 5 ? "9" : guest);
        }

        /// <summary>
        /// 半全场
        /// </summary>
        /// <param name="investCode"></param>
        /// <returns></returns>
        protected string ResolveDoubleResultInvested(string investCode, SportsMatchResult matchResult)
        {
            return null;
        }

        /// <summary>
        /// 总进球
        /// </summary>
        /// <param name="investCode"></param>
        /// <returns></returns>
        protected string ResolveGoalsInvested(string investCode, SportsMatchResult matchResult)
        {
            return null;
        }

        public override async Task<Handle> CalculateAsync()
        {
            string[] investMatches = LotteryMerchanteOrder.TicketedOdds.Split(new string[] { "#^" }, StringSplitOptions.RemoveEmptyEntries);

            int minWinnerCount = int.Parse($"N{LotteryMerchanteOrder.LotteryPlayId}".ToJingcaiLottery());
            Stack<decimal> winnerOdds = new Stack<decimal>();
            int completedCount = 0;
            for (int i = 0; i < investMatches.Length; i++)
            {
                string investMatch = investMatches[i];
                string matchId = ResolveMatchId(investMatch);
                var matchResult = await GetMatchResultAsync(long.Parse(matchId));
                if (matchResult == null)
                {
                    return Handle.Waiting;
                }
                int lotteryId = ResolveLotteryId(investMatch) ?? LotteryMerchanteOrder.LotteryId;
                string result = null;
                switch (lotteryId)
                {
                    case 20201:
                        {
                            result = matchResult.FinalScore.VictoryLevels(); break;
                        }
                    case 20206:
                        {
                            sbyte letBallCount = ResolveLetBallCount(investMatch);
                            result = matchResult.FinalScore.VictoryLevels(letBallCount); break;
                        }
                    case 20202:
                        {
                            result = matchResult.FinalScore.Score(); break;
                        }
                    default: break;
                }
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
                string[] investCodes = ResolveInvestCodes(investMatch);

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
