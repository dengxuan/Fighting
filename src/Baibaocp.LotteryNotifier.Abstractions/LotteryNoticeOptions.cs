using System;
using System.Collections.Generic;

namespace Baibaocp.LotteryNotifier
{
    public class LotteryNoticeOptions
    {
        public List<NoticeConfiguration> Configures { get; } = new List<NoticeConfiguration>();

        public Dictionary<Type, Type> Mappings { get; } = new Dictionary<Type, Type>();
    }
}
