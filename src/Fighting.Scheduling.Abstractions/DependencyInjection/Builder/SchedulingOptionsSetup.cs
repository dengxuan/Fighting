using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fighting.Scheduling.DependencyInjection.Builder
{
    internal class SchedulingOptionsSetup : ConfigureOptions<SchedulingOptions>
    {
        public SchedulingOptionsSetup() : base(ConfigureOptions)
        {
        }

        private static void ConfigureOptions(SchedulingOptions options)
        {
        }
    }
}
