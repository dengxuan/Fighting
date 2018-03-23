using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Baibaocp.LotteryCalculating.DependencyInjection.Builder
{
    public class LotteryCalculatingBuilder
    {
        public IServiceCollection Services { get; }

        internal LotteryCalculatingBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        internal void Build()
        {
        }
    }
}
