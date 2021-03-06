﻿using Baibaocp.LotteryCalculating;
using Baibaocp.LotteryCalculating.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Abstractions;
using Baibaocp.LotteryDispatching.MessageServices.Messages;
using Baibaocp.LotteryNotifier.MessageServices.Abstractions;
using Baibaocp.LotteryNotifier.MessageServices.Messages;
using Baibaocp.LotteryOrdering.MessageServices.Messages;
using Fighting.Extensions.UnitOfWork.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Baibaocp.LotteryOrdering.Scheduling.Abstractions
{
    public class LotteryAwardingScheduler : ILotteryAwardingScheduler
    {
        private readonly IServiceProvider _iocResolver;

        private readonly ILogger<LotteryAwardingScheduler> _logger;

        private readonly IDispatchQueryingMessageService _dispatchQueryingMessageService;

        private readonly ILotteryNoticingMessagePublisher _lotteryNoticingMessagePublisher;

        private readonly ILotteryCalculatorFactory _lotteryCalculatorFactory;

        public LotteryAwardingScheduler(IServiceProvider iocResolver, IDispatchQueryingMessageService dispatchQueryingMessageService, ILotteryNoticingMessagePublisher lotteryNoticingMessagePublisher, ILotteryCalculatorFactory lotteryCalculatorFactory, ILogger<LotteryAwardingScheduler> logger)
        {
            _iocResolver = iocResolver;
            _logger = logger;
            _dispatchQueryingMessageService = dispatchQueryingMessageService;
            _lotteryCalculatorFactory = lotteryCalculatorFactory;
            _lotteryNoticingMessagePublisher = lotteryNoticingMessagePublisher;
        }

        public async Task<bool> RunAsync(AwardingScheduleArgs args)
        {
            IUnitOfWorkManager unitOfWorkManager = _iocResolver.GetRequiredService<IUnitOfWorkManager>();
            using (var uow = unitOfWorkManager.Begin())
            {
                ILotteryCalculator lotteryCalculator = await _lotteryCalculatorFactory.GetLotteryCalculatorAsync(args.LdpOrderId);
                Handle handle = await lotteryCalculator.CalculateAsync();
                _logger.LogInformation("Execute Awarding Scheduler: {0}-{1}-{2}", args.LdpMerchanerId, args.LdpOrderId, handle);
                uow.Complete();
                switch (handle)
                {
                    case Handle.Winner:
                        await _dispatchQueryingMessageService.PublishAsync(args.LdpOrderId, args.LdpMerchanerId, args.LvpOrderId, args.LvpMerchanerId, args.LotteryId, QueryingTypes.Awarding);
                        return true;
                    case Handle.Losing:
                        await _lotteryNoticingMessagePublisher.PublishAsync($"LotteryOrdering.Awarded.{args.LvpMerchanerId}", new NoticeMessage<LotteryAwarded>(args.LdpOrderId, args.LdpMerchanerId, new LotteryAwarded
                        {
                            LvpOrderId = args.LvpOrderId,
                            LvpMerchanerId = args.LvpMerchanerId,
                            BonusAmount = 0,
                            AftertaxBonusAmount = 0,
                            AwardingType = LotteryAwardingTypes.Loseing,
                        }));
                        return true;
                    case Handle.Waiting: return false;
                }
                return false;
            }
        }
    }
}
