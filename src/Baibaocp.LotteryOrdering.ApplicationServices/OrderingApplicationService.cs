using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using Fighting.Abstractions;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Extensions;
using Fighting.Storaging;
using Fighting.Storaging.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices
{
    public class OrderingApplicationService : ApplicationService, IOrderingApplicationService
    {

        private readonly StorageOptions _options;

        private readonly IIdentityGenerater _identityGenerater;

        private readonly ILogger<OrderingApplicationService> _logger;

        private readonly IRepository<LotteryPhase, int> _lotteryPhaseRepository;

        private readonly IRepository<LotteryMerchanteOrder, string> _orderingReoository;

        private readonly IRepository<LotterySportsMatch, long> _lotterySportsMatchRepository;


        public OrderingApplicationService(StorageOptions options, IRepository<LotteryPhase, int> lotteryPhaseRepository, IRepository<LotteryMerchanteOrder, string> orderingReoository, IRepository<LotterySportsMatch, long> lotterySportsMatchRepository, ILogger<OrderingApplicationService> logger, IIdentityGenerater identityGenerater, ICacheManager cacheManager) : base(cacheManager)
        {
            _options = options;
            _logger = logger;
            _identityGenerater = identityGenerater;
            _lotteryPhaseRepository = lotteryPhaseRepository;
            _orderingReoository = orderingReoository;
            _lotterySportsMatchRepository = lotterySportsMatchRepository;
        }

        public async Task<LotteryMerchanteOrder> FindOrderAsync(string id)
        {
            return await _orderingReoository.FirstOrDefaultAsync(id);
        }

        public async Task<LotteryMerchanteOrder> CreateAsync(string lvpOrderId, long? lvpUserId, string lvpVenderId, int lotteryId, int lotteryPlayId, int? issueNumber, string investCode, bool investType, short investCount, byte investTimes, int investAmount)
        {
            string id = _identityGenerater.Generate().ToString();
            DateTime expectedBonusTime = DateTime.Now;
            if (issueNumber.HasValue)
            {
                var phaseNumber = await _lotteryPhaseRepository.FirstOrDefaultAsync(predicate => predicate.LotteryId == lotteryId && predicate.IssueNumber == issueNumber);
                expectedBonusTime = phaseNumber.EndTime;
            }
            else
            {
                string[] investMatches = investCode.Split(new string[] { "^" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var item in investMatches)
                {
                    string[] items = item.Split('|');
                    long matchId = long.Parse($"{ items[0] }{ items[1] }{ items[2]}");

                    LotterySportsMatch lotterySportsMatch = await _lotterySportsMatchRepository.FirstOrDefaultAsync(matchId);
                    if (expectedBonusTime < lotterySportsMatch.StartTime)
                    {
                        expectedBonusTime = lotterySportsMatch.StartTime;
                    }
                }
                expectedBonusTime = expectedBonusTime.AddHours(3);
            }

            return await _orderingReoository.InsertAsync(new LotteryMerchanteOrder
            {
                Id = id,
                LvpOrderId = lvpOrderId,
                LotteryBuyerId = 619,
                LvpUserId = lvpUserId,
                LvpVenderId = lvpVenderId,
                LotteryId = lotteryId,
                LotteryPlayId = lotteryPlayId,
                IssueNumber = issueNumber,
                InvestCode = investCode,
                InvestType = investType,
                InvestCount = investCount,
                InvestTimes = investTimes,
                InvestAmount = investAmount,
                ExpectedBonusTime = expectedBonusTime,
                Status = (int)OrderStatus.Succeed,
                CreationTime = DateTime.Now
            });
        }


        //public async Task UpdateAsync(TicketedMessage message)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
        //    {
        //        using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `LdpVenderId`=@LdpVenderId, `ChannelOrderId`=@LdpOrderId, `TicketOdds`=@TicketOdds, `Status` = @Status WHERE `Id`=@Id", new
        //            {
        //                Id = message.LvpOrder.LvpOrderId,
        //                LdpVenderId = message.LdpVenderId,
        //                LdpOrderId = message.LdpOrderId,
        //                TicketOdds = message.TicketOdds,
        //                Status = message.Status
        //            });
        //            if (message.Status == OrderStatus.TicketDrawing)
        //            {
        //                /* 出票成功，上游扣款，增加出票金额 */
        //                await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` - @OrderAmount, `OutTicketMoney` = `OutTicketMoney` + @OrderAmount WHERE `Id` = @Id;", new
        //                {
        //                    Id = message.LdpVenderId,
        //                    OrderAmount = message.LvpOrder.InvestAmount
        //                });
        //                await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
        //                {
        //                    Id = _identityGenerater.Generate(),
        //                    ChannelId = message.LdpVenderId,
        //                    LotteryId = message.LvpOrder.LotteryId,
        //                    OrderId = message.LdpOrderId,
        //                    Amount = message.LvpOrder.InvestAmount,
        //                    Status = 3000,
        //                    CreationTime = DateTime.Now
        //                });

        //                /* 出票成功，下游增加出票金额 */
        //                await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` - @OrderAmount, `OutTicketMoney` = `OutTicketMoney` + @OrderAmount WHERE `Id` = @Id;", new
        //                {
        //                    Id = message.LvpOrder.LvpVenderId,
        //                    OrderAmount = message.LvpOrder.InvestAmount
        //                });
        //                await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
        //                {
        //                    Id = _identityGenerater.Generate(),
        //                    ChannelId = message.LvpOrder.LvpVenderId,
        //                    LotteryId = message.LvpOrder.LotteryId,
        //                    OrderId = message.LvpOrder.LvpOrderId,
        //                    Amount = message.LvpOrder.InvestAmount,
        //                    Status = 2000,
        //                    CreationTime = DateTime.Now
        //                });
        //            }
        //            trans.Complete();
        //        }
        //    }
        //}

        //public async Task UpdateAsync(AwardedMessage message)
        //{
        //    using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
        //    {
        //        using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
        //        {
        //            if (message.Status == OrderStatus.TicketWinning)
        //            {
        //                    await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `BonusAmount`=@BonusAmount, `AfterTaxBonusAmount`=@AftertaxBonusAmount, `Status` = `Status` | @Status WHERE `Id`=@Id", new
        //                    {
        //                        Id = message.LvpOrder.LvpOrderId,
        //                        BonusAmount = message.BonusAmount,
        //                        AftertaxBonusAmount = message.AftertaxAmount,
        //                        Status = OrderStatus.TicketWinning
        //                    });

        //                    // ldp
        //                    await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` + @Amount, `RewardMoney` = `RewardMoney` + @Amount WHERE `Id` = @Id;", new
        //                    {
        //                        Id = message.LdpVenderId,
        //                        Amount = message.BonusAmount
        //                    });
        //                    await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
        //                    {
        //                        Id = _identityGenerater.Generate(),
        //                        ChannelId = message.LdpVenderId,
        //                        LotteryId = message.LvpOrder.LotteryId,
        //                        OrderId = message.LvpOrder.LvpOrderId,
        //                        Amount = message.BonusAmount,
        //                        Status = OrderStatus.TicketWinning,
        //                        CreationTime = DateTime.Now
        //                    });

        //                    //lvp
        //                    await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` + @Amount, `RewardMoney` = `RewardMoney` + @Amount WHERE `Id` = @Id;", new
        //                    {
        //                        Id = message.LvpOrder.LvpVenderId,
        //                        Amount = message.BonusAmount
        //                    });
        //                    await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
        //                    {
        //                        Id = _identityGenerater.Generate(),
        //                        ChannelId = message.LvpOrder.LvpVenderId,
        //                        LotteryId = message.LvpOrder.LotteryId,
        //                        OrderId = message.LvpOrder.LvpOrderId,
        //                        Amount = message.BonusAmount,
        //                        Status = OrderStatus.TicketWinning,
        //                        CreationTime = DateTime.Now
        //                    });
        //            }
        //            else if (message.Status == OrderStatus.TicketLosing)
        //            {
        //                await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `Status` = `Status` = @Status WHERE `Id`=@Id", new
        //                {
        //                    Id = message.LvpOrder.LvpOrderId,
        //                    Status = OrderStatus.TicketLosing
        //                });
        //            }
        //            transaction.Complete();
        //        }
        //    }
        //    //ICache cacher = _cacheManager.GetCache("LotteryVender.Orders");
        //    //await cacher.SetAsync(order.Id, Task.FromResult(order));
        //}

        public async Task UpdateAsync(LotteryMerchanteOrder order)
        {
            await _orderingReoository.UpdateAsync(order);
        }

        public async Task<LotteryMerchanteOrder> TicketedAsync(long ldpOrderId, string ldpVenderId, string ticketedNumber, DateTime ticketedTime, string ticketOdds = default(string))
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(ldpOrderId.ToString());
            order.TicketedNumber = ticketedNumber;
            order.TicketedTime = ticketedTime;
            order.TicketedOdds = ticketOdds;
            order.LdpVenderId = ldpVenderId;
            order.Status = (int)OrderStatus.TicketDrawing;
            return await _orderingReoository.UpdateAsync(order);
        }

        public async Task<LotteryMerchanteOrder> WinningAsync(long ldpOrderId, int amount, int aftertaxBonusAmount)
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(ldpOrderId.ToString());
            order.BonusAmount = amount;
            order.AftertaxBonusAmount = aftertaxBonusAmount;
            order.Status = (int)OrderStatus.TicketWinning;
            return await _orderingReoository.UpdateAsync(order);
        }

        public async Task<LotteryMerchanteOrder> RejectedAsync(long ldpOrderId)
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(ldpOrderId.ToString());
            order.Status = (int)OrderStatus.TicketFailed;
            return await _orderingReoository.UpdateAsync(order);
        }

        public Task<LotteryMerchanteOrder> LoseingAsync(long lvpOrderId)
        {
            throw new NotImplementedException();
        }
    }
}
