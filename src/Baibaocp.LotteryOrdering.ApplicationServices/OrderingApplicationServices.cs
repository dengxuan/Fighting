using Baibaocp.Storaging.Entities;
using Baibaocp.LotteryOrdering.Core.Entities;
using Baibaocp.LotteryOrdering.Messages;
using Dapper;
using Fighting.Abstractions;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Storaging;
using Fighting.Storaging.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using Pomelo.Data.MySql;
using System;
using System.Threading.Tasks;
using System.Transactions;

namespace Baibaocp.LotteryOrdering.ApplicationServices
{
    public class OrderingApplicationServices : ApplicationService, IOrderingApplicationService
    {

        private readonly StorageOptions _options;

        private readonly ILogger<OrderingApplicationServices> _logger;

        private readonly IIdentityGenerater _identityGenerater;

        private readonly IRepository<LotteryVenderOrderEntity, string> _orderingReoository;


        public OrderingApplicationServices(StorageOptions options, IRepository<LotteryVenderOrderEntity, string> orderingReoository, ILogger<OrderingApplicationServices> logger, IIdentityGenerater identityGenerater, ICacheManager cacheManager) : base(cacheManager)
        {
            _options = options;
            _logger = logger;
            _identityGenerater = identityGenerater;
            _orderingReoository = orderingReoository;
        }

        public async Task<LotteryVenderOrderEntity> FindOrderAsync(string id)
        {
            return await _orderingReoository.FirstOrDefaultAsync(id);
        }

        public async Task CreateAsync(LvpOrderMessage message)
        {
           await _orderingReoository.InsertAsync(new LotteryVenderOrderEntity
            {
                Id = message.LvpOrderId,
                LotteryBuyerId = 619,
                LvpUserId = message.LvpUserId,
                LvpVenderId = message.LvpVenderId,
                LotteryId = message.LotteryId,
                LotteryPlayId = message.LotteryPlayId,
                IssueNumber = message.IssueNumber,
                InvestCode = message.InvestCode,
                InvestType = message.InvestType,
                InvestCount = message.InvestCount,
                InvestTimes = message.InvestTimes,
                InvestAmount = message.InvestAmount,
                Status = (int)OrderStatus.Succeed,
                CreationTime = DateTime.Now
            });
            //using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            //{
            //    int count = await connection.ExecuteAsync(@"INSERT INTO `BbcpOrders`(`Id`,`LotteryBuyerId`,`LvpUserId`,`LvpVenderId`,`LotteryId`,`LotteryPlayId`,`IssueNumber`,`InvestCode`,`InvestType`,`InvestCount`,`InvestTimes`,`InvestAmount`,`Status`,`CreationTime`)VALUES(@Id,@LotteryBuyerId,@LvpUserId,@LvpVenderId,@LotteryId,@LotteryPlayId,@IssueNumber,@InvestCode,@InvestType,@InvestCount,@InvestTimes,@InvestAmount,@Status,@CreationTime);", new
            //    {
            //        Id = message.LvpOrderId,
            //        LotteryBuyerId = 619,
            //        LvpUserId = message.LvpUserId,
            //        LvpVenderId = message.LvpVenderId,
            //        LotteryId = message.LotteryId,
            //        LotteryPlayId = message.LotteryPlayId,
            //        IssueNumber = message.IssueNumber,
            //        InvestCode = message.InvestCode,
            //        InvestType = message.InvestType,
            //        InvestCount = message.InvestCount,
            //        InvestTimes = message.InvestTimes,
            //        InvestAmount = message.InvestAmount,
            //        Status = OrderStatus.Succeed,
            //        CreationTime = DateTime.Now
            //    });
            //}
        }


