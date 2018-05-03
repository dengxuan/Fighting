using Baibaocp.LotteryOrdering.ApplicationServices.Abstractions;
using Baibaocp.LotteryOrdering.Core.Entities.Merchantes;
using Baibaocp.Storaging.Entities;
using Baibaocp.Storaging.Entities.Lotteries;
using Baibaocp.Storaging.Entities.Merchants;
using Fighting.Abstractions;
using Fighting.ApplicationServices.Abstractions;
using Fighting.Caching.Abstractions;
using Fighting.Extensions;
using Fighting.Storaging;
using Fighting.Storaging.Repositories.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.ApplicationServices
{
    public class OrderingApplicationService : ApplicationService, IOrderingApplicationService
    {

        private readonly StorageOptions _options;

        private readonly IIdentityGenerater _identityGenerater;

        private readonly ILogger<OrderingApplicationService> _logger;

        private readonly IRepository<LotteryPhase, int> _lotteryPhaseRepository;

        private readonly IRepository<LotterySportsMatch, long> _lotterySportsMatchRepository;

        private readonly IRepository<LotteryMerchanteOrder, string> _orderingReoository;


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

        public async Task<(LotteryMerchanteOrder order, TimeSpan? delay)> CreateAsync(string lvpOrderId, long? lvpUserId, string lvpVenderId, int lotteryId, int lotteryPlayId, int? issueNumber, string investCode, bool investType, short investCount, byte investTimes, int investAmount)
        {
            string id = _identityGenerater.Generate().ToString();
            DateTime expectedBonusTime = DateTime.Now;
            DateTime currentTime = DateTime.Now;
            TimeSpan? delayTime = null;
            if (issueNumber.HasValue && issueNumber != 0)
            {
                var phaseNumber = await _lotteryPhaseRepository.FirstOrDefaultAsync(predicate => predicate.LotteryId == lotteryId && predicate.IssueNumber == issueNumber);
                expectedBonusTime = phaseNumber.EndTime;
                if (lotteryId < 100)
                {
                    expectedBonusTime = expectedBonusTime.AddHours(1.5);

                    /*低频数字彩票0点到12点不销售*/
                    if (currentTime.Hour < 9)
                    {
                        delayTime = currentTime.Date.AddHours(9) - currentTime;
                    }
                }
                else
                {
                    /* 高频彩票期后30秒开始投注出票*/
                    if (phaseNumber.StartTime > currentTime)
                    {
                        delayTime = (phaseNumber.StartTime - currentTime).Add(TimeSpan.FromSeconds(30));
                    }
                }
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
                /* 竞彩周日，周一 1点到9点，不销售，其他时间0点到9点不销售*/
                if (currentTime.Hour < 9)
                {
                    if ((currentTime.DayOfWeek == DayOfWeek.Sunday || currentTime.DayOfWeek == DayOfWeek.Monday) && currentTime.Hour > 1)
                    {
                        delayTime = currentTime.Date.AddHours(9) - currentTime;
                    }
                    else
                    {
                        delayTime = currentTime.Date.AddHours(9) - currentTime;
                    }
                }
            }

            var order = await _orderingReoository.InsertAsync(new LotteryMerchanteOrder
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
            return (order, delayTime);
        }

        public async Task UpdateAsync(LotteryMerchanteOrder order)
        {
            await _orderingReoository.UpdateAsync(order);
        }

        public async Task<LotteryMerchanteOrder> TicketedAsync(string oderId, string ldpVenderId, string ticketedNumber, DateTime ticketedTime, string ticketOdds = default(string))
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(oderId);
            if (order != null)
            {
                order.TicketedNumber = ticketedNumber;
                order.TicketedTime = ticketedTime;
                order.TicketedOdds = ticketOdds;
                order.LdpVenderId = ldpVenderId;
                order.Status = (int)OrderStatus.TicketDrawing;
                return await _orderingReoository.UpdateAsync(order);
            }
            else
            {
                _logger.LogInformation($"出票成功 订单不存在:[{oderId}-{ldpVenderId}-{ticketedNumber}-{ticketOdds}]");
            }
            return null;
        }

        public async Task<LotteryMerchanteOrder> WinningAsync(string orderId, int amount, int aftertaxBonusAmount)
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(orderId);
            order.BonusAmount = amount;
            order.AftertaxBonusAmount = aftertaxBonusAmount;
            order.Status = (int)OrderStatus.TicketWinning;
            return await _orderingReoository.UpdateAsync(order);
        }

        public async Task<LotteryMerchanteOrder> RejectedAsync(string orderId)
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(orderId);
            if (order != null)
            {
                order.Status = (int)OrderStatus.TicketFailed;
                return await _orderingReoository.UpdateAsync(order);
            }
            else
            {
                _logger.LogInformation($"出票失败订单不存在:[{orderId}]");
            }
            return order;
        }

        public async Task<LotteryMerchanteOrder> LoseingAsync(string orderId)
        {
            var order = await _orderingReoository.FirstOrDefaultAsync(orderId);
            order.Status = (int)OrderStatus.TicketLosing;
            return await _orderingReoository.UpdateAsync(order);
        }
    }
}
