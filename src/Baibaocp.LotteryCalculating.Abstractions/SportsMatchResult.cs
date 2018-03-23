using System;
using System.Collections.Generic;
using System.Text;

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
    }

    public class SportsMatchResult
    {
        internal SportsMatchResult(string halfScore, string finalScore, sbyte letBallNumber)
        {
            string[] halfResult = halfScore.Split(':');
            string[] finalResult = finalScore.Split(':');
            HalfScore = new ScoreValue(int.Parse(halfResult[0]), int.Parse(halfResult[1]));
            FinalScore = new ScoreValue(int.Parse(finalResult[0]), int.Parse(finalResult[1]));
            LetBallNumber = letBallNumber;
        }

        public sbyte LetBallNumber { get; }

        public ScoreValue HalfScore { get; }

        public ScoreValue FinalScore { get; }
    }
}
