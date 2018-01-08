// This file is part of Hangfire.
// Copyright © 2016 Sergey Odinokov.
// 
// Hangfire is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as 
// published by the Free Software Foundation, either version 3 
// of the License, or any later version.
// 
// Hangfire is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public 
// License along with Hangfire. If not, see <http://www.gnu.org/licenses/>.

using System;
using Hangfire;
using Hangfire.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace Fighting.HangfireWorker
{
    public class FightingJobActivator : JobActivator
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public FightingJobActivator([NotNull] IServiceScopeFactory serviceScopeFactory)
        {
            if (serviceScopeFactory == null) throw new ArgumentNullException(nameof(serviceScopeFactory));
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override JobActivatorScope BeginScope(JobActivatorContext context)
        {
            return new FightingJobActivatorScope(_serviceScopeFactory.CreateScope());
        }
        
        [Obsolete]
        public override JobActivatorScope BeginScope()
        {
            return new FightingJobActivatorScope(_serviceScopeFactory.CreateScope());
        }
    }
}