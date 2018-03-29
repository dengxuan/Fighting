using Baibaocp.LotteryTrading.TradeLogging.Subscribers;
using Fighting.DependencyInjection.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryTrading.TradeLogging.DependencyInjection
{
    public static class TradeLoggingFightBuilderExtensions
    {

        public static FightBuilder ConfigureTradeLogging(this FightBuilder fightBuilder)
        {
            fightBuilder.Services.AddSingleton<IHostedService, LotteryAwardingMessageSubscriber>();
            fightBuilder.Services.AddSingleton<IHostedService, LotteryTicketingMessageSubscriber>();
            return fightBuilder;
        }
    }
}
