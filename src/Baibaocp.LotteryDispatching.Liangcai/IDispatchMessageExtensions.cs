using Baibaocp.LotteryDispatching.MessageServices.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryDispatcher.Liangcai
{
    internal static class IDispatchMessageExtensions
    {
        private static string ToCommand(this QueryingTypes querying)
        {
            switch (querying)
            {
                case QueryingTypes.Awarding: return "103";
                case QueryingTypes.Ticketing: return "102";
            }
            throw new Exception();
        }
        public static string ToCommand(this IDispatchMessage message)
        {
            switch (message)
            {
                case OrderingDispatchMessage ordering: return "101";
                case QueryingDispatchMessage querying: return querying.QueryingType.ToCommand();
            }
            throw new Exception();
        }
    }
}