        public async Task UpdateAsync(TicketedMessage message)
        {
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                using (TransactionScope trans = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
                {
                    await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `LdpVenderId`=@LdpVenderId, `ChannelOrderId`=@LdpOrderId, `TicketOdds`=@TicketOdds, `Status` = @Status WHERE `Id`=@Id", new
                    {
                        Id = message.LvpOrder.LvpOrderId,
                        LdpVenderId = message.LdpVenderId,
                        LdpOrderId = message.LdpOrderId,
                        TicketOdds = message.TicketOdds,
                        Status = message.Status
                    });
                    if (message.Status == OrderStatus.TicketDrawing)
                    {
                        /* 出票成功，上游扣款，增加出票金额 */
                        await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` - @OrderAmount, `OutTicketMoney` = `OutTicketMoney` + @OrderAmount WHERE `Id` = @Id;", new
                        {
                            Id = message.LdpVenderId,
                            OrderAmount = message.LvpOrder.InvestAmount
                        });
                        await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                        {
                            Id = _identityGenerater.Generate(),
                            ChannelId = message.LdpVenderId,
                            LotteryId = message.LvpOrder.LotteryId,
                            OrderId = message.LdpOrderId,
                            Amount = message.LvpOrder.InvestAmount,
                            Status = 3000,
                            CreationTime = DateTime.Now
                        });

                        /* 出票成功，下游增加出票金额 */
                        await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` - @OrderAmount, `OutTicketMoney` = `OutTicketMoney` + @OrderAmount WHERE `Id` = @Id;", new
                        {
                            Id = message.LvpOrder.LvpVenderId,
                            OrderAmount = message.LvpOrder.InvestAmount
                        });
                        await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                        {
                            Id = _identityGenerater.Generate(),
                            ChannelId = message.LvpOrder.LvpVenderId,
                            LotteryId = message.LvpOrder.LotteryId,
                            OrderId = message.LvpOrder.LvpOrderId,
                            Amount = message.LvpOrder.InvestAmount,
                            Status = 2000,
                            CreationTime = DateTime.Now
                        });
                    }
                    trans.Complete();
                }
            }
        }

        public async Task UpdateAsync(AwardedMessage message)
        {
            using (MySqlConnection connection = new MySqlConnection(_options.DefaultNameOrConnectionString))
            {
                using (TransactionScope transaction = new TransactionScope(TransactionScopeOption.Required, TimeSpan.FromSeconds(30), TransactionScopeAsyncFlowOption.Enabled))
                {
                    if (message.Status == OrderStatus.TicketWinning)
                    {
                            await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `BonusAmount`=@BonusAmount, `AfterTaxBonusAmount`=@AftertaxBonusAmount, `Status` = `Status` | @Status WHERE `Id`=@Id", new
                            {
                                Id = message.LvpOrder.LvpOrderId,
                                BonusAmount = message.BonusAmount,
                                AftertaxBonusAmount = message.AftertaxAmount,
                                Status = OrderStatus.TicketWinning
                            });

                            // ldp
                            await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` + @Amount, `RewardMoney` = `RewardMoney` + @Amount WHERE `Id` = @Id;", new
                            {
                                Id = message.LdpVenderId,
                                Amount = message.BonusAmount
                            });
                            await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                            {
                                Id = _identityGenerater.Generate(),
                                ChannelId = message.LdpVenderId,
                                LotteryId = message.LvpOrder.LotteryId,
                                OrderId = message.LvpOrder.LvpOrderId,
                                Amount = message.BonusAmount,
                                Status = OrderStatus.TicketWinning,
                                CreationTime = DateTime.Now
                            });

                            //lvp
                            await connection.ExecuteAsync("UPDATE `BbcpChannels` SET `RestPreMoney` = `RestPreMoney` + @Amount, `RewardMoney` = `RewardMoney` + @Amount WHERE `Id` = @Id;", new
                            {
                                Id = message.LvpOrder.LvpVenderId,
                                Amount = message.BonusAmount
                            });
                            await connection.ExecuteAsync("INSERT INTO `BbcpChannelAccountDetails`(`Id`,`ChannelId`,`LotteryId`,`OrderId`,`Amount`,`Status`,`CreationTime`)VALUES(@Id,@ChannelId,@LotteryId,@OrderId,@Amount,@Status,@CreationTime)", new
                            {
                                Id = _identityGenerater.Generate(),
                                ChannelId = message.LvpOrder.LvpVenderId,
                                LotteryId = message.LvpOrder.LotteryId,
                                OrderId = message.LvpOrder.LvpOrderId,
                                Amount = message.BonusAmount,
                                Status = OrderStatus.TicketWinning,
                                CreationTime = DateTime.Now
                            });
                    }
                    else if (message.Status == OrderStatus.TicketLosing)
                    {
                        await connection.ExecuteAsync("UPDATE `BbcpOrders` SET `Status` = `Status` = @Status WHERE `Id`=@Id", new
                        {
                            Id = message.LvpOrder.LvpOrderId,
                            Status = OrderStatus.TicketLosing
                        });
                    }
                    transaction.Complete();
                }
            }
            //ICache cacher = _cacheManager.GetCache("LotteryVender.Orders");
            //await cacher.SetAsync(order.Id, Task.FromResult(order));
        }
    }
}
