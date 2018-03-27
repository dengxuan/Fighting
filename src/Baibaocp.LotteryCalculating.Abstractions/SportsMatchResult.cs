namespace Baibaocp.LotteryCalculating
{
    public class ScoreValue
    {
        internal ScoreValue(int home, int guest)
        {
            Home = home;
            Guest = guest;
        }

        public int Home { get; set; }

        public int Guest { get; set; }

        /// <summary>
        /// 胜平负结果
        /// </summary>
        /// <param name="letBallCount">让球数</param>
        /// <returns></returns>
        public string VictoryLevels(sbyte letBallCount = 0)
        {
            if (Home + letBallCount > Guest)
            {
                return "3";
            }
            else if (Home + letBallCount == Guest)
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        public string ScoreResult()
        {
            if (Home > 5 && Guest > 2)
            {
                return "90";
            }
            else if (Home == Guest && Home > 3)
            {
                return "99";
            }
            else if (Home > 2 && Guest > 5)
            {
                return "09";
            }
            else
            {
                return string.Join("", Home, Guest);
            }
        }

        public string TotalGoals()
        {
            int count = Home + Guest;
            return string.Format("{0}", count > 7 ? 7 : count);
        }
    }

    public class SportsMatchResult
    {
        internal SportsMatchResult(string halfScore, string finalScore)
        {
            string[] halfResult = halfScore.Split(':');
            string[] finalResult = finalScore.Split(':');
            HalfScore = new ScoreValue(int.Parse(halfResult[0]), int.Parse(halfResult[1]));
            FinalScore = new ScoreValue(int.Parse(finalResult[0]), int.Parse(finalResult[1]));
        }

        public ScoreValue HalfScore { get; }

        public ScoreValue FinalScore { get; }
    }
}
